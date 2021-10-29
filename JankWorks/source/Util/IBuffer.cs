using System;

namespace JankWorks.Util
{
    public interface IBuffer<T> where T : unmanaged
    {
        ref T this[int index] { get; }

        int Capacity { get; }

        void Clear();

        void Clear(int offset, int length);

        Span<T> GetBufferSpan();
    }
}