using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace JankWorks.Graphics
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Bounds : IEquatable<Bounds>
    {
        public Vector2 TopRight => new Vector2(this.BottomRight.X, this.TopLeft.Y);

        public Vector2 BottomLeft => new Vector2(this.TopLeft.X, this.BottomRight.Y);

        public Vector2 TopLeft { get; set; }

        public Vector2 BottomRight { get; set; }

        public Vector2 Size
        {
            get
            {
                return new Vector2(this.BottomRight.X - this.TopLeft.X, this.BottomRight.Y - this.TopLeft.Y);
            }
        }

        public Bounds(float left, float top, float right, float bottom)
        {
            this.TopLeft = new Vector2(left, top);
            this.BottomRight = new Vector2(right, bottom);
        }

        public Bounds(Vector2 position, Vector2 size)
        {
            this.TopLeft = position;
            this.BottomRight = position + size;
        }

        public bool Contains(Vector2 pos)
        {
            var tl = this.TopLeft;
            var br = this.BottomRight;

            return pos.X >= tl.X && pos.X <= br.X && pos.Y >= tl.Y && pos.Y <= br.Y;
        }

        public bool Intersects(Bounds other)
        {            
            Bounds src;
            Bounds des;
            
            if (this.TopLeft.X <= other.TopLeft.X)
            {
                src = this;
                des = other;
            }
            else
            {
                src = other;
                des = this;
            }

            if(src.BottomRight.X >= des.TopLeft.X)
            {
                if (this.TopLeft.Y <= other.TopLeft.Y)
                {
                    src = this;
                    des = other;
                }
                else
                {
                    src = other;
                    des = this;
                }

                return src.BottomRight.Y >= des.TopLeft.Y;
            }

            return false;                

        }
        public override int GetHashCode() => this.TopLeft.GetHashCode() ^ this.BottomRight.GetHashCode();
        public override bool Equals(object obj) => obj is Bounds other && this == other;
        public bool Equals(Bounds other) => this == other;
        public static bool operator ==(Bounds a, Bounds b) => a.TopLeft == b.TopLeft && a.BottomRight == b.BottomRight;
        public static bool operator !=(Bounds a, Bounds b) => a.TopLeft != b.TopLeft || a.BottomRight != b.BottomRight;


        public static Bounds Zero = new Bounds(Vector2.Zero, Vector2.Zero);
        public static Bounds One => new Bounds(Vector2.Zero, Vector2.One);
    }
}