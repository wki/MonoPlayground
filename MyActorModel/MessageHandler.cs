using System;

namespace MyActorModel
{
    public class MessageHandler
    {
        public Type Type { get; private set; }
        public Action<object> Handler { get; private set; }

        public MessageHandler(Type type, Action<object> handler)
        {
            Type = type;
            Handler = handler;
        }
    }
}
