using System;

namespace MyActorModel
{
    public class MessageUnhandledException : Exception
    {
        public MessageUnhandledException(string message) : base(message)
        {
        }
    }
}
