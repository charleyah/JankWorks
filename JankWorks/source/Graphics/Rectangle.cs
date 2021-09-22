using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace JankWorks.Graphics
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Rectangle : IEquatable<Rectangle>
    {
        public Vector2i Position;
        public Vector2i Size;

        public Rectangle(int x, int y, int width, int height) : this(new Vector2i(x, y), new Vector2i(width, height)) { }

        public Rectangle(Vector2i position, Vector2i size)
        {
            this.Position = position;
            this.Size = size;
        }
        public override int GetHashCode() => this.Position.GetHashCode() ^ this.Size.GetHashCode();
        public override bool Equals(object obj) => obj is Rectangle other && this == other;
        public bool Equals(Rectangle other) => this == other;
        public static bool operator ==(Rectangle a, Rectangle b) => a.Position == b.Position && a.Size == b.Position;
        public static bool operator !=(Rectangle a, Rectangle b) => a.Position != b.Position || a.Size != b.Position;       
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Bounds : IEquatable<Bounds>
    {
        public Vector2 TopRight => new Vector2(this.BottomRight.X, this.TopLeft.Y);

        public Vector2 BottomLeft => new Vector2(this.TopLeft.X, this.BottomRight.Y);

        public Vector2 TopLeft { get; set; }

        public Vector2 BottomRight { get; set; }

        public Bounds(float left, float top, float bottom, float right)
        {
            this.TopLeft = new Vector2(left, top);
            this.BottomRight = new Vector2(right, bottom);
        }

        public Bounds(Vector2 position, Vector2 size)
        {
            this.TopLeft = position;
            this.BottomRight = position + size;
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