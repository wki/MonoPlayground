using System;
using Akka;
using Akka.Actor;

namespace AkkaDemo
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var actorSystem = new ActorSystem();

            var calculator = actorSystem.ActorOf(Props.Create<CalculatingActor>());

            Console.WriteLine("Press enter to continue");
            Console.ReadLine();

            actorSystem.Shutdown();
            actorSystem.AwaitTermination();
        }
    }
}
