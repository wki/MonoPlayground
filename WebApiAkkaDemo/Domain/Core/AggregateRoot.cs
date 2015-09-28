using System;
using Akka.Persistence;

using Akka.Event;

namespace WebApiAkkaDemo.Domain.Core
{
    /// <summary>
    /// Aggregate actor.
    /// </summary>
    /// <description>
    /// base class for aggregates
    /// </description>
    public class AggregateRoot<TId, TState> : ReceivePersistentActor
        where TState: class, new()
    {
        private const int SHAPSHOT_AFTER_EVENTS = 10;
        private int eventsSinceLastSnapshot = 0;

        private ILoggingAdapter log;
        protected ILoggingAdapter Log { get { return log ?? (log = Context.GetLogger()); } }

        public TId Id { get; set; }
        public override string PersistenceId { get { return BuildPersistenceId(); } }
        public TState State { get; protected set; }

        public AggregateRoot(TId id)
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
