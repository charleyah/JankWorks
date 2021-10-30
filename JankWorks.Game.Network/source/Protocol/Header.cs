using System.Runtime.InteropServices;   

namespace JankWorks.Game.Network.Protocol
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct Header
    {
        public Signature Signature;        
        public PacketType Type;
        public byte Channel;
        public ushort Length;
    }
}
