using System;
using Akka.Actor;

namespace WebApiAkkaDemo.Domain
{
    public class AskableActor : ReceiveActor
    {
        public AskableActor()
        {
            Receive<string>(AnswerQuestion);
        }

        private void AnswerQuestion(string question)
        {
            Sender.Tell(
                String.Format("Received Question '{0}', answer unknown", question)
            );
        }
    }
}
