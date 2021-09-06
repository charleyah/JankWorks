using System.Numerics;

using JankWorks.Core;

namespace JankWorks.Audio
{
    public abstract class Listener : Disposable
    {
        public virtual float Volume { get; set; }

        public virtual Vector3 Position { get; set; }

        public virtual Vector3 Velocity { get; set; }

        public virtual Quaternion Orientation { get; set; }        
    }
}