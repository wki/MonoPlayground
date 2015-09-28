using System;
using WebApiAkkaDemo.Domain.Core;

namespace WebApiAkkaDemo.Domain.Measurement
{
    public class ProvideMeasurement : ICommand, IAddressed<int>
    {
        public ProvideMeasurement(int id, int result)
        {
            Id = id;
            Result = result;
        }

        public int Id { get; set; }
        public int Result { get; set; }
    }

    public class ReadMeasurement : ICommand, IAddressed<int>
    {
        public ReadMeasurement(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}
