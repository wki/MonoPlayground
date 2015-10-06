using System;
using System.Collections.Generic;
using System.Linq;

namespace MyActorModel
{
    public class ActorPath
    {
        // private IActorSystem System;
        // public string Root { get; private set; }
        private List<string> PathParts;

        public ActorPath(IEnumerable<string> path)
        {
            // Root = "FIXME@localhost";
            PathParts = new List<string>(path);
        }

        public string Address
        {
            get 
            { 
                return String.Join("/", PathParts);
            }
        }

        public IEnumerable<string> Elements()
        {
            return PathParts;
        }

        public string Name
        {
            get
            {
                return PathParts.Last();
            }
        }

        public ActorPath Parent
        {
            get
            {
                return PathParts.Any() 
                    ? new ActorPath(PathParts.Take(PathParts.Count - 1))
                    : null;
            }
        }

        public ActorPath Child(string name)
        {
            return new ActorPath(PathParts.Concat(new [] { name }));
        }

        public override string ToString()
        {
            return Address;
        }
    }
}
