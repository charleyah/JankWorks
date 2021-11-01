using System.Numerics;

using JankWorks.Core;

namespace JankWorks.Graphics
{
    public abstract class ShapeRenderer : Disposable
    {
        public abstract Camera Camera { get; set; }

        public abstract void Reserve(int vertices);

        public abstract void Clear();
       
        public abstract void BeginDraw();

        public abstract void BeginDraw(DrawState state);

        public abstract bool ReDraw(Surface surface);

        public abstract void DrawLine(Vector2 start, Vector2 end, RGBA colour, float thickness);

        public virtual void DrawRectangle(Vector2 size, Vector2 position, RGBA fillcolour)
        {
            this.DrawRectangle(size, position, Vector2.Zero, 0f, fillcolour);
        }

        public abstract void DrawRectangle(Vector2 size, Vector2 position, Vector2 origin, float rotation, RGBA fillcolour);

        public virtual void DrawTriangle(Vector2 size, Vector2 position, RGBA fillcolour)
        {
            this.DrawTriangle(size, position, Vector2.Zero, 0f, fillcolour);
        }

        public abstract void DrawTriangle(Vector2 size, Vector2 position, Vector2 origin, float rotation, RGBA fillcolour);

        public abstract void EndDraw(Surface surface);
    }
}