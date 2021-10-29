using System;

namespace JankWorks.Game.Hosting.Messaging
{
    public interface IChannel : IDisposable
    {
        byte Id { get; }

        bool Pending { get; }

        public enum Direction
        {
            Down,
            Up
        }

        public enum Reliability
        {
            Reliable,
            Unreliable
        }
    }

    public interface IMessageChannel<Message> : IChannel where Message : unmanaged
    {
        int Queued { get; }

        void Send(Message message);

        void Send(ReadOnlySpan<Message> messages);

        Message? Receive();

        int Receive(Span<Message> messages);
    }
}