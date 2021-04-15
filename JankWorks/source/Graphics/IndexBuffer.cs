using System;
using JankWorks.Core;

namespace JankWorks.Graphics
{
    public abstract class IndexBuffer : Disposable
    {
        public abstract int ElementCount { get; }
        public abstract BufferUsage Usage { get; set; }
        public abstract void Write(ReadOnlySpan<uint> data);
        public abstract void CopyTo(Span<uint> data);
        public abstract uint[] Read();
    }
}
