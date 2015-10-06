using System;

namespace MyActorModel
{
	public interface IActorRefFactory
	{
        IActorRef ActorOf(Props props);
        IActorRef ActorOf(Props props, string name);
        // Dispatcher bei akka (scala) definiert
        // void Stop(IActorRef actor);
        IActorRef ActorFor(string path);
        IActorRef ActorFor(string[] path);
        IActorRef ActorFor(ActorPath path);
	}
}
