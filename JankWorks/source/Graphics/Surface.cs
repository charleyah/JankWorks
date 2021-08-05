using System;

using JankWorks.Core;

namespace JankWorks.Graphics
{
    public abstract class Surface : Disposable
    {
        public abstract Rectangle Viewport { get; set; }

        public abstract RGBA ClearColour { get; set; }

        public DrawState DefaultDrawState 
        {
            get => this.defaultDrawState;
            set
            {                
                this.ApplyDrawState(in value);
                this.defaultDrawState = value;
            }
        }

        private DrawState defaultDrawState;

        protected Surface(DrawState defaultDrawState)
        {
            this.DefaultDrawState = defaultDrawState;
        }

        public virtual void Clear()
        {
            this.ApplyDrawState(this.DefaultDrawState);
            this.Clear(ClearBitMask.Colour | ClearBitMask.Depth | ClearBitMask.Stencil);
        }

        public virtual void Clear(ClearBitMask bits, in DrawState drawState)
        {
            this.ApplyDrawState(drawState);
            this.Clear(bits);
        }

        public abstract void Clear(ClearBitMask bits);
        public abstract void Display();

        protected abstract void ApplyDrawState(in DrawState drawState);

        public virtual void DrawPrimitives(Shader shader, DrawPrimitiveType primitive, int offset, int count, in DrawState drawState)
        {
            this.ApplyDrawState(in drawState);
            this.DrawPrimitives(shader, primitive, offset, count);
            this.ApplyDrawState(this.DefaultDrawState);
        }

        public virtual void DrawPrimitivesInstanced(Shader shader, DrawPrimitiveType primitive, int offset, int count, int instanceCount, in DrawState drawState)
        {
            this.ApplyDrawState(in drawState);
            this.DrawPrimitivesInstanced(shader, primitive, offset, count, instanceCount);
            this.ApplyDrawState(this.DefaultDrawState);
        }

        public virtual void DrawIndexedPrimitives(Shader shader, DrawPrimitiveType primitive, int count, in DrawState drawState)
        {
            this.ApplyDrawState(in drawState);
            this.DrawIndexedPrimitives(shader, primitive, count);
            this.ApplyDrawState(this.DefaultDrawState);
        }

        public virtual void DrawIndexedPrimitivesInstanced(Shader shader, DrawPrimitiveType primitive, int count, int instanceCount, in DrawState drawState)
        {
            this.ApplyDrawState(in drawState);
            this.DrawIndexedPrimitivesInstanced(shader, primitive, count, instanceCount);
            this.ApplyDrawState(this.DefaultDrawState);
        }

        public abstract void DrawPrimitives(Shader shader, DrawPrimitiveType primitive, int offset, int count);
        public abstract void DrawPrimitivesInstanced(Shader shader, DrawPrimitiveType primitive, int offset, int count, int instanceCount);
        public abstract void DrawIndexedPrimitives(Shader shader, DrawPrimitiveType primitive, int count);
        public abstract void DrawIndexedPrimitivesInstanced(Shader shader, DrawPrimitiveType primitive, int count, int instanceCount);
    }

    public struct DrawState
    {
        public DepthTestMode DepthTest;
        public BlendMode Blend;

        public static DrawState Default => new DrawState()
        {
            DepthTest = DepthTestMode.None,
            Blend = BlendMode.None
        };
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

    public enum BlendMode
    {
        None,
        Alpha,
        Additive,
    }

    public enum DepthTestMode
    {
        None,
        Always,
        Never,
        Equal,
        NotEqual,
        Less,
        Greater,
        LessOrEqual,
        GreaterOrEqual,
    }
}