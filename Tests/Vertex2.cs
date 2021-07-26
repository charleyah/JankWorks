using System;
using System.Runtime.InteropServices;
using System.Numerics;

using JankWorks.Graphics;

namespace Tests
{
    [StructLayout(LayoutKind.Sequential)]
    struct Vertex2
    {
        public Vector2 Position;
        public Vector3 Colour;
        public Vector2 TexCoords;
        public Vertex2(Vector2 position, RGBA colour, Vector2 texcoords) : this(position, (Vector3)colour, texcoords) { }
        public Vertex2(Vector2 position, Vector3 colour, Vector2 texcoords)
        {
            this.Position = position;
            this.Colour = colour;
            this.TexCoords = texcoords;
        }
    }
}
