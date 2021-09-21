using System;
using System.Numerics;

using JankWorks.Core;

namespace JankWorks.Graphics
{
    public abstract class TextRenderer : Disposable
    {
        public Camera Camera { get; set; }

        public Font Font { get; set; }

        public abstract void BeginDraw();

        public abstract void BeginDraw(DrawState state);

        public abstract void ReDraw(Surface surface);

        public abstract void ReDraw(Surface surface, DrawState state);

        public virtual void Draw(ReadOnlySpan<char> text, Vector2 position, RGBA colour)
        {
            this.Draw(text, position, Vector2.Zero, colour);
        }

        public abstract void Draw(ReadOnlySpan<char> text, Vector2 position, Vector2 origin, RGBA colour);

        public abstract void EndDraw(Surface surface);

        public abstract void Flush();
    }
}