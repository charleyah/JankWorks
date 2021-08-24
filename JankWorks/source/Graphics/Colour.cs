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
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RGBA : IEquatable<RGBA>
    {
        public byte R;
        public byte G;
        public byte B;
        public byte A;

        public RGBA(byte r, byte g, byte b, byte a = 255)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }

        public unsafe RGBA(uint value)
        {
            this = *(RGBA*)&value;
        }

        public override bool Equals(object obj) => obj is RGBA other && this == other;
        public bool Equals(RGBA other) => this == other;
        public override int GetHashCode() => ((uint)this).GetHashCode();
        public override string ToString() => $"{nameof(RGBA)} {{ R = {this.R}, G = {this.G}, B = {this.B}, A = {this.A} }}";
        public static unsafe bool operator ==(RGBA left, RGBA right) => *(uint*)&left == *(uint*)&right;
        public static unsafe bool operator !=(RGBA left, RGBA right) => *(uint*)&left == *(uint*)&right;

        public static implicit operator ARGB(RGBA col) => new ARGB(col.R, col.G, col.B, col.A);

        public static implicit operator BGRA(RGBA col) => new BGRA(col.R, col.G, col.B, col.A);

        public static implicit operator ABGR(RGBA col) => new ABGR(col.R, col.G, col.B, col.A);

        public static unsafe explicit operator uint(RGBA col) => *(uint*)&col;

        public static explicit operator Vector4(RGBA col)
        {
            var vec = new Vector4(col.R, col.G, col.B, col.A);
            return Vector4.Divide(vec, 255);
        }

        public static explicit operator RGBA(Vector4 vec)
        {
            var veccol = Vector4.Multiply(vec, 255);
            return new RGBA(Convert.ToByte(veccol.X), Convert.ToByte(veccol.Y), Convert.ToByte(veccol.Z), Convert.ToByte(veccol.W));
        }

        public static explicit operator Vector3(RGBA col)
        {
            var vec = new Vector3(col.R, col.G, col.B);
            return Vector3.Divide(vec, 255);
        }

        public static explicit operator RGBA(Vector3 vec)
        {
            var veccol = Vector3.Multiply(vec, 255);
            return new RGBA(Convert.ToByte(veccol.X), Convert.ToByte(veccol.Y), Convert.ToByte(veccol.Z));
        }
    }


    /// <summary>
    /// Colour represented by 8-bits per channel in order of alpha, red, green and blue
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ARGB : IEquatable<ARGB>
    {
        public byte A;
        public byte R;
        public byte G;
        public byte B;

        public ARGB(byte r, byte g, byte b, byte a = 255)
        {
            this.A = a;
            this.R = r;
            this.G = g;
            this.B = b;
        }

        public unsafe ARGB(uint value)
        {
            this = *(ARGB*)&value;
        }
      
        public override bool Equals(object obj) => obj is ARGB other && this == other;
        public bool Equals(ARGB other) => this == other;
        public override int GetHashCode() => ((uint)this).GetHashCode();
        public override string ToString() => $"{nameof(ARGB)} {{ A = {this.A}, R = {this.R}, G = {this.G}, B = {this.B} }}";
        public static unsafe bool operator ==(ARGB left, ARGB right) => *(uint*)&left == *(uint*)&right;
        public static unsafe bool operator !=(ARGB left, ARGB right) => *(uint*)&left == *(uint*)&right;

        public static implicit operator RGBA(ARGB col) => new RGBA(col.R, col.G, col.B, col.A);

        public static implicit operator BGRA(ARGB col) => new BGRA(col.R, col.G, col.B, col.A);

        public static implicit operator ABGR(ARGB col) => new ABGR(col.R, col.G, col.B, col.A);

        public static unsafe explicit operator uint(ARGB col) => *(uint*)&col;

        public static explicit operator Vector4(ARGB col)
        {
            var vec = new Vector4(col.R, col.G, col.B, col.A);
            return Vector4.Divide(vec, 255);
        }

        public static explicit operator ARGB(Vector4 vec)
        {
            var veccol = Vector4.Multiply(vec, 255);
            return new ARGB(Convert.ToByte(veccol.X), Convert.ToByte(veccol.Y), Convert.ToByte(veccol.Z), Convert.ToByte(veccol.W));
        }

        public static explicit operator Vector3(ARGB col)
        {
            var vec = new Vector3(col.R, col.G, col.B);
            return Vector3.Divide(vec, 255);
        }

        public static explicit operator ARGB(Vector3 vec)
        {
            var veccol = Vector3.Multiply(vec, 255);
            return new ARGB(Convert.ToByte(veccol.X), Convert.ToByte(veccol.Y), Convert.ToByte(veccol.Z));
        }
    }



    /// <summary>
    /// Colour represented by 8-bits per channel in order of blue, green, red and alpha
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BGRA : IEquatable<BGRA>
    {
        public byte B;
        public byte G;
        public byte R;
        public byte A;

        public BGRA(byte r, byte g, byte b, byte a = 255)
        {            
            this.A = a;
            this.R = r;
            this.G = g;
            this.B = b;
        }

        public unsafe BGRA(uint value)
        {
            this = *(BGRA*)&value;
        }

        public override bool Equals(object obj) => obj is BGRA other && this == other;
        public bool Equals(BGRA other) => this == other;
        public override int GetHashCode() => ((uint)this).GetHashCode();
        public override string ToString() => $"{nameof(BGRA)} {{ B = {this.B}, G = {this.G}, R = {this.R}, A = {this.A} }}";
        public static unsafe bool operator ==(BGRA left, BGRA right) => *(uint*)&left == *(uint*)&right;
        public static unsafe bool operator !=(BGRA left, BGRA right) => *(uint*)&left == *(uint*)&right;

        public static implicit operator RGBA(BGRA col) => new RGBA(col.R, col.G, col.B, col.A);

        public static implicit operator ARGB(BGRA col) => new ARGB(col.R, col.G, col.B, col.A);

        public static implicit operator ABGR(BGRA col) => new ABGR(col.R, col.G, col.B, col.A);

        public static unsafe explicit operator uint(BGRA col) => *(uint*)&col;

        public static explicit operator Vector4(BGRA col)
        {
            var vec = new Vector4(col.B, col.G, col.R, col.A);
            return Vector4.Divide(vec, 255);
        }

        public static explicit operator BGRA(Vector4 vec)
        {
            var veccol = Vector4.Multiply(vec, 255);
            return new BGRA(Convert.ToByte(veccol.Z), Convert.ToByte(veccol.Y), Convert.ToByte(veccol.X), Convert.ToByte(veccol.W));
        }

        public static explicit operator Vector3(BGRA col)
        {
            var vec = new Vector3(col.B, col.G, col.R);
            return Vector3.Divide(vec, 255);
        }

        public static explicit operator BGRA(Vector3 vec)
        {
            var veccol = Vector3.Multiply(vec, 255);
            return new BGRA(Convert.ToByte(veccol.Z), Convert.ToByte(veccol.Y), Convert.ToByte(veccol.X));
        }
    }



    /// <summary>
    /// Colour represented by 8-bits per channel in order of alpha, blue, green and red
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ABGR : IEquatable<ABGR>
    {
        public byte A;
        public byte B;
        public byte G;
        public byte R;

        public ABGR(byte r, byte g, byte b, byte a = 255)
        {
            this.A = a;
            this.R = r;
            this.G = g;
            this.B = b;
        }

        public unsafe ABGR(uint value)
        {
            this = *(ABGR*)&value;
        }

        public override bool Equals(object obj) => obj is ABGR other && this == other;
        public bool Equals(ABGR other) => this == other;
        public override int GetHashCode() => ((uint)this).GetHashCode();
        public override string ToString() => $"{nameof(ABGR)} {{ A = {this.A}, B = {this.B}, G = {this.G}, R = {this.R} }}";
        public static unsafe bool operator ==(ABGR left, ABGR right) => *(uint*)&left == *(uint*)&right;
        public static unsafe bool operator !=(ABGR left, ABGR right) => *(uint*)&left == *(uint*)&right;

        public static implicit operator RGBA(ABGR col) => new RGBA(col.R, col.G, col.B, col.A);

        public static implicit operator ARGB(ABGR col) => new ARGB(col.R, col.G, col.B, col.A);

        public static implicit operator BGRA(ABGR col) => new BGRA(col.R, col.G, col.B, col.A);

        public static unsafe explicit operator uint(ABGR col) => *(uint*)&col;

        public static explicit operator Vector4(ABGR col)
        {
            var vec = new Vector4(col.A, col.B, col.G, col.R);
            return Vector4.Divide(vec, 255);
        }

        public static explicit operator ABGR(Vector4 vec)
        {
            var veccol = Vector4.Multiply(vec, 255);
            return new ABGR(Convert.ToByte(veccol.W), Convert.ToByte(veccol.Z), Convert.ToByte(veccol.Y), Convert.ToByte(veccol.X));
        }

        public static explicit operator Vector3(ABGR col)
        {
            var vec = new Vector3(col.B, col.G, col.R);
            return Vector3.Divide(vec, 255);
        }

        public static explicit operator ABGR(Vector3 vec)
        {
            var veccol = Vector3.Multiply(vec, 255);
            return new ABGR(Convert.ToByte(veccol.Z), Convert.ToByte(veccol.Y), Convert.ToByte(veccol.X));
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

        [FieldOffset(0)] private RGBA big;
        [FieldOffset(0)] private ABGR little;

        public unsafe RGBA32(byte r, byte g, byte b, byte a = 255)
        {
            if (BitConverter.IsLittleEndian)
            {
                var abgr = new ABGR(r, g, b, a);
                this = *(RGBA32*)&abgr;
            }
            else
            {
                var rgba = new RGBA(r, g, b, a);
                this = *(RGBA32*)&rgba;
            }
        }

        public unsafe RGBA32(uint value)
        {
            this = *(RGBA32*)&value;
        }
       
        public override bool Equals(object obj) => obj is RGBA32 other && this == other;
        public bool Equals(RGBA32 other) => this == other;
        public override int GetHashCode() => ((uint)this).GetHashCode();
        public override string ToString() => $"{nameof(RGBA32)} {{ R = {this.R}, G = {this.G}, B = {this.B}, A = {this.A} }}";
        public static unsafe bool operator ==(RGBA32 left, RGBA32 right) => *(uint*)&left == *(uint*)&right;
        public static unsafe bool operator !=(RGBA32 left, RGBA32 right) => *(uint*)&left == *(uint*)&right;

        public static implicit operator RGBA(RGBA32 col) => BitConverter.IsLittleEndian ? col.little : col.big;
        public static implicit operator ABGR(RGBA32 col) => BitConverter.IsLittleEndian ? col.little : col.big;

        public static implicit operator RGBA32(RGBA col) => BitConverter.IsLittleEndian ? new RGBA32(col.R, col.G, col.B, col.A) : new RGBA32((uint)col);
        public static implicit operator RGBA32(ABGR col) => BitConverter.IsLittleEndian ? new RGBA32((uint)col) : new RGBA32(col.R, col.G, col.B, col.A);


        public static unsafe explicit operator uint(RGBA32 col) => *(uint*)&col;
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

        [FieldOffset(0)] private ARGB big;
        [FieldOffset(0)] private BGRA little;


        public unsafe ARGB32(uint value)
        {
            this = *(ARGB32*)&value;
        }

        public unsafe ARGB32(byte r, byte g, byte b, byte a = 255)
        {
            if (BitConverter.IsLittleEndian)
            {
                var bgra = new BGRA(r, g, b, a);
                this = *(ARGB32*)&bgra;
            }
            else
            {
                var argb = new ARGB(r, g, b, a);
                this = *(ARGB32*)&argb;
            }
        }

        public override bool Equals(object obj) => obj is ARGB32 other && this == other;
        public bool Equals(ARGB32 other) => this == other;
        public override int GetHashCode() => ((uint)this).GetHashCode();
        public override string ToString() => $"{nameof(ARGB32)} {{ A = {this.A}, R ={this.R}, G = {this.G}, B = {this.B} }}";
        public static unsafe bool operator ==(ARGB32 left, ARGB32 right) => *(uint*)&left == *(uint*)&right;
        public static unsafe bool operator !=(ARGB32 left, ARGB32 right) => *(uint*)&left != *(uint*)&right;

        public static implicit operator ARGB(ARGB32 col) => BitConverter.IsLittleEndian ? col.little : col.big;
        public static implicit operator BGRA(ARGB32 col) => BitConverter.IsLittleEndian ? col.little : col.big;

        public static implicit operator ARGB32(ARGB col) => BitConverter.IsLittleEndian ? new ARGB32(col.R, col.G, col.B, col.A) : new ARGB32((uint)col);
        public static implicit operator ARGB32(BGRA col) => BitConverter.IsLittleEndian ? new ARGB32((uint)col) : new ARGB32(col.R, col.G, col.B, col.A);

        public static unsafe explicit operator uint(ARGB32 col) => *(uint*)&col;
    }

    public static class Colour
    {
        public static readonly RGBA Transparent = new RGBA(0, 0, 0, 0);
        public static readonly RGBA Black = new RGBA(0, 0, 0);
        public static readonly RGBA White = new RGBA(255, 255, 255);

        public static readonly RGBA Red = new RGBA(255, 0, 0);
        public static readonly RGBA Green = new RGBA(0, 255, 0);
        public static readonly RGBA Blue = new RGBA(0, 0, 255);

        public static readonly RGBA Yellow = new RGBA(255, 255, 0);
        public static readonly RGBA Pink = new RGBA(255, 0, 255);
        public static readonly RGBA Cyan = new RGBA(0, 255, 255);

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
