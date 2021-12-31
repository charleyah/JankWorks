using System.IO;
using System.Numerics;

using JankWorks.Core;

namespace JankWorks.Audio
{
    public abstract class Music : Disposable, IPlayable
    {
        public abstract float Volume { get; set; }

        public abstract Vector3? Position { get; set; }

        public abstract Vector3 Direction { get; set; }

        public abstract Vector3 Velocity { get; set; }

        public abstract float MinDistance { get; set; }

        public abstract float MaxDistance { get; set; }

        public abstract float DistanceScale { get; set; }

        public abstract PlayState State { get; }

        public abstract void Pause();

        public abstract void Play();

        public abstract void Resume();

        public abstract void Stop();

        public abstract bool Stream();

        public abstract void ChangeStream(Stream stream, AudioFormat format);
    }
}