using System;
using System.Numerics;

using JankWorks.Core;

namespace JankWorks.Graphics
{
    public abstract class TextRenderer : Disposable
    {
        public abstract Camera Camera { get; set; }

        public abstract void SetFont(Font font);

        public abstract void Reserve(int charCount);

        public abstract void Clear();

        public abstract void BeginDraw();

        public abstract void BeginDraw(DrawState state);

        public abstract bool ReDraw(Surface surface);

        public virtual Bounds Draw(ReadOnlySpan<char> text, Vector2 position, RGBA colour)
        {
            return this.Draw(text, position, Vector2.Zero, 0f, colour);
        }

        public virtual Bounds Draw(ReadOnlySpan<char> text, Vector2 position, Func<char, int, RGBA> colourpicker)
        {
            return this.Draw(text, position, Vector2.Zero, 0f, colourpicker);
        }

        public abstract Bounds Draw(ReadOnlySpan<char> text, Vector2 position, Vector2 origin, float rotation, RGBA colour);

        public abstract Bounds Draw(ReadOnlySpan<char> text, Vector2 position, Vector2 origin, float rotation, Func<char, int, RGBA> colourpicker);

        public abstract void EndDraw(Surface surface);
    }
}