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
       
        public abstract void BeginDraw();

        public abstract void BeginDraw(DrawState state);

        public abstract void ReDraw(Surface surface);

        public abstract void ReDraw(Surface surface, DrawState state);

        public virtual void Draw(Texture2D texture, Bounds destination)
        {
            this.Draw(texture, destination, Bounds.One, Colour.White);
        }

        public virtual void Draw(Texture2D texture, Bounds destination, Bounds source)
        {
            this.Draw(texture, destination, Bounds.One, Colour.White);
        }

        public virtual void Draw(Texture2D texture, Bounds destination, Bounds source, RGBA colour)
        {
            this.Draw(texture, destination.Position, destination.Size, Vector2.Zero, 0f, colour, source);
        }

        public abstract void Draw(Texture2D texture, Vector2 position, Vector2 size, Vector2 origin, float rotation, RGBA colour, Bounds source);
        
        public abstract void EndDraw(Surface surface);

        public abstract void Flush();
    }
}