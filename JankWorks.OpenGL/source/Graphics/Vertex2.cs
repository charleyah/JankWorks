using System.Numerics;
using System.Runtime.InteropServices;

namespace JankWorks.Drivers.OpenGL.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct Vertex2
    {
        public Vector2 position;
        public Vector2 texcoord;
        public Vector4 colour;

        public Vertex2(Vector2 position, Vector2 texcoord, Vector4 colour)
        {
            this.position = position;
            this.texcoord = texcoord;
            this.colour = colour;
        }
    }
}