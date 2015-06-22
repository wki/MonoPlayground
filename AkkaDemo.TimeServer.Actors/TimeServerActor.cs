using Akka.Actor;
using System;

namespace AkkaDemo.TimeServer.Actors
{
    public class TimeServerActor : ReceiveActor
    {
        #region messages
        public class GetTime
        {
        }
        #endregion

        public TimeServerActor()
        {
            Receive<GetTime>(HandleGetTime);
        }

        private void HandleGetTime(GetTime getTime)
        {
            Console.WriteLine("Received GetTime Request");
            Sender.Tell(DateTime.Now.ToShortTimeString());
        }
    }
}
