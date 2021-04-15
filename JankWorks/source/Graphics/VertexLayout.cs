using System;
using JankWorks.Core;

namespace JankWorks.Graphics
{
    public abstract class VertexLayout : Disposable
    {
        public abstract void SetAttribute(VertexAttribute attribute);
        public abstract void SetAttributes(ReadOnlySpan<VertexAttribute> attributes);
    }
}
