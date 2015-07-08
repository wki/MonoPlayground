using System;
// using Akka.Actor;
using Akka.Persistence;

namespace AkkaDemo.PersistentSensor
{
    #region State
    public class SensorState
    {
        public int Result { get; set; }
    }
    #endregion

    public class SensorActor : ReceivePersistentActor
    {

        #region Commands
        public class ProvideMeasurement
        {
            public int Result { get; private set; }

            public ProvideMeasurement (int result)
            {
                Result = result;
            }
        }
        #endregion

        #region Events
        public class MeasurementProvided
        {
            public int Result { get; private set; }

            public MeasurementProvided (int result)
            {
                Result = result;
            }
        }
        #endregion


        // FIXME: must become dynamic. Should be handled by some base class
        public override string PersistenceId { get { return "sensor-id-1"; } }

        // FIXME: must go to base class
        private const int SHAPSHOT_AFTER_EVENTS = 10;
        private int eventsSinceLastSnapshot = 0;

        public SensorState State { get; private set; }

        public SensorActor()
        {
            State = new SensorState();

            Recover<SnapshotOffer>(snapshotOffer =>
                {
                    Console.WriteLine("Recover SnapshotOffer: {0}", snapshotOffer);

                    var state = snapshotOffer.Snapshot as SensorState;
                    if (state != null)
                        State = state;

                    return true;
                });

            Recover<MeasurementProvided>(measurementProvided =>
                {
                    Console.WriteLine("Recover MeasurementProvided: {0}", measurementProvided);

                    State.Result = measurementProvided.Result;

                    return true;
                });

            Command<ProvideMeasurement>(provideMeasurement =>
                {
                    Console.WriteLine("Command ProvideMeasurement: {0}", provideMeasurement);

                    Publish(new MeasurementProvided(provideMeasurement.Result));

                    return true;
                });

            Command<string>(command =>
                {
                    Console.WriteLine("Command {0}", command);

                    if (command == "print")
                    {
                        Console.WriteLine("Current Result: {0}", State.Result);
                    }
                    else if (command == "snap")
                    {
                        SaveSnapshot(State);
                    }

                    return true;
                });
        }

        // FIXME: must go to a base class
        protected void Publish(object @event)
        {
            Persist(@event, OnRecover);

            if (++eventsSinceLastSnapshot > SHAPSHOT_AFTER_EVENTS)
            {
                SaveSnapshot(State);
                eventsSinceLastSnapshot = 0;
            }

            Context.System.EventStream.Publish(@event);
        }
    }
}
