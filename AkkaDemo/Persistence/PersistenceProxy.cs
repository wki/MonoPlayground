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

        // configurable options TODO: read from config, move into *writer/reader
        private readonly string storageDir =
            System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "storage"
            );

        // infos about the aggregate root
        private Type aggregateRootType;
        private int id;

        // our current state
        private int sequenceNr;

        private IActorRef journal;
        private IActorRef snapshot;
        private IActorRef aggregateRoot;


        public PersistenceProxy(Type aggregateRootType, int id)
        {
            this.aggregateRootType = aggregateRootType;
            this.id = id;

            sequenceNr = 0;

            // erzeugen neue Aktoren für journal*, snapshot*, aggregateRoot, unloadTimer
            journal = Context.ActorOf(
                Props.Create<FileJournal>(aggregateRootType, id, storageDir)
            );

            Become(Loading);
        }

        #region behaviors
        private void Loading()
        {
            /* TODOs
             *  - Fehler beim Journal/Snapshot Schreiben
             *  - Absturz des (Journal/Snapshot)(Writer/Reader)
             *  - Fehler beim Journal/Snapshot lesen
             */

            // Commands:
            Receive<RestoreSnapshot>(_ => 
                snapshot.Tell(new RestoreSnapshot())
            );

            Receive<RestoreJournal>(restoreJournal =>
                journal.Tell(restoreJournal)
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
            Receive<CreateSnapshot>(createSnapshot => snapshot.Tell(createSnapshot));

            Receive<Unload>(_ => Become(Unloading));

            // Events:
            // *** TODO ***

            // Forward everything else to aggregate Root
            Receive<object>(message =>
                {
                    // FIXME: wie können wir Events von Commands unterscheiden?
                    journal.Tell(new AppendEvent(message));
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
