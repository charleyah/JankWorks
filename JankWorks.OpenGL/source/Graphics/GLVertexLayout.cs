using System;
using System.Collections.Generic;
using JankWorks.Graphics;

using static JankWorks.Drivers.OpenGL.Native.Functions;

namespace JankWorks.Drivers.OpenGL.Graphics
{
    sealed class GLVertexLayout : VertexLayout
    {
        internal uint Id => this.vaoId;            

        private uint vaoId;
        
        private Dictionary<int, VertexAttribute> attributes;

        public GLVertexLayout()
        {
            var vao = 0u;
            unsafe
            {
                glGenVertexArrays(1, &vao);
            }
            this.vaoId = vao;

            this.attributes = new Dictionary<int, VertexAttribute>();
        }

        public override void SetAttribute(VertexAttribute attribute)
        {
            if(!this.attributes.TryAdd(attribute.Index, attribute))
            {
                this.attributes[attribute.Index] = attribute;
            }
        }

        public override void SetAttributes(ReadOnlySpan<VertexAttribute> attributes)
        {
            foreach (var attribute in attributes)
            {                
                this.attributes[attribute.Index] = attribute;
            }
        }

        internal void ApplyAttributes()
        {
            foreach(var attrib in this.attributes.Values)
            {
                this.ApplyAttribute(in attrib);
            }
        }

        private void ApplyAttribute(in VertexAttribute attribute)
        {
            var type = attribute.Format.GetGLPointerType();
            var count = 1;

            switch (attribute.Format)
            {
                case VertexAttributeFormat.Vector2f:
                case VertexAttributeFormat.Vector2i: count = 2; break;

                case VertexAttributeFormat.Vector3f:
                case VertexAttributeFormat.Vector3i: count = 3; break;

                case VertexAttributeFormat.Vector4f:
                case VertexAttributeFormat.Vector4i: count = 4; break;
            }

            unsafe
            {
                glVertexAttribPointer((uint)attribute.Index, count, type, false, attribute.Stride, (void*)attribute.Offset);
                glEnableVertexAttribArray((uint)attribute.Index);
            }
        }

        protected override void Dispose(bool finalising)
        {
            var vao = this.vaoId;

            unsafe
            {
                glDeleteVertexArrays(1, &vao);
            }

            base.Dispose(finalising);
        }
    }
}
