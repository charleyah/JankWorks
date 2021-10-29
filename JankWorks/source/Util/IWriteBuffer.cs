using System;

namespace JankWorks.Util
{
    public interface IWriteBuffer<T> : IBuffer<T> where T : unmanaged
    {
        int Length { get; }

        int WritePosition { get; set; }

        void Write(T value);

        void Write(ReadOnlySpan<T> values);

        Span<T> GetWritten();
    }
}
