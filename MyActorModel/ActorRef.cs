using System;

namespace MyActorModel
{
    public class ActorRef : IActorRef
    {
        private Actor Actor { get; set; }
        public ActorPath Path { get { return Actor.Path; } private set; }

        public ActorRef(Actor actor)
        {
            Actor = actor;
        }

        public void Tell(object message)
        {
            throw new NotImplementedException();
        }

        public void Ask(object message)
        {
            throw new NotImplementedException();
        }
    }
}
