using Akka.Actor;
using System;

namespace AkkaDemo.TimeServer.Client
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Time Server client");

            var system = Akka.Actor.ActorSystem.Create("TimeServer");
        }
    }
}
