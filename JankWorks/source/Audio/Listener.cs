using System.Numerics;

using JankWorks.Core;

namespace JankWorks.Audio
{
    public abstract class Listener : Disposable
    {
        public virtual float Volume { get; set; }

        public virtual Vector3 Position { get; set; }

        public virtual Vector3 Velocity { get; set; }

        public virtual Orientation Orientation { get; set; }        
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
    }
}