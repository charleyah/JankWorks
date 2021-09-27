using System;
using System.Numerics;

using JankWorks.Core;

namespace JankWorks.Graphics
{
    public abstract class SpriteRenderer : Disposable
    {
        public enum DrawOrder
        {
            Sequential,
            Reversed,
            Texture
        }

        public abstract DrawOrder Order { get; set; }

        public abstract Camera Camera { get; set; }

        public abstract void Clear();

        public abstract void Reserve(int spriteCount);

        public abstract void BeginDraw();

        public abstract void BeginDraw(DrawState state);

        public abstract bool ReDraw(Surface surface);

        public virtual void Draw(Texture2D texture, Vector2 position, Vector2 size)
        {
            this.Draw(texture, position, size, Vector2.Zero, 0f, Colour.White, Bounds.One);
        }

        public virtual void Draw(Texture2D texture, Vector2 position, Vector2 size, Vector2 origin)
        {
            this.Draw(texture, position, size, origin, 0f, Colour.White, Bounds.One);
        }

        public virtual void Draw(Texture2D texture, Vector2 position, Vector2 size, Vector2 origin, float rotation)
        {
            this.Draw(texture, position, size, origin, rotation, Colour.White, Bounds.One);
        }

        public virtual void Draw(Texture2D texture, Vector2 position, Vector2 size, Vector2 origin, RGBA colour, Bounds textureBounds)
        {
            this.Draw(texture, position, size, origin, 0f, colour, textureBounds);
        }

        public abstract void Draw(Texture2D texture, Vector2 position, Vector2 size, Vector2 origin, float rotation, RGBA colour, Bounds textureBounds);
        
        public abstract void EndDraw(Surface surface);
    }
}