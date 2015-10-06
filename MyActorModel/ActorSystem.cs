using System;

namespace MyActorModel
{
    public class ActorSystem : Node
    {
        public string Name { get; private set; }

        public ActorSystem(string name) 
            : base(null, new ActorPath(new string [] { }))
        {
            Name = name;

            // Parent currently is null

            ChildRootPath = new ActorPath(new [] { "user" });
        }
    }
}
