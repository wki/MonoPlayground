using Akka.Actor;
using AkkaDemo.TimeServer.Actors;
using System;

namespace AkkaDemo.TimeServer.Server
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting Time Server");

            var system = Akka.Actor.ActorSystem.Create("TimeServer");
            system.ActorOf<TimeServerActor>("time");

            Console.ReadLine();
            Console.WriteLine("Stopping Time Server");

            system.AwaitTermination();
        }
    }
}
