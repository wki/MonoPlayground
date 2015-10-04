using System;

namespace MyActorModel
{
    public class ActorContext : IActorRefFactory
    {
        public ActorContext()
        {
        }

        public IActorRef ActorOf(Props props)
        {
            throw new NotImplementedException();
        }

        public IActorRef ActorOf(Props props, string name)
        {
            throw new NotImplementedException();
        }

        public void Stop(IActorRef actor)
        {
            throw new NotImplementedException();
        }

        public IActorRef ActorFor(string path)
        {
            throw new NotImplementedException();
        }

        public IActorRef ActorFor(string[] path)
        {
            throw new NotImplementedException();
        }

        public IActorRef ActorFor(ActorPath path)
        {
            throw new NotImplementedException();
        }
    }
}
