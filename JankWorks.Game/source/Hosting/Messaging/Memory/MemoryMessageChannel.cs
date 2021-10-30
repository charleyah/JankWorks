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

        private ArrayReadWriteBuffer<Message> sendBuffer;
        private ArrayReadWriteBuffer<Message> receiveBuffer;

        private readonly int maxQueue;

        public MemoryMessageChannel(byte id, ChannelParameters parameters, Settings settings) : base(id, parameters, settings) 
        {
            this.maxQueue = parameters.MaxQueueSize > 0 ? parameters.MaxQueueSize : 1024;

            this.sendBuffer = new ArrayReadWriteBuffer<Message>();
            this.receiveBuffer = new ArrayReadWriteBuffer<Message>();
        }

        private void VerifyDirection(bool receive)
        {
            if(Debugger.IsAttached)
            {
                Thread expectedThread;

                if (receive)
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
        }

        private void CheckMaxQueue()
        {
            if (this.Reliability == IChannel.Reliability.Unreliable && this.sendBuffer.Length >= this.maxQueue)
            {
                this.sendBuffer.WritePosition = 0;
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
            this.CheckMaxQueue();
            this.sendBuffer.Write(message);
        }

        public void Send(ReadOnlySpan<Message> messages)
        {
            this.VerifyDirection(false);
            this.CheckMaxQueue();
            this.sendBuffer.Write(messages);
        }

        public override void Synchronise()
        {
            if(this.sendBuffer.Length > 0)
            {
                if(this.Reliability == IChannel.Reliability.Unreliable)
                {
                    this.sendBuffer.Swap(this.receiveBuffer);                    
                }
                else
                {
                    this.receiveBuffer.CompactWithoutResize();
                    this.receiveBuffer.Write(this.sendBuffer.GetSpan());                                        
                }

                this.sendBuffer.WritePosition = 0;
            }            
        }
    }
}