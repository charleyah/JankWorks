using System;
using System.Diagnostics;
using System.Threading;

using JankWorks.Util;
using JankWorks.Game.Diagnostics;
using JankWorks.Game.Configuration;

namespace JankWorks.Game.Hosting.Messaging.Memory
{
    sealed class MemoryMessageChannel<Message> : MemoryChannel, IMessageChannel<Message> where Message : unmanaged
    {
        public override bool Pending => this.receiveBuffer.Length > 0;

        public int Queued => this.receiveBuffer.Length;

        private ArrayWriteBuffer<Message> sendBuffer;
        private ArrayReadBuffer<Message> receiveBuffer;

        public MemoryMessageChannel(byte id, Settings settings, IChannel.Direction direction) : base(id, settings, direction) 
        {
            this.sendBuffer = new ArrayWriteBuffer<Message>();
            this.receiveBuffer = new ArrayReadBuffer<Message>(new Message[this.sendBuffer.Capacity]);
        }

        [Conditional("DEBUG")]
        private void VerifyDirection(bool receive)
        {
            Thread expectedThread;

            if(receive)
            {
                expectedThread = this.Direction switch
                {
                    IChannel.Direction.Down => Threads.ClientThread,
                    IChannel.Direction.Up => Threads.HostThread,
                    _ => throw new NotImplementedException()
                };
            }
            else
            {
                expectedThread = this.Direction switch
                {
                    IChannel.Direction.Down => Threads.HostThread,
                    IChannel.Direction.Up => Threads.ClientThread,
                    _ => throw new NotImplementedException()
                };
            }
            
            if (Thread.CurrentThread != expectedThread)
            {
                throw new Exceptions.MessageException($"Cannot {(receive ? "receive from a" : "send to a")} {this.Direction} Channel");
            }
        }

        public Message? Receive()
        {
            this.VerifyDirection(true);
            return this.receiveBuffer.Read();
        }

        public int Receive(Span<Message> messages)
        {
            this.VerifyDirection(true);
            return this.receiveBuffer.Read(messages);
        }

        public void Send(Message message)
        {
            this.VerifyDirection(false);
            this.sendBuffer.Write(message);
        }

        public void Send(ReadOnlySpan<Message> messages)
        {
            this.VerifyDirection(false);
            this.sendBuffer.Write(messages);
        }

        public override void Synchronise()
        {
            if(this.sendBuffer.Length > 0)
            {
                this.sendBuffer.Swap(this.receiveBuffer);                                
            }            
        }
    }
}