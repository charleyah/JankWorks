using System;
using System.Runtime.InteropServices;
using System.Numerics;

namespace JankWorks.Graphics
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2i : IEquatable<Vector2i>
    {
        public int X;
        public int Y;

        public Vector2i(int value) : this(value, value) { }
        public Vector2i(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public override string ToString() => $"{ nameof(Vector2i) } {{ X = { this.X }, Y = { this.Y } }}";

        public override int GetHashCode() => this.X.GetHashCode() ^ this.Y.GetHashCode();       
        public override bool Equals(object obj) => obj is Vector2i other && this == other;
        public bool Equals(Vector2i other) => this == other;
        public static bool operator ==(Vector2i a, Vector2i b) => a.X == b.X && a.Y == b.Y;
        public static bool operator !=(Vector2i a, Vector2i b) => a.X != b.X || a.Y != b.Y;

        public static Vector2i operator +(Vector2i a) => new Vector2i(+a.X, +a.Y);
        public static Vector2i operator -(Vector2i a) => new Vector2i(-a.X, -a.Y);
        public static Vector2i operator +(Vector2i a, Vector2i b) => new Vector2i(a.X + b.X, a.Y + b.Y);
        public static Vector2i operator -(Vector2i a, Vector2i b) => new Vector2i(a.X - b.X, a.Y - b.Y);
        public static Vector2i operator /(Vector2i a, Vector2i b) => new Vector2i(a.X / b.X, a.Y / b.Y);
        public static Vector2i operator *(Vector2i a, Vector2i b) => new Vector2i(a.X * b.X, a.Y * b.Y);

        public static Vector2i FromVector2(Vector2 vec, MidpointRounding rounding)
        {
            return new Vector2i((int)Math.Round(vec.X, 0, rounding), (int)Math.Round(vec.Y, 0, rounding));
        }

        public static explicit operator Vector2i(Vector2 vec) => new Vector2i(Convert.ToInt32(vec.X), Convert.ToInt32(vec.Y));

        public static explicit operator Vector2(Vector2i vec) => new Vector2(Convert.ToSingle(vec.X), Convert.ToSingle(vec.Y));

        public static Vector2i Zero => new Vector2i(0);
    }
}