using JankWorks.Core;

using JankWorks.Game.Configuration;

namespace JankWorks.Game.Hosting.Messaging.Memory
{
    abstract class MemoryChannel : Disposable, IChannel
    {
        public byte Id { get; private set; }

        public abstract bool Pending { get; }
        
        internal IChannel.Direction Direction { get; private set; }

        internal IChannel.Reliability Reliability { get; private set; }

        public MemoryChannel(byte id, ChannelParameters parameters, Settings settings)
        {
            this.Id = id;
            this.Direction = parameters.Direction;
            this.Reliability = parameters.Reliability;
        }

        public abstract void Synchronise();
    }
}