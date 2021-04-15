using System;

using JankWorks.Graphics;

using static OpenGL.Functions;
using static OpenGL.Constants;

namespace JankWorks.Drivers.OpenGL.Graphics
{
    sealed class GLVertexBuffer<T> : VertexBuffer<T> where T : unmanaged
    {
        internal uint Id => this.buffer.BufferId;
        public override int ElementCount => this.buffer.ElementCount;
        public override BufferUsage Usage { get; set; }

        private GLBuffer<T> buffer;

        public GLVertexBuffer()
        {
            this.buffer.Generate();
        }

        public override void CopyTo(Span<T> data) => this.buffer.CopyTo(GL_ARRAY_BUFFER, data);

        public override T[] Read() => this.buffer.Read(GL_ARRAY_BUFFER);

        public override void Write(ReadOnlySpan<T> data) => this.buffer.Write(GL_ARRAY_BUFFER, this.Usage, data);

        protected override void Dispose(bool finalising)
        {
            glBindBuffer(GL_ARRAY_BUFFER, 0);
            this.buffer.Delete();
            base.Dispose(finalising);
        }
    }
}
