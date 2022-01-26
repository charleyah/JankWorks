using System.Numerics;
using System.Runtime.InteropServices;

using JankWorks.Graphics;

namespace Pong.Match.Physics
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct PhysicsComponent
    {
        public Vector2 position;
        public Vector2 destination;
        public Vector2 origin;
        public Vector2 velocity;

        public Vector2 size;
        public RGBA colour;

        public Bounds GetBounds()
        {
            var tl = this.position;
            var br = this.position + this.size;
            var originOffset = this.size * this.origin;

            tl -= originOffset;
            br -= originOffset;

            return new Bounds(tl.X, tl.Y, br.X, br.Y);                 
        }

    }
}