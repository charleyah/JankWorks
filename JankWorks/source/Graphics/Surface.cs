using System;

using JankWorks.Core;

namespace JankWorks.Graphics
{
    public abstract class Surface : Disposable
    {
        public abstract Rectangle Viewport { get; set; }

        public abstract RGBA ClearColour { get; set; }

        public void Clear() => this.Clear(ClearBitMask.Colour | ClearBitMask.Depth | ClearBitMask.Stencil);
        public abstract void Clear(ClearBitMask bits);
        public abstract void Display();

        public abstract void DrawPrimitives(Shader shader, DrawPrimitiveType primitive, int offset, int count);
        public abstract void DrawPrimitivesInstanced(Shader shader, DrawPrimitiveType primitive, int offset, int count, int instanceCount);
        public abstract void DrawIndexedPrimitives(Shader shader, DrawPrimitiveType primitive, int count);
        public abstract void DrawIndexedPrimitivesInstanced(Shader shader, DrawPrimitiveType primitive, int count, int instanceCount);
    }

    [Flags]
    public enum ClearBitMask
    {
        Colour,
        Depth,
        Stencil
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





