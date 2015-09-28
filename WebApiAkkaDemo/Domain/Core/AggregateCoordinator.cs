using System;
using Akka.Actor;
using Akka.Event;
using System.Collections.Generic;
using System.Linq;

namespace WebApiAkkaDemo.Domain.Core
{
    // borrowed from https://github.com/Horusiath/AkkaCQRS/blob/master/src/AkkaCQRS.Core/AggregateCoordinator.cs

    /// <summary>
    /// Base class for an aggregate coordinator.
    /// </summary>
    /// <description>
    /// the aggregate coordinator has these responsibilities:
    /// 
    ///  * load aggregates not in memory
    ///  * free up unused aggregates
    ///  * forward messages to aggregates
    /// 
    /// typical usage
    /// <code>
    /// class People : AggregateCoordinator<Person, int>
    /// {
    ///     public People() : this("Person")
    ///     {
    ///         Receive<SomeMessage>(SomeMessageHandler);
    ///     }
    /// 
    ///     // maybe override GetProps(int) or GetPersistenceId(int)
    ///     private void SomeMessageHandler(SomeMessage s)
    ///     {
    ///         ForwardCommand(s.Id, s.Command);
    ///     }
    /// }
    /// </code>
    /// 
    /// </description>
    public class AggregateCoordinator <TAggregate, TId> : ReceiveActor
        where TAggregate: ActorBase
    {
        #region messages
        protected sealed class PendingCommand
        {
            public readonly IActorRef Sender;
            public readonly TId AggregateId;
            public readonly string PersistenceId; // theoretically: calculatable
            public object Command;

            public PendingCommand(IActorRef sender, TId aggregateId, string persistenceId, object command)
            {
                Sender = sender;
                AggregateId = aggregateId;
                PersistenceId = persistenceId;
                Command = command;
            }
        }
        #endregion

        // FIXME: move into configurable area
        private const int MAX_CHILDREN_COUNT = 64;
        private const int CHILDREN_TO_KILL_COUNT = 20;

        // prefix for actor name. Name = "{prefix} - {id}"
        protected string ChildPrefix { get; set; }

        // all children terminated on demand by ourselves.
        private readonly ISet<IActorRef> terminatingChildren = new HashSet<IActorRef>();

        // commands we must send to children as soon as they are alive
        private ICollection<PendingCommand> pendingCommands = new List<PendingCommand>(0);

        private ILoggingAdapter log;
        protected ILoggingAdapter Log { get { return log ?? (log = Context.GetLogger()); } }

        public AggregateCoordinator(string childPrefix)
        {
            if (childPrefix == null)
                throw new ArgumentNullException("childPrefix");
            
            ChildPrefix = childPrefix;

            Receive<Terminated>(HandleChildTermination);
        }

        /// <summary>
        /// determine the PersistenceId for the Aggregate to use for persisting
        /// </summary>
        /// <returns>PersistenceId</returns>
        /// <param name="aggregateId">Id.</param>
        protected string GetPersistenceId(TId aggregateId)
        {
            return String.Format("{0}-{1}", ChildPrefix, aggregateId);
        }

        /// <summary>
        /// Properties for creating an aggregate
        /// </summary>
        /// <returns>The properties.</returns>
        /// <param name="id">Id.</param>
        protected Props GetProps(TId id)
        {
            return Props.Create<TAggregate>(id);
        }

        // TODO: original had 2 Interfaces: IAddressed, ICommand

        /// <summary>
        /// Forwards a command to a (maybe loaded) aggregate
        /// </summary>
        /// <param name="command">Command.</param>
        protected void ForwardCommand(IAddressed<TId> command)
        {
            ForwardCommand(command.Id, command);
        }

        /// <summary>
        /// Forwards a command to a (maybe loaded) aggregate
        /// </summary>
        /// <param name="aggregateId">Aggregate identifier.</param>
        /// <param name="command">Command.</param>
        /// <description>
        /// typical usage:
        /// 
        /// class SomeCommand { public int Id {}; public object Command {} };
        /// 
        /// Receive<SomeCommand>(c => { ForwardCommand(c.Id, c.Command) });
        ///
        /// 
        /// </description>
        protected void ForwardCommand(TId aggregateId, object command)
        {
            var child = Find(aggregateId);
            if (child == ActorRefs.Nobody)
            {
                child = Create(aggregateId);
            }
            else
            {
                if (terminatingChildren.Contains(child))
                {
                    pendingCommands.Add(new PendingCommand(Sender, aggregateId, GetPersistenceId(aggregateId), command));
                    return;
                }
            }

            child.Forward(command);
        }

        private void HandleChildTermination(Terminated terminated)
        {
            terminatingChildren.ExceptWith(new [] { terminated.ActorRef });

            var commands = pendingCommands.Where(c => c.PersistenceId == terminated.ActorRef.Path.Name);
            foreach (var pendingCommand in commands)
            {
                var child = FindOrCreate(pendingCommand.AggregateId);
                child.Tell(pendingCommand.Command, pendingCommand.Sender);
            }
        }

        private IActorRef FindOrCreate(TId aggregateId)
        {
            var child = Find(aggregateId);
            if (child == ActorRefs.Nobody)
            {
                child = Create(aggregateId);
                Log.Debug("Created aggregate {0}", GetPersistenceId(aggregateId));
            }
            else
            {
                Log.Debug("Found aggregate {0}", GetPersistenceId(aggregateId));
            }
            
            return child;
        }

        // returns found child or ActorRefs.Nobody
        private IActorRef Find(TId aggregateId)
        {
            return Context.Child(GetPersistenceId(aggregateId));
        }

        private IActorRef Create(TId aggregateId)
        {
            CleanupChildren();

            // alternativ: Context.ActorOf<TAggregate>(GetPersistenceId(aggregateId));
            var aggregate = Context.ActorOf(GetProps(aggregateId), GetPersistenceId(aggregateId));
            Context.Watch(aggregate);
            return aggregate;
        }

        // FIXME: find a better strategy eg. by longest not-used time
        private void CleanupChildren()
        {
            var runningChildren = Context.GetChildren().Except(terminatingChildren).ToList();
            if (runningChildren.Count() >= MAX_CHILDREN_COUNT)
            {
                Log.Debug("Max children count exceeded, killing children");

                foreach (var child in runningChildren.Take(CHILDREN_TO_KILL_COUNT))
                {
                    child.Tell(Kill.Instance);
                    terminatingChildren.Add(child);
                }
            }
        }
    }
}
