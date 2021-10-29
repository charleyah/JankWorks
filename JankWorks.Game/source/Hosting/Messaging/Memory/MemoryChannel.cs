using JankWorks.Core;

using JankWorks.Game.Configuration;

namespace JankWorks.Game.Hosting.Messaging.Memory
{
    abstract class MemoryChannel : Disposable, IChannel
    {
        public byte Id { get; private set; }

        public abstract bool Pending { get; }
        
        internal IChannel.Reliability Reliability { get; private set; }

        public MemoryChannel(byte id, Settings settings, IChannel.Reliability reliability)
        {
            this.Id = id;
            this.Reliability = reliability;
        }

        public abstract void Synchronise();
    }
}