using System.Runtime.InteropServices;

namespace Pong.Match.Physics
{
    [StructLayout(LayoutKind.Explicit)]
    struct PhysicsEvent
    {
        public const byte Channel = 1;

        public enum Type : byte
        {
            DataCount,

            Data
        }

        [FieldOffset(0)]
        public Type type;

        [FieldOffset(1)]
        public ushort componentId;

        [FieldOffset(1)]
        public ushort componentCount;

        [FieldOffset(3)]
        public PhysicsComponent data;
    }
}