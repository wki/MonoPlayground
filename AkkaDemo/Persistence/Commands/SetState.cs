using System;

namespace AkkaDemo.Persistence
{
    public class SetState
    {
        public object State { get; private set; }

        public SetState(object state)
        {
            State = state;
        }
    }
}
