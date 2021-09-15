using System.Numerics;

using JankWorks.Core;

namespace JankWorks.Audio
{
    public abstract class Emitter : Disposable, IPlayable
    {
        public abstract Sound Sound { get; set; }

        public abstract float Volume { get; set; }

        public abstract Vector3? Position { get; set; }

        public abstract Vector3 Direction { get; set; }

        public abstract Vector3 Velocity { get; set; }

        public abstract float MinDistance { get; set; }

        public abstract float MaxDistance { get; set; }

        public abstract float DistanceScale { get; set; }

        public abstract bool Loop { get; set; }

        public abstract PlayState State { get; }
       
        public abstract void Play();

        public abstract void Stop();

        public abstract void Pause();

        public abstract void Resume();
    }
}