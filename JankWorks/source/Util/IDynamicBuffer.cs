using System;

namespace JankWorks.Util
{
    public interface IDynamicBuffer<T> : IBuffer<T> where T : unmanaged
    {
        int Length { get; }

        void Reserve(int count);

        void Compact();

        Span<T> GetSpan();
    }
}