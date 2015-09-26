using Akka.Actor;
using System;
using System.Threading.Tasks;

namespace WebApiAkkaDemo
{
    public class AskableService : IAskableService
    {
        private IActorRef askableActor;

        public AskableService(IActorRef askableActor)
        {
            this.askableActor = askableActor;
        }

        public string Ask(string question)
        {
            return ResultOf(() => AskAsync(question));
        }

        public Task<string> AskAsync(string question)
        {
            return askableActor.Ask<string>(new AskAQuestion(question));
        }

        private T ResultOf<T>(Func<Task<T>> f)
        {
            var task = f();
            task.Wait();
            return task.Result;
        }
    }
}
