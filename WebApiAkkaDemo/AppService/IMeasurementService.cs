using System;
using System.Threading.Tasks;

namespace WebApiAkkaDemo.AppService
{
    public interface IMeasurementService
    {
        void ProvideMeasurement(int sensorId, int result);
        Task ProvideMeasurementAsync(int sensorId, int result);

        int ReadMeasurement(int sensorId);
        Task<int> ReadMeasurementAsync(int sensorId);
    }
}
