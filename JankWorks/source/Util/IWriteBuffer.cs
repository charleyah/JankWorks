using System;

namespace JankWorks.Util
{
    public interface IWriteBuffer<T> : IBuffer<T> where T : unmanaged
    {
        int WritePosition { get; set; }

        void Write(T value);

        void Write(ReadOnlySpan<T> values);
    }
}
