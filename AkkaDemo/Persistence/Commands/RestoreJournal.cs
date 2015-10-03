using System;

namespace AkkaDemo
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
