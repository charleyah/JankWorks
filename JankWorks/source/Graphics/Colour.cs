using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace JankWorks.Graphics
{
    /// <summary>
    /// Colour represented by 8-bits per channel in order of red, green, blue and alpha
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Explicit)]
    public struct RGBA : IEquatable<RGBA>
    {
        [FieldOffset(0)] private uint value;

        [FieldOffset(0)] public byte R;

        [FieldOffset(1)] public byte G;

        [FieldOffset(2)] public byte B;

        [FieldOffset(3)] public byte A;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RGBA From(uint value)
        {
            RGBA col = default;
            col.value = value;
            return col;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RGBA From(byte r, byte g, byte b, byte a = 255)
        {
            RGBA col = default;
            col.R = r;
            col.G = g;
            col.B = b;
            col.A = a;

            return col;
        }

        public override bool Equals(object obj) => obj is RGBA other && this == other;
        public bool Equals(RGBA other) => this == other;
        public override int GetHashCode() => this.value.GetHashCode();
        public override string ToString() => $"R{this.R} G{this.G} B{this.B} A{this.A}";
        public static bool operator ==(RGBA left, RGBA right) => left.value == right.value;
        public static bool operator !=(RGBA left, RGBA right) => left.value != right.value;

        public static implicit operator ARGB(RGBA col) => ARGB.From(col.R, col.G, col.B, col.A);

        public static implicit operator BGRA(RGBA col) => BGRA.From(col.R, col.G, col.B, col.A);

        public static implicit operator ABGR(RGBA col) => ABGR.From(col.R, col.G, col.B, col.A);

        public static explicit operator uint(RGBA col) => col.value;

        public static explicit operator Vector4(RGBA col)
        {
            var vec = new Vector4(col.R, col.G, col.B, col.A);
            return Vector4.Divide(vec, 255);
        }

        public static explicit operator RGBA(Vector4 vec)
        {
            var veccol = Vector4.Multiply(vec, 255);
            return RGBA.From(Convert.ToByte(veccol.X), Convert.ToByte(veccol.Y), Convert.ToByte(veccol.Z), Convert.ToByte(veccol.W));
        }

        public static explicit operator Vector3(RGBA col)
        {
            var vec = new Vector3(col.R, col.G, col.B);
            return Vector3.Divide(vec, 255);
        }

        public static explicit operator RGBA(Vector3 vec)
        {
            var veccol = Vector3.Multiply(vec, 255);
            return RGBA.From(Convert.ToByte(veccol.X), Convert.ToByte(veccol.Y), Convert.ToByte(veccol.Z));
        }
    }


    /// <summary>
    /// Colour represented by 8-bits per channel in order of alpha, red, green and blue
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Explicit)]
    public struct ARGB : IEquatable<ARGB>
    {
        [FieldOffset(0)] private uint value;

        [FieldOffset(0)] public byte A;

        [FieldOffset(1)] public byte R;

        [FieldOffset(2)] public byte G;

        [FieldOffset(3)] public byte B;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ARGB From(uint value)
        {
            ARGB col = default;
            col.value = value;
            return col;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ARGB From(byte r, byte g, byte b, byte a = 255)
        {
            ARGB col = default;
            col.A = a;
            col.R = r;
            col.G = g;
            col.B = b;

            return col;
        }

        public override bool Equals(object obj) => obj is ARGB other && this == other;
        public bool Equals(ARGB other) => this == other;
        public override int GetHashCode() => this.value.GetHashCode();
        public override string ToString() => $"A{this.A} R{this.R} G{this.G} B{this.B}";
        public static bool operator ==(ARGB left, ARGB right) => left.value == right.value;
        public static bool operator !=(ARGB left, ARGB right) => left.value != right.value;

        public static implicit operator RGBA(ARGB col) => RGBA.From(col.R, col.G, col.B, col.A);

        public static implicit operator BGRA(ARGB col) => BGRA.From(col.R, col.G, col.B, col.A);

        public static implicit operator ABGR(ARGB col) => ABGR.From(col.R, col.G, col.B, col.A);

        public static explicit operator uint(ARGB col) => col.value;

        public static explicit operator Vector4(ARGB col)
        {
            var vec = new Vector4(col.R, col.G, col.B, col.A);
            return Vector4.Divide(vec, 255);
        }

        public static explicit operator ARGB(Vector4 vec)
        {
            var veccol = Vector4.Multiply(vec, 255);
            return ARGB.From(Convert.ToByte(veccol.X), Convert.ToByte(veccol.Y), Convert.ToByte(veccol.Z), Convert.ToByte(veccol.W));
        }

        public static explicit operator Vector3(ARGB col)
        {
            var vec = new Vector3(col.R, col.G, col.B);
            return Vector3.Divide(vec, 255);
        }

        public static explicit operator ARGB(Vector3 vec)
        {
            var veccol = Vector3.Multiply(vec, 255);
            return ARGB.From(Convert.ToByte(veccol.X), Convert.ToByte(veccol.Y), Convert.ToByte(veccol.Z));
        }
    }



    /// <summary>
    /// Colour represented by 8-bits per channel in order of blue, green, red and alpha
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Explicit)]
    public struct BGRA : IEquatable<BGRA>
    {
        [FieldOffset(0)] private uint value;

        [FieldOffset(0)] public byte B;

        [FieldOffset(1)] public byte G;

        [FieldOffset(2)] public byte R;

        [FieldOffset(3)] public byte A;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BGRA From(uint value)
        {
            BGRA col = default;
            col.value = value;
            return col;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BGRA From(byte r, byte g, byte b, byte a = 255)
        {
            BGRA col = default;
            col.A = a;
            col.R = r;
            col.G = g;
            col.B = b;

            return col;
        }

        public override bool Equals(object obj) => obj is BGRA other && this == other;
        public bool Equals(BGRA other) => this == other;
        public override int GetHashCode() => this.value.GetHashCode();
        public override string ToString() => $"B{this.B} G{this.G} R{this.R} A{this.A}";
        public static bool operator ==(BGRA left, BGRA right) => left.value == right.value;
        public static bool operator !=(BGRA left, BGRA right) => left.value != right.value;

        public static implicit operator RGBA(BGRA col) => RGBA.From(col.R, col.G, col.B, col.A);

        public static implicit operator ARGB(BGRA col) => ARGB.From(col.R, col.G, col.B, col.A);

        public static implicit operator ABGR(BGRA col) => ABGR.From(col.R, col.G, col.B, col.A);

        public static explicit operator uint(BGRA col) => col.value;

        public static explicit operator Vector4(BGRA col)
        {
            var vec = new Vector4(col.B, col.G, col.R, col.A);
            return Vector4.Divide(vec, 255);
        }

        public static explicit operator BGRA(Vector4 vec)
        {
            var veccol = Vector4.Multiply(vec, 255);
            return BGRA.From(Convert.ToByte(veccol.Z), Convert.ToByte(veccol.Y), Convert.ToByte(veccol.X), Convert.ToByte(veccol.W));
        }

        public static explicit operator Vector3(BGRA col)
        {
            var vec = new Vector3(col.B, col.G, col.R);
            return Vector3.Divide(vec, 255);
        }

        public static explicit operator BGRA(Vector3 vec)
        {
            var veccol = Vector3.Multiply(vec, 255);
            return BGRA.From(Convert.ToByte(veccol.Z), Convert.ToByte(veccol.Y), Convert.ToByte(veccol.X));
        }
    }



    /// <summary>
    /// Colour represented by 8-bits per channel in order of alpha, blue, green and red
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Explicit)]
    public struct ABGR : IEquatable<ABGR>
    {
        [FieldOffset(0)] private uint value;

        [FieldOffset(0)] public byte A;

        [FieldOffset(1)] public byte B;

        [FieldOffset(2)] public byte G;

        [FieldOffset(3)] public byte R;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ABGR From(uint value)
        {
            ABGR col = default;
            col.value = value;
            return col;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ABGR From(byte r, byte g, byte b, byte a = 255)
        {
            ABGR col = default;
            col.A = a;
            col.R = r;
            col.G = g;
            col.B = b;

            return col;
        }

        public override bool Equals(object obj) => obj is ABGR other && this == other;
        public bool Equals(ABGR other) => this == other;
        public override int GetHashCode() => this.value.GetHashCode();
        public override string ToString() => $"A{this.A} B{this.B} G{this.G} R{this.R}";
        public static bool operator ==(ABGR left, ABGR right) => left.value == right.value;
        public static bool operator !=(ABGR left, ABGR right) => left.value != right.value;

        public static implicit operator RGBA(ABGR col) => RGBA.From(col.R, col.G, col.B, col.A);

        public static implicit operator ARGB(ABGR col) => ARGB.From(col.R, col.G, col.B, col.A);

        public static implicit operator BGRA(ABGR col) => BGRA.From(col.R, col.G, col.B, col.A);

        public static explicit operator uint(ABGR col) => col.value;

        public static explicit operator Vector4(ABGR col)
        {
            var vec = new Vector4(col.A, col.B, col.G, col.R);
            return Vector4.Divide(vec, 255);
        }

        public static explicit operator ABGR(Vector4 vec)
        {
            var veccol = Vector4.Multiply(vec, 255);
            return ABGR.From(Convert.ToByte(veccol.W), Convert.ToByte(veccol.Z), Convert.ToByte(veccol.Y), Convert.ToByte(veccol.X));
        }

        public static explicit operator Vector3(ABGR col)
        {
            var vec = new Vector3(col.B, col.G, col.R);
            return Vector3.Divide(vec, 255);
        }

        public static explicit operator ABGR(Vector3 vec)
        {
            var veccol = Vector3.Multiply(vec, 255);
            return ABGR.From(Convert.ToByte(veccol.Z), Convert.ToByte(veccol.Y), Convert.ToByte(veccol.X));
        }
    }



    /// <summary>
    /// Colour represented by 32-bits thats endian dependent. Format is RGBA on big endian and ABGR on little endian
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Explicit)]
    public struct RGBA32 : IEquatable<RGBA32>
    {
        public byte R 
        {
            get => BitConverter.IsLittleEndian ? this.little.R : this.big.R;
            set
            {
                if (BitConverter.IsLittleEndian) { this.little.R = value; }
                else { this.big.R = value; }
            }
        }

        public byte G
        {
            get => BitConverter.IsLittleEndian ? this.little.G : this.big.G;
            set
            {
                if (BitConverter.IsLittleEndian) { this.little.G = value; }
                else { this.big.G = value; }
            }
        }


        public byte B
        {
            get => BitConverter.IsLittleEndian ? this.little.B : this.big.B;
            set
            {
                if (BitConverter.IsLittleEndian) { this.little.B = value; }
                else { this.big.B = value; }
            }
        }

        public byte A
        {
            get => BitConverter.IsLittleEndian ? this.little.A : this.big.A;
            set
            {
                if (BitConverter.IsLittleEndian) { this.little.A = value; }
                else { this.big.A = value; }
            }
        }


        [FieldOffset(0)] private uint value;
        [FieldOffset(0)] private RGBA big;
        [FieldOffset(0)] private ABGR little;


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RGBA32 From(uint value)
        {
            RGBA32 col = default;
            col.value = value;
            return col;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RGBA32 From(byte r, byte g, byte b, byte a = 255)
        {
            RGBA32 col = default;

            if(BitConverter.IsLittleEndian)
            {
                col.little.A = a;
                col.little.R = r;
                col.little.G = g;
                col.little.B = b;
            }
            else
            {
                col.big.A = a;
                col.big.R = r;
                col.big.G = g;
                col.big.B = b;
            }
            

            return col;
        }

        public override bool Equals(object obj) => obj is RGBA32 other && this == other;
        public bool Equals(RGBA32 other) => this == other;
        public override int GetHashCode() => this.value.GetHashCode();
        public override string ToString() => $"R{this.R} G{this.G} B{this.B} A{this.A}";
        public static bool operator ==(RGBA32 left, RGBA32 right) => left.value == right.value;
        public static bool operator !=(RGBA32 left, RGBA32 right) => left.value != right.value;

        public static implicit operator RGBA(RGBA32 col) => BitConverter.IsLittleEndian ? col.little : col.big;
        public static implicit operator ABGR(RGBA32 col) => BitConverter.IsLittleEndian ? col.little : col.big;

        public static implicit operator RGBA32(RGBA col) => BitConverter.IsLittleEndian ? RGBA32.From(col.R, col.G, col.B, col.A) : RGBA32.From((uint)col);
        public static implicit operator RGBA32(ABGR col) => BitConverter.IsLittleEndian ? RGBA32.From((uint)col) : RGBA32.From(col.R, col.G, col.B, col.A);


        public static explicit operator uint(RGBA32 col) => col.value;
    }


    /// <summary>
    /// Colour represented by 32-bits thats endian dependent. Format is ARGB on big endian and BGRA on little endian
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Explicit)]
    public struct ARGB32 : IEquatable<ARGB32>
    {
        public byte R
        {
            get => BitConverter.IsLittleEndian ? this.little.R : this.big.R;
            set
            {
                if (BitConverter.IsLittleEndian) { this.little.R = value; }
                else { this.big.R = value; }
            }
        }

        public byte G
        {
            get => BitConverter.IsLittleEndian ? this.little.G : this.big.G;
            set
            {
                if (BitConverter.IsLittleEndian) { this.little.G = value; }
                else { this.big.G = value; }
            }
        }


        public byte B
        {
            get => BitConverter.IsLittleEndian ? this.little.B : this.big.B;
            set
            {
                if (BitConverter.IsLittleEndian) { this.little.B = value; }
                else { this.big.B = value; }
            }
        }

        public byte A
        {
            get => BitConverter.IsLittleEndian ? this.little.A : this.big.A;
            set
            {
                if (BitConverter.IsLittleEndian) { this.little.A = value; }
                else { this.big.A = value; }
            }
        }


        [FieldOffset(0)] private uint value;
        [FieldOffset(0)] private ARGB big;
        [FieldOffset(0)] private BGRA little;


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ARGB32 From(uint value)
        {
            ARGB32 col = default;
            col.value = value;
            return col;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ARGB32 From(byte r, byte g, byte b, byte a = 255)
        {
            ARGB32 col = default;

            if (BitConverter.IsLittleEndian)
            {
                col.little.A = a;
                col.little.R = r;
                col.little.G = g;
                col.little.B = b;
            }
            else
            {
                col.big.A = a;
                col.big.R = r;
                col.big.G = g;
                col.big.B = b;
            }


            return col;
        }

        public override bool Equals(object obj) => obj is ARGB32 other && this == other;
        public bool Equals(ARGB32 other) => this == other;
        public override int GetHashCode() => this.value.GetHashCode();
        public override string ToString() => $"A{this.A} R{this.R} G{this.G} B{this.B}";
        public static bool operator ==(ARGB32 left, ARGB32 right) => left.value == right.value;
        public static bool operator !=(ARGB32 left, ARGB32 right) => left.value != right.value;

        public static implicit operator ARGB(ARGB32 col) => BitConverter.IsLittleEndian ? col.little : col.big;
        public static implicit operator BGRA(ARGB32 col) => BitConverter.IsLittleEndian ? col.little : col.big;

        public static implicit operator ARGB32(ARGB col) => BitConverter.IsLittleEndian ? ARGB32.From(col.R, col.G, col.B, col.A) : ARGB32.From((uint)col);
        public static implicit operator ARGB32(BGRA col) => BitConverter.IsLittleEndian ? ARGB32.From((uint)col) : ARGB32.From(col.R, col.G, col.B, col.A);

        public static explicit operator uint(ARGB32 col) => col.value;
    }

    public static class Colour
    {
        public static readonly RGBA Transparent = RGBA.From(0, 0, 0, 0);
        public static readonly RGBA Black = RGBA.From(0, 0, 0);
        public static readonly RGBA White = RGBA.From(255, 255, 255);

        public static readonly RGBA Red = RGBA.From(255, 0, 0);
        public static readonly RGBA Green = RGBA.From(0, 255, 0);
        public static readonly RGBA Blue = RGBA.From(0, 0, 255);

        public static readonly RGBA Yellow = RGBA.From(255, 255, 0);
        public static readonly RGBA Pink = RGBA.From(255, 0, 255);
        public static readonly RGBA Cyan = RGBA.From(0, 255, 255);
        public static RGBA From(byte r, byte g, byte b, byte a = byte.MaxValue) => RGBA.From(r, g, b, a);

        public static RGBA Blend(RGBA source, RGBA additive, float amount)
        {
            return (RGBA)Vector4.Lerp((Vector4)source, (Vector4)additive, amount);
        }

        public static void Copy(this ReadOnlySpan<ABGR> source, Span<RGBA> destination)
        {
            if(source.IsEmpty || source.Length > destination.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            unsafe
            {
                fixed(ABGR* sourceptr = source)
                {
                    fixed(RGBA* desintationptr = destination)
                    {
                        int copied = 0;

                        do
                        {
                            desintationptr[copied].A = sourceptr[copied].A;
                            desintationptr[copied].R = sourceptr[copied].R;
                            desintationptr[copied].G = sourceptr[copied].G;
                            desintationptr[copied].B = sourceptr[copied].B;
                            copied++;
                        }
                        while (copied < source.Length);
                    }
                }
            }
        }
        public static void Copy(this ReadOnlySpan<RGBA> source, Span<ABGR> destination)
        {
            if (source.IsEmpty || source.Length > destination.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            unsafe
            {
                fixed (RGBA* sourceptr = source)
                {
                    fixed (ABGR* desintationptr = destination)
                    {
                        int copied = 0;

                        do
                        {
                            desintationptr[copied].A = sourceptr[copied].A;
                            desintationptr[copied].R = sourceptr[copied].R;
                            desintationptr[copied].G = sourceptr[copied].G;
                            desintationptr[copied].B = sourceptr[copied].B;
                            copied++;
                        }
                        while (copied < source.Length);
                    }
                }
            }
        }

        public static void Copy(this ReadOnlySpan<ARGB> source, Span<BGRA> destination)
        {
            if (source.IsEmpty || source.Length > destination.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            unsafe
            {
                fixed (ARGB* sourceptr = source)
                {
                    fixed (BGRA* desintationptr = destination)
                    {
                        int copied = 0;

                        do
                        {
                            desintationptr[copied].A = sourceptr[copied].A;
                            desintationptr[copied].R = sourceptr[copied].R;
                            desintationptr[copied].G = sourceptr[copied].G;
                            desintationptr[copied].B = sourceptr[copied].B;
                            copied++;
                        }
                        while (copied < source.Length);
                    }
                }
            }
        }
        public static void Copy(this ReadOnlySpan<BGRA> source, Span<ARGB> destination)
        {
            if (source.IsEmpty || source.Length > destination.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            unsafe
            {
                fixed (BGRA* sourceptr = source)
                {
                    fixed (ARGB* desintationptr = destination)
                    {
                        int copied = 0;

                        do
                        {
                            desintationptr[copied].A = sourceptr[copied].A;
                            desintationptr[copied].R = sourceptr[copied].R;
                            desintationptr[copied].G = sourceptr[copied].G;
                            desintationptr[copied].B = sourceptr[copied].B;
                            copied++;
                        }
                        while (copied < source.Length);
                    }
                }
            }
        }
    }
}
