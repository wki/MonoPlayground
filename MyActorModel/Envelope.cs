using System;

namespace MyActorModel
{
    public class Envelope
    {
        public IMessage Message { get; private set; }
        public IActorRef Sender { get; private set; }

        public Envelope(IMessage message, IActorRef sender)
        {
            Message = message;
            Sender = sender;
        }
    }
}
