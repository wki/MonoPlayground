using System;
using System.Collections.Generic;

namespace MyActorModel
{
    public class Node
    {
        public IActorRef Parent { get; private set; }
        public List<IActorRef> Children { get; private set; }
        public ActorPath ChildRootPath { get; internal set; }
        public ActorPath Path { get; private set; }

        public Node(IActorRef parent, ActorPath path)
        {
            Parent = parent;
            Children = new List<IActorRef>();
            Path = path;
            ChildRootPath = Path;
        }
    }
}
