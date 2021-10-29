using System;

namespace JankWorks.Game.Hosting.Messaging.Memory
{
    sealed class MemoryDispatcher : Dispatcher
    {
        private MemoryChannel[] channels;

        public MemoryDispatcher(Application application) : base(application)
        {
            this.channels = new MemoryChannel[byte.MaxValue];
        }

        public override void ClearChannels()
        {            
            Array.ForEach(this.channels, (channel) => channel?.Dispose());
            Array.Clear(this.channels, 0, this.channels.Length);
        }

        public override IMessageChannel<Message> GetMessageChannel<Message>(byte id, IChannel.Direction direction, IChannel.Reliability reliability)
        {
            ref var channel = ref this.channels[id];

            if(channel == null || channel.Disposed)
            {
                channel = new MemoryMessageChannel<Message>(id, this.Application.Settings, reliability);
            }

            return (IMessageChannel<Message>)channel;
        }

        public override void Synchronise()
        {
            for(int id = 0; id < this.channels.Length; id++)
            {
                ref var channel = ref this.channels[id];

                if (channel == null)
                {
                    continue;
                }
                else if (channel.Disposed)
                {
                    channel = null;
                }
                else
                {
                    channel.Synchronise();
                }
            }
        }
    }
}