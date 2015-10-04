using System;
using System.Collections.Generic;
using System.Linq;

namespace MyActorModel
{
    public class ActorPath
    {
        private IActorSystem system;
        public string Root { get; private set; }
        private List<string> pathParts;

        public ActorPath(IActorSystem system, IEnumerable<string> path)
        {
            this.system = system;
            Root = "FIXME@localhost";
            pathParts = new List<string>(path);
        }

        public string Address
        {
            get
            { 
                return String.Format("{0}/{1}",
                    Root,
                    String.Join("/", pathParts)
                );
            }
        }

        public IEnumerable<string> Elements()
        {
            return pathParts;
        }

        public string Name
        {
            get
            {
                return pathParts.Last();
            }
        }

        public ActorPath Parent
        {
            get
            {
                return pathParts.Any() 
                    ? new ActorPath(system, pathParts.Take(pathParts.Count - 1))
                    : null;
            }
        }

        public ActorPath Child(string name)
        {
            return new ActorPath(system, pathParts.Concat(new [] { name }));
        }

        public override string ToString()
        {
            return Address;
        }
    }
}
