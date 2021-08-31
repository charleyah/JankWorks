using System;

using JankWorks.Core;

namespace JankWorks.Graphics
{
    public abstract class VertexBuffer<T> : Disposable where T : unmanaged
    { 
        public abstract int ElementCount { get; }
        public abstract BufferUsage Usage { get; set; }
        public abstract void Write(ReadOnlySpan<T> data);
        public abstract void Update(ReadOnlySpan<T> data, int offset);
        public abstract void CopyTo(Span<T> data);
        public abstract T[] Read();
    }

    public enum BufferUsage
    {
        Static,
        Stream,
        Dynamic
    }
}
