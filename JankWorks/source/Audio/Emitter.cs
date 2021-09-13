using System.Numerics;

using JankWorks.Core;

namespace JankWorks.Audio
{
    public abstract class Emitter : Disposable, IPlayable
    {
        public virtual Sound Sound { get; set; }

        public virtual float Volume { get; set; }

        public virtual Vector3? Position { get; set; }

        public virtual Vector3 Velocity { get; set; }

        public virtual bool Loop { get; set; }

        public abstract PlayState State { get; }
       
        public abstract void Play();

        public abstract void Stop();

        public abstract void Pause();

        public abstract void Resume();
    }
}