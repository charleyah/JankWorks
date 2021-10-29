using System;

namespace JankWorks.Game.Hosting.Messaging.Exceptions
{
    public sealed class ChannelException : Exception
    {
        public ChannelException(string error, Exception inner) : base(error, inner) { }
    }
}