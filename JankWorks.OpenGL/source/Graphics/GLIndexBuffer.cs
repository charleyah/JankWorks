using System;

using JankWorks.Graphics;

using static JankWorks.Drivers.OpenGL.Native.Functions;
using static JankWorks.Drivers.OpenGL.Native.Constants;

namespace JankWorks.Drivers.OpenGL.Graphics
{
    sealed class GLIndexBuffer : IndexBuffer
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

        public override void Update(ReadOnlySpan<uint> data, int offset) => this.buffer.Update(GL_ELEMENT_ARRAY_BUFFER, this.Usage, data, offset);

        protected override void Dispose(bool finalising)
        {
            glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
            this.buffer.Delete();
            base.Dispose(finalising);
        }
    }
}
