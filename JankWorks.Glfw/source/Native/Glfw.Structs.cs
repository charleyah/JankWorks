// Sourced from https://github.com/smack0007/GLFWDotNet

using System;

namespace JankWorks.Drivers.Glfw.Native
{
    public struct GLFWvidmode
    {
        public int width;
        public int height;
        public int redBits;
        public int greenBits;
        public int blueBits;
        public int refreshRate;
    }

    public struct GLFWgammaramp
    {
        public ushort[] red;
        public ushort[] green;
        public ushort[] blue;
        public uint size;
    }

    public struct GLFWimage
    {
        public int width;
        public int height;
        public IntPtr pixels;
    }

    public struct GLFWgamepadstate
    {
        public IntPtr buttons;
        public float axes;
    }
}
