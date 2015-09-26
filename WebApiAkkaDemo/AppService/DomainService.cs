using Akka.Actor;
using System;

namespace WebApiAkkaDemo.Domain
{
    public class DomainService : IDomainService
    {
        private IActorRef domainActor;

        public DomainService(IActorRef domainActor)
        {
            this.domainActor = domainActor;
        }

        public async void ProvideMeasurement(int sensorId, int measurement)
        {
            
        }
    }
}
