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
    /// 
    /// zusätzlich "application Facade"
    /// </description>
    public class DomainActor : ReceiveActor
    {
        #region Command Messages
        public class ProvideMeasurement
        {
            public int Result { get; set; }
            public ProvideMeasurement(int result)
            {
                Result = result;
            }
        }

        public class BuildAggregate
        {
            public Type Type { get; set; }
            public int Id { get; set; }

            public BuildAggregate(Type type, int id)
            {
                Type = type;
                Id = id;
            }
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
            Receive<ProvideMeasurement>(Provide);
            Receive<BuildAggregate>(Build);
        }


        private void Provide(ProvideMeasurement provideMeasurement)
        {
            // TODO: ist das eine Aufgabe für einen ganz eigenen Aktor?
            // aggregate holen
            // messwert weitergeben
        }

        private void Build(BuildAggregate buildAggregate)
        {
            var name = String.Format("{0}-{1}", buildAggregate.Type.Name, buildAggregate.Id);
            var actor = Context.ActorOf(Props.Create(buildAggregate.Type, buildAggregate.Id), name);

            Sender.Tell(new Aggregate(actor));
        }
    }
}
