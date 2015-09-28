using Akka.Actor;
using System;
using WebApiAkkaDemo.Domain.Core;

namespace WebApiAkkaDemo.Domain.Measurement
{
    public class SensorState
    {
        public int Result { get; internal set; }
    }

    public class Sensor : AggregateRoot<int, SensorState>
    {
        public Sensor(int id) : base(id)
        {
            Command<ProvideMeasurement>(HandleProvideMeasurement);
            Command<ReadMeasurement>(HandleReadMeasurement);
            Recover<MeasurementProvided>(ApplyMeasurement);
        }

        #region Command handlers
        private bool HandleProvideMeasurement(ProvideMeasurement provideMeasurement)
        {
            Publish(new MeasurementProvided(Id, provideMeasurement.Result));
            return true;
        }

        private bool HandleReadMeasurement(ReadMeasurement readMeasurement)
        {
            Sender.Tell(State.Result);
            return true;
        }
        #endregion

        #region Event handlers
        private bool ApplyMeasurement(MeasurementProvided measurementProvided)
        {
            State.Result = measurementProvided.Result;
            return true;
        }
        #endregion
    }
}
