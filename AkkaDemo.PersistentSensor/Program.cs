using Akka.Actor;
using System;

namespace AkkaDemo.PersistentSensor
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting Actor System");

            var system = Akka.Actor.ActorSystem.Create("Measurement");
            var sensor1 = system.ActorOf<SensorActor>("sensor1");

            sensor1.Tell(new SensorActor.ProvideMeasurement(42));
            sensor1.Tell("print");
            sensor1.Tell("snap");

            Console.ReadLine();

            Console.WriteLine("Stopping Actor System");

            system.Shutdown();
            system.AwaitTermination();
        }
    }
}
