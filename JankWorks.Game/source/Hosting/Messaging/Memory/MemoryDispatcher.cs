using System;

using JankWorks.Game.Hosting.Messaging.Exceptions;

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

        public override IMessageChannel<Message> GetMessageChannel<Message>(byte id, ChannelParameters parameters)
        {
            ref var channel = ref this.channels[id];

            if(channel == null || channel.Disposed)
            {
                channel = new MemoryMessageChannel<Message>(id, parameters, this.Application.Settings);
            }

            try
            {
                return (IMessageChannel<Message>)channel;
            }
            catch(InvalidCastException ice)
            {
                throw new ChannelException("Channel mismatch", ice);
            }            
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