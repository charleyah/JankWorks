using System;
using System.Runtime.InteropServices;

namespace Pong.Match.Players
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct PlayerEvent
    {
        public const byte Channel = 2;

        public byte PlayerNumber;

        public PlayerMovement Movement;
    }

    enum PlayerMovement : sbyte
    {
        Up = 1,

        Still = 0,

        Down = -1
    }
}