using System.Numerics;

using JankWorks.Core;

namespace JankWorks.Audio
{
    public abstract class Listener : Disposable
    {
        public abstract float Volume { get; set; }

        public abstract Vector3 Position { get; set; }

        public abstract Vector3 Velocity { get; set; }

        public abstract Orientation Orientation { get; set; }        
    }

    public struct Orientation
    {
        public Vector3 Direction;
        public Vector3 Up;

        public Orientation(Vector3 direction, Vector3 up)
        {
            this.Direction = direction;
            this.Up = up;
        }

        public static Orientation Ortho => new Orientation(Vector3.UnitZ, -Vector3.UnitY);
    }
}