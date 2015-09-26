using System;

namespace WebApiAkkaDemo.Domain
{
    public class SensorState
    {
        public int Result { get; internal set; }
    }

    public class Sensor : AggregateRoot<SensorState>
    {
        #region Commands
        public class ProvideMeasurement
        {
            public int Result { get; private set; }

            public ProvideMeasurement(int result)
            {
                Result = result;
            }
        }
        #endregion

        #region Events
        public class MeasurementProvided
        {
            public int Result { get; private set; }

            public MeasurementProvided(int result)
            {
                Result = result;
            }
        }

        #endregion

        public Sensor(int id) : base(id)
        {
            Command<ProvideMeasurement>(HandleMeasurement);
            Recover<MeasurementProvided>(ApplyMeasurement);
        }

        #region Command handlers
        private bool HandleMeasurement(ProvideMeasurement provideMeasurement)
        {
            Publish(new MeasurementProvided(provideMeasurement.Result));
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
