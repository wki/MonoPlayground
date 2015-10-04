using System;

namespace AkkaDemo.Persistence
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
