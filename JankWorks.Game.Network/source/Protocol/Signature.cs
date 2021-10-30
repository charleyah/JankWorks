using System;
using System.Runtime.InteropServices;

namespace JankWorks.Game.Network.Protocol
{
    [StructLayout(LayoutKind.Explicit)]
    struct Signature : IEquatable<Signature>
    {
        [FieldOffset(0)]
        private uint value;

        [FieldOffset(0)]
        private byte j;

        [FieldOffset(1)]
        private byte w;

        [FieldOffset(2)]
        private byte g;

        [FieldOffset(3)]
        private byte p;

        public override int GetHashCode() => this.value.GetHashCode();

        public override bool Equals(object obj) => obj is Signature other && this == other;

        public bool Equals(Signature other) => this == other;

        public static bool operator ==(Signature left, Signature right) => left.value == right.value;

        public static bool operator !=(Signature left, Signature right) => left.value != right.value;

        public static readonly Signature Valid = new Signature()
        {
            j = 74,
            w = 87,
            g = 71,
            p = 80
        };
    }
}