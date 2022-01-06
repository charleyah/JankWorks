using System;

using JankWorks.Graphics;

using static JankWorks.Drivers.OpenGL.Native.Functions;
using static JankWorks.Drivers.OpenGL.Native.Constants;

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

        public override void Update(ReadOnlySpan<T> data, int offset) => this.buffer.Update(GL_ARRAY_BUFFER, this.Usage, data, offset);

        protected override void Dispose(bool disposing)
        {
            glBindBuffer(GL_ARRAY_BUFFER, 0);
            this.buffer.Delete();
            base.Dispose(disposing);
        }
    }
}