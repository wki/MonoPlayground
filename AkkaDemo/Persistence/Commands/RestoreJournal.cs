using System;

namespace AkkaDemo.Persistence
{
    public class RestoreJournal
    {
        public int StartSequenceNr { get; private set; }

        public RestoreJournal(int startSequenceNr)
        {
            StartSequenceNr = startSequenceNr;
        }
    }
}
