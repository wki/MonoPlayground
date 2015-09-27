using Akka.Actor;
using System;
using System.Linq;

namespace AkkaDemo.PersistentSensor
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting Actor System");

            var system = ActorSystem.Create("Measurement");
            var sensor1 = system.ActorOf<SensorActor>("sensor1");
            Console.WriteLine("Sensor 1 = {0}", sensor1.Path);

            sensor1.Tell(new SensorActor.ProvideMeasurement(42));
            sensor1.Tell("print");
            sensor1.Tell("snap");

            var sensor2 = system.ActorSelection("/user/sensor1");
            Console.WriteLine(
                "Sensor 2 = {0} / {1}", 
                sensor2.PathString,
                sensor2.Anchor
            );

            try
            {
                var t = sensor2.ResolveOne(TimeSpan.FromMilliseconds(100));
                t.Wait();
                Console.WriteLine("Got it! resolved selection");
            }
            catch (Exception e)
            {
                Console.WriteLine("During Resolve caught: {0} ({1})", e.Message, e.GetType().FullName);
            }

            try
            {
                var sensor3 = system.ActorOf<SensorActor>("sensor1");
                Console.WriteLine("Sensor 3 = {0} // should never print", sensor3.Path);
            }
            catch (Exception e)
            {
                Console.WriteLine("During ActorOf caught: {0} ({1})", e.Message, e.GetType().FullName);
            }

                

            sensor2.Tell(new SensorActor.ProvideMeasurement(33));
            sensor2.Tell("print");

            Console.ReadLine();

            Console.WriteLine("Stopping Actor System");

            system.Shutdown();
            system.AwaitTermination();
        }
    }
}
