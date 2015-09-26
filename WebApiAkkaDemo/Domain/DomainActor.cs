using Akka.Actor;
using System;

namespace WebApiAkkaDemo.Domain
{
    /// <summary>
    /// Domain actor.
    /// </summary>
    /// <description>
    /// ist zuständig für das Erzeugen von Aggregate Roots.
    /// wenn es persistierte Aktoren sind werden sie geladen
    /// </description>
    public class DomainActor : ReceiveActor
    {
        #region Command Messages
        public class BuildAggregate
        {
            public BuildAggregate(Type type, int id)
            {
                Type = type;
                Id = id;
            }

            public Type Type { get; set; }
            public int Id { get; set; }
        }
        #endregion

        #region Document Messages
        public class Aggregate
        {
            public Aggregate(IActorRef actor)
            {
                Actor = actor;
            }

            public IActorRef Actor { get; set; }
        }
        #endregion

        public DomainActor()
        {
            Receive<BuildAggregate>(Build);
        }

        private void Build(BuildAggregate buildAggregate)
        {
            var name = String.Format("{0}-{1}", buildAggregate.Type.Name, buildAggregate.Id);
            var actor = Context.ActorOf(Props.Create(buildAggregate.Type, buildAggregate.Id), name);

            Sender.Tell(new Aggregate(actor));
        }
    }
}
