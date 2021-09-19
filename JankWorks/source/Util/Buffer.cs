using System;

namespace JankWorks.Util
{
    public abstract class Buffer<T> where T : unmanaged
    {
        public abstract ref T this[int index] { get; }

        public abstract int Position { get; set; }

        public abstract int Capacity { get; }

        public abstract void Clear();

        public abstract void Clear(int offset, int length);

        public abstract void Write(T value);

        public abstract void Write(ReadOnlySpan<T> values);

        public abstract Span<T> GetSpan();

        public abstract Span<T> GetBufferSpan();
    }
}