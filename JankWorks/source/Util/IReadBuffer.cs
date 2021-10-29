using System;

namespace JankWorks.Util
{
    public interface IReadBuffer<T> : IBuffer<T> where T : unmanaged
    {
        int ReadPosition { get; set; }

        T? Read();

        int Read(Span<T> destination);
    }
}
