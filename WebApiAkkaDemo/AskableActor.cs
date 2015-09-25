using System;
using Akka.Actor;

namespace WebApiAkkaDemo
{
    public class AskableActor : ReceiveActor
    {
        public AskableActor()
        {
            Receive<AskAQuestion>(AnswerQuestion);
        }

        private void AnswerQuestion(AskAQuestion question)
        {
            Sender.Tell(
                String.Format("Received Question '{0}', answer unknown", question.Question)
            );
        }
    }
}
