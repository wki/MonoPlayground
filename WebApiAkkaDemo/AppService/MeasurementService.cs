using System;
using Akka.Actor;
using WebApiAkkaDemo.Domain.Measurement;
using System.Threading.Tasks;

namespace WebApiAkkaDemo.AppService
{
    /// <summary>
    /// Measurement service.
    /// </summary>
    /// <description>
    /// The measurement service is a thin layer allowing to talk to the
    /// sensor coordinator without having any knowledge of the events to
    /// send to it.
    /// </description>
    public class MeasurementService : IMeasurementService
    {
        private ICanTell SensorCoordinator { get; set; }

        public MeasurementService(ICanTell sensorCoordinator)
        {
            SensorCoordinator = sensorCoordinator;
        }

        public void ProvideMeasurement(int sensorId, int result)
        {
            var task = ProvideMeasurementAsync(sensorId, result);
            task.Wait();
        }

        public Task ProvideMeasurementAsync(int sensorId, int result)
        {
            return SensorCoordinator.Ask(new ProvideMeasurement(sensorId, result));
        }

        public int ReadMeasurement(int sensorId)
        {
            var task = ReadMeasurementAsync(sensorId);
            task.Wait();
            return task.Result;
        }

        public Task<int> ReadMeasurementAsync(int sensorId)
        {
            return SensorCoordinator.Ask<int>(new ReadMeasurement(sensorId));
        }
    }
}
