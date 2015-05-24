using System;
using Akka.Actor;
using System.Threading.Tasks;

namespace AkkaDemo
{
    public class CalculatingActor : ReceiveActor
    {
        public int Value { get; private set; }

        #region Message Types
        public class AskValue
        {
        }

        public class AddMessage
        {
            public int Value { get; private set; }

            public AddMessage(int value)
            {
                this.Value = value;
            }
        }
        #endregion

        public CalculatingActor()
        {
            Receive<AskValue>(ReturnValue);
            Receive<AddMessage>(AddToValue);
        }

        protected override bool AroundReceive(Receive receive, object message)
        {
            Console.WriteLine("Around Receive: {0}", message);
            return base.AroundReceive(receive, message);
        }

        public Task<int> ReturnValue(AskValue ask)
        {
            Console.WriteLine("Value asked.");
            var task = Task<int>.Factory.StartNew(() => Value );
            task.Start();
            return task;
        }

        private void AddToValue(AddMessage add)
        {
            Value += add.Value;
        }
    }
}
