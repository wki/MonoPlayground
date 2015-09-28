using System;
using WebApiAkkaDemo.Domain.Core;

namespace WebApiAkkaDemo.Domain.Measurement
{
    public class MeasurementProvided : IEvent, IAddressed<int>
    {
        public MeasurementProvided(int id, int result)
        {
            Id = id;
            Result = result;
        }

        public int Id { get; set; }
        public int Result { get; set; }
    }
}
