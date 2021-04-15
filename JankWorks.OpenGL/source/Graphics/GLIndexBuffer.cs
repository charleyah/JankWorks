using System;

using JankWorks.Graphics;

using static OpenGL.Functions;
using static OpenGL.Constants;


namespace JankWorks.Drivers.OpenGL.Graphics
{
    class GLIndexBuffer : IndexBuffer
    {
        internal uint Id => this.buffer.BufferId;
        public override int ElementCount => this.buffer.ElementCount;
        public override BufferUsage Usage { get; set; }

        private GLBuffer<uint> buffer;

        public GLIndexBuffer()
        {
            this.buffer.Generate();
        }

        public override void CopyTo(Span<uint> data) => this.buffer.CopyTo(GL_ELEMENT_ARRAY_BUFFER, data);

        public override uint[] Read() => this.buffer.Read(GL_ELEMENT_ARRAY_BUFFER);

        public override void Write(ReadOnlySpan<uint> data) => this.buffer.Write(GL_ELEMENT_ARRAY_BUFFER, this.Usage, data);

        protected override void Dispose(bool finalising)
        {
            glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
            this.buffer.Delete();
            base.Dispose(finalising);
        }
    }
}
