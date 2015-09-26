using System;

namespace WebApiAkkaDemo.Domain
{
    public interface IDomainService
    {
        void ProvideMeasurement(int sensorId, int measurement);
    }
}
