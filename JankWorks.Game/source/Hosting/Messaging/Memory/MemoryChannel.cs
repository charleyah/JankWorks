using JankWorks.Core;

using JankWorks.Game.Configuration;

namespace JankWorks.Game.Hosting.Messaging.Memory
{
    abstract class MemoryChannel : Disposable, IChannel
    {
        public byte Id { get; private set; }

        public abstract bool Pending { get; }
        
        internal IChannel.Direction Direction { get; private set; }

        public MemoryChannel(byte id, Settings settings, IChannel.Direction direction)
        {
            this.Id = id;
            this.Direction = direction;
        }

        public abstract void Synchronise();
    }
}