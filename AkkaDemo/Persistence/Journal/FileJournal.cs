using Akka.Actor;
using System;

namespace AkkaDemo.Persistence
{
    public class FileJournal : ReceiveActor
    {
        private Type aggregateRootType;
        private int id;
        private readonly string storageDir;

        public FileJournal(Type aggregateRootType, int id, string storageDir)
        {
            this.aggregateRootType = aggregateRootType;
            this.id = id;
            this.storageDir = storageDir;
        }
    }
}
