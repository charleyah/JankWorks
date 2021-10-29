using System;

namespace JankWorks.Game.Hosting.Messaging.Exceptions
{
    public sealed class MessageException : Exception
    {
        public MessageException(string error) : base(error) { }
    }
}
