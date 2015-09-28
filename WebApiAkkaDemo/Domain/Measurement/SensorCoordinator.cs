using System;
using WebApiAkkaDemo.Domain.Core;

namespace WebApiAkkaDemo.Domain.Measurement
{
    /// <summary>
    /// Sensor coordinator.
    /// </summary>
    /// <description>
    /// The sensor coordinator's responsibility is to deal with persistence
    /// and forwarding messages to a maybe just-loaded sensor.
    /// 
    /// All command messages derieve from IAddressed<int>
    /// </description>
    public class SensorCoordinator : AggregateCoordinator<Sensor, int>
    {
        public SensorCoordinator() : base("Sensor")
        {
            // forward messages to (maybe just loaded) sensor
            Receive<ProvideMeasurement>(ForwardCommand);
            Receive<ReadMeasurement>(ForwardCommand);
        }
    }
}
