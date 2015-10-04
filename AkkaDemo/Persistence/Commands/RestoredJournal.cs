using System;

namespace AkkaDemo.Persistence
{
    public class RestoredJournal
    {
        public int LastSequenceNr { get; private set; }

        public RestoredJournal(int lastSequenceNr)
        {
            LastSequenceNr = lastSequenceNr;
        }
    }
}
