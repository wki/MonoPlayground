using Akka.Actor;
using AkkaDemo.TimeServer.Actors;
using System;

namespace AkkaDemo.TimeServer.Client
{
    // CLIENT
    class MainClass
    {
        private static ActorSystem system;

        public static void Main(string[] args)
        {
            Console.WriteLine("Time Server client");

            system = Akka.Actor.ActorSystem.Create("TimeClient");

            Console.WriteLine("Settings: {0}", system.Settings);
            ReadTime();

            Console.ReadLine();
            Console.WriteLine("Time Server client Shutdown");
            system.Shutdown();
            system.AwaitTermination();
        }

        private static void XReadTime()
        {
            var time = system
                .ActorSelection("akka.tcp://TimeServer@localhost:9000/user/time");
            //send a message to the remote actor
            time.Tell(new TimeServerActor.GetTime());
        }

        private static async void ReadTime()
        {
            var time = system.ActorSelection("akka.tcp://TimeServer@localhost:9000/user/time");

//            var client = system.ActorOf(
//                Props.Create( () => new TimeServerActor() )
//            );

            var result = await time.Ask<string>(new TimeServerActor.GetTime());

            Console.WriteLine("Time received: {0}", result);
        }
    }
}
