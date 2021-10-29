using System;

using JankWorks.Util;
using JankWorks.Game.Configuration;

namespace JankWorks.Game.Hosting.Messaging.Memory
{
    sealed class MemoryMessageChannel<Message> : MemoryChannel, IMessageChannel<Message> where Message : unmanaged
    {
        public override bool Pending => this.receiveBuffer.Length > 0;

        public int Queued => this.receiveBuffer.Length;

        private ArrayWriteBuffer<Message> sendBuffer;
        private ArrayReadBuffer<Message> receiveBuffer;

        public MemoryMessageChannel(byte id, Settings settings, IChannel.Reliability reliability) : base(id, settings, reliability) 
        {
            this.sendBuffer = new ArrayWriteBuffer<Message>();
            this.receiveBuffer = this.sendBuffer.GetReadBuffer();
        }

        public Message? Receive() => this.receiveBuffer.Read();

        public int Receive(Span<Message> messages) => this.receiveBuffer.Read(messages);

        public void Send(Message message) => this.sendBuffer.Write(message);

        public void Send(ReadOnlySpan<Message> messages) => this.sendBuffer.Write(messages);

        public override void Synchronise()
        {
            if(this.sendBuffer.Length > 0)
            {
                this.sendBuffer.Swap(this.receiveBuffer);                                
            }            
        }
    }
}