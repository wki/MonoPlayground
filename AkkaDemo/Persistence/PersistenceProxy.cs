using Akka.Actor;
using System;

namespace AkkaDemo.Persistence
{
    /// <summary>
    /// Persistence proxy.
    /// </summary>
    /// <description>
    /// A persistence proxy acts as a Aggregate root.
    /// 
    ///  * it knows the Id and the Type of the aggregate root
    ///  * it receives commands meant for the aggregate root and forwards them
    ///  * it loads the aggregate root from snapshot or journal
    ///  * 1) it receives all events the aggregate root emits
    ///  * 2) events are persisted into journal and then broadcasted ?
    ///  * 1+2) alternative: receive broadcasted events and persist + forward
    ///  * after x events: create snapshot
    ///  * after certain time: unload
    /// 
    /// Behaviors:
    ///   Loading   --> buffer commands
    ///   Receiving --> work on commands (2 min. Timeout)
    ///   Unloading --> buffer commands, shutdown on mailbox empty
    /// 
    /// </description>
    public class PersistenceProxy: ReceiveActor, IWithUnboundedStash
    {
        public IStash Stash { get; set; }

        private int id;
        private Type aggregateRootType;

        private int sequenceNr;

        private IActorRef journalWriter;
        private IActorRef journalReader;
        private IActorRef snapshotWriter;
        private IActorRef snapshotReader;
        private IActorRef aggregateRoot;


        public PersistenceProxy(int id, Type aggregateRootType)
        {
            this.id = id;
            this.aggregateRootType = aggregateRootType;

            sequenceNr = 0;

            // erzeugen neue Aktoren für journal*, snapshot*, aggregateRoot, unloadTimer

            Become(Loading);
        }

        #region behaviors
        private void Loading()
        {
            // Commands:
            Receive<RestoreSnapshot>(_ => 
                snapshotReader.Tell(new RestoreSnapshot())
            );

            Receive<RestoreJournal>(restoreJournal =>
                journalReader.Tell(restoreJournal)
            );

            // Events:
            Receive<RestoredSnapshot>(restoredSnapshot =>
                {
                    sequenceNr = restoredSnapshot.LastSequenceNr;
                    Self.Tell(new RestoreJournal(restoredSnapshot.LastSequenceNr));
                }
            );

            Receive<RestoredJournal>(restoredJournal => 
                {
                    sequenceNr = restoredJournal.LastSequenceNr;
                    Become(Receiving);
                }
            );

            Receive<ReceiveTimeout>(_ => Become(Unloading));

            // stash all other messages
            Receive<object>(message => Stash.Stash());

            // start restoring. snapshot first, then journal
            Self.Tell(new RestoreSnapshot());
            SetReceiveTimeout(TimeSpan.FromMinutes(2));
        }

        private void Receiving()
        {
            // Commands:
            Receive<CreateSnapshot>(createSnapshot => snapshotWriter.Tell(createSnapshot));

            Receive<Unload>(_ => Become(Unloading));

            // Events:
            // *** TODO ***

            // Forward everything else to aggregate Root
            Receive<object>(message =>
                {
                    // FIXME: wie können wir Events von Commands unterscheiden?
                    journalWriter.Tell(new AppendEvent(message));
                    aggregateRoot.Tell(message, Sender);
                }
            );

            // replay all stashed messages from Loading phase
            Stash.UnstashAll();

            // TODO: start a timer for unloading
        }

        private void Unloading()
        {
            // was machen wir hier? uns aufgeben -- reicht das?
            Self.Tell(PoisonPill.Instance);
        }
        #endregion

        #region handlers

        #endregion

    }
}
