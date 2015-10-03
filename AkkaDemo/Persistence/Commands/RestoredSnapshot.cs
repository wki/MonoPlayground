using System;

namespace AkkaDemo
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
