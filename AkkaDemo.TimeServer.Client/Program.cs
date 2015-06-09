using Akka.Actor;
using AkkaDemo.TimeServer.Actors;
using System;

namespace AkkaDemo.TimeServer.Client
{
    class MainClass
    {
        private static ActorSystem system;

        public static void Main(string[] args)
        {
            Console.WriteLine("Time Server client");

            system = Akka.Actor.ActorSystem.Create("TimeClient");

            ReadTime();

            system.AwaitTermination();
        }

        private static async void ReadTime()
        {
            var time = system.ActorSelection("akka.tcp://TimeServer@localhost:9000/user/time");

            var client = system.ActorOf(
                Props.Create( () => new Tim

            var result = await time.Ask<string>(new TimeServerActor.GetTime());

            Console.WriteLine("Time received: {0}", result);
        }
    }
}
