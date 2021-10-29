using System;

namespace JankWorks.Game.Hosting.Messaging
{
    public struct ChannelParameters
    {
        public IChannel.Direction Direction { get; set; }

        public IChannel.Reliability Reliability { get; set; }

        public uint MaxQueueSize { get; set; }
    }
}