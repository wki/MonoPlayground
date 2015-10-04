using System;

namespace AkkaDemo.Persistence
{
    public class RestoredSnapshot
    {
        public int LastSequenceNr { get; private set; }

        public RestoredSnapshot(int lastSequenceNr)
        {
            LastSequenceNr = lastSequenceNr;
        }
    }
}
