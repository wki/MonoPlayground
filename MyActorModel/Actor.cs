using System.Reactive;
using System;
using System.Reactive.Subjects;
using System.Collections.Generic;
using System.Linq;

namespace MyActorModel
{
    public class Actor : Node, IDisposable
    {
        public IActorRef Self { get; private set; }
        public IActorRef Sender { get; private set; }
        // TODO: SupervisorStrategy

        private Subject<Envelope> mailbox;
        private List<IActorRef> children;
        private List<MessageHandler> receivers;

        public Actor(IActorRef self)
        {
            Self = self;

            mailbox = new Subject<Envelope>();
            children = new List<IActorRef>();
            receivers = new List<MessageHandler>();

            mailbox.Subscribe(DispatchMessage);
        }

        public virtual void PreStart() 
        {
        }

        public virtual void PostStop() 
        {
        }

        public virtual void PreRestart()
        {
            StopChildren();
            PostStop();
        }

        public virtual void PostRestart()
        {
            PreStart();
        }

        private void Stop()
        {
            mailbox.OnCompleted();
        }

        protected void StopChildren()
        {
            children.ForEach(c => c.Stop());
            children.Clear();
        }

        protected void Unhandled(object message)
        {
            throw new MessageUnhandledException(message.ToString());
        }

        // low level version -- put a message into mailbox
        internal void QueueMessage(object  message, IActorRef sender)
        {
            mailbox.OnNext(new Envelope(message, sender));
        }

        // usage Receive<DoThis>(dothis => Handle(dothis));
        public void Receive<TMessage>(Action<object> handler)
        {
            receivers.Add(new MessageHandler(typeof(TMessage), handler));
        }

        private void DispatchMessage(Envelope envelope)
        {
            Sender = envelope.Sender;

            receivers
                .First(r => r.Type.IsInstanceOfType(envelope.Message))
                .Handler(envelope.Message);
        }

        public void Dispose()
        {
            if (mailbox != null)
                mailbox.Dispose();

            mailbox = null;
        }
    }
}
