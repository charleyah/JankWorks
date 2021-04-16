using System;

using JankWorks.Core;
using JankWorks.Drivers;

namespace JankWorks.Graphics
{
    public abstract class Surface : Disposable
    {
        public abstract Rectangle Viewport { get; set; }

        public abstract RGBA ClearColour { get; set; }

        public abstract void Clear();
        public abstract void Display();

        public abstract void CopyToTexture(Texture2D texture);

        public abstract void DrawPrimitives(Shader shader, DrawPrimitiveType primitive, int offset, int count);
        public abstract void DrawPrimitivesInstanced(Shader shader, DrawPrimitiveType primitive, int offset, int count, int instanceCount);
        public abstract void DrawIndexedPrimitives(Shader shader, DrawPrimitiveType primitive, int count);
        public abstract void DrawIndexedPrimitivesInstanced(Shader shader, DrawPrimitiveType primitive, int count, int instanceCount);
    }

    public enum DrawPrimitiveType
    {
        Points,
        Lines,
        LineStrip,
        LineLoop,

        Triangles,
        TriangleStrip
    }
}





