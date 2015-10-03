using System;

namespace AkkaDemo
{
    public class AppendEvent
    {
        public object Event { get; private set; }

        public AppendEvent(object @event)
        {
            Event = @event;
        }
    }
}
