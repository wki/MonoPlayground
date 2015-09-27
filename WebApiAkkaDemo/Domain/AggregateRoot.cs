using System;
using Akka.Persistence;

/*
TODOs:

from:
https://gitter.im/petabridge/akka-bootcamp/archives/2015/06/25

Coordinator:
https://github.com/Horusiath/AkkaCQRS/blob/master/src/AkkaCQRS.Core/AggregateCoordinator.cs

In akka.net, currently I've found 2 ways of dealing with loading/harvesing 
persistent actors.

1) you have some aggregate coordinator, which you request for your aggregates. 
If has internal collection of children aggregates - if they are already loaded, 
they are returned immediately. If not it will create them and return. In my 
example there is a limit of allowed children buffer, so that you won't overuse 
the memory. If limit is passed it will harvest X actors to free part the buffer. 
Coordinator is a proxy for messages send to aggregates - it also buffers 
messages waiting for aggregates to being recreated.

2) Same think - aggregate coordinator returns aggregates on demand. But here 
each aggregate uses Context.SetReceiveTimeout method to identify if it's not 
used for some period of time. If so, it will receive ReceiveTimeout message. 
Then it should send some kind of Passivate message back to coordinator and stop 
itself. Passivate is necessary so that coordinator knows not to send any 
messages to the dying aggregate. If there are some messages pending, coordinator 
caches them, then recreates an aggregate and finally sends waiting messages back 
to it.
Difference between 1 and 2 is that first defines some pool of actors, that you 
won't exceed, but you won't know if actor is not used for a longer period of 
time and be able to automatically free the aggregate pool. In second you don't 
have a pool, but each actor knows, for how long it has not been used and it's 
able to "recycle" itself if no longer needed. Cons is that you could possibly 
have more actors at the moment, than your memory is allowed to fit. Ofc you can 
create a hybrid of both approaches.

*/
using Akka.Event;

namespace WebApiAkkaDemo.Domain
{
    /// <summary>
    /// Aggregate actor.
    /// </summary>
    /// <description>
    /// base class for aggregates
    /// </description>
    public class AggregateRoot<TState> : ReceivePersistentActor
        where TState: class, new()
    {
        private const int SHAPSHOT_AFTER_EVENTS = 10;
        private int eventsSinceLastSnapshot = 0;

        private ILoggingAdapter log;
        protected ILoggingAdapter Log { get { return log ?? (log = Context.GetLogger()); } }

        public int Id { get; set; }
        public override string PersistenceId { get { return BuildPersistenceId(); } }
        public TState State { get; protected set; }

        public AggregateRoot(int id)
        {
            Id = id;
            State = new TState();

            Recover<SnapshotOffer>(RecoverFromSnapshot);
        }

        protected virtual string BuildPersistenceId()
        {
            return String.Format("{0}-{1}", this.GetType().FullName, Id);
        }

        #region Snapshot handling
        private bool RecoverFromSnapshot(SnapshotOffer snapshotOffer)
        {
            Log.Debug("Recover from Snapshot. Id: {0}, Sequence Nr: {1}", 
                PersistenceId,
                snapshotOffer.Metadata.SequenceNr
            );

            var state = snapshotOffer.Snapshot as TState;
            if (state != null)
                State = state;

            return true;
        }

        private void SaveSnapshot()
        {
            Log.Debug("Save Snapshot. Id: {0}", PersistenceId);

            SaveSnapshot(State);
            eventsSinceLastSnapshot = 0;
        }
        #endregion

        /// <summary>
        /// Publish the specified event.
        /// </summary>
        /// <param name="event">Event.</param>
        /// <description>
        /// typically used by a command handler. Ensures that the given
        /// event is persisted into the journal
        /// </description>
        protected void Publish(object @event)
        {
            Persist(@event, OnRecover);

            if (++eventsSinceLastSnapshot > SHAPSHOT_AFTER_EVENTS)
            {
                SaveSnapshot(State);
                eventsSinceLastSnapshot = 0;
            }

            Context.System.EventStream.Publish(@event);
        }
    }
}
