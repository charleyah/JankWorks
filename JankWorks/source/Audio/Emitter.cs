using System.Numerics;

using JankWorks.Core;

namespace JankWorks.Audio
{
    public abstract class Emitter : Disposable, IPlayable
    {
        public virtual Sound Sound { get; set; }

        public virtual float Volume { get; set; }

        public virtual Vector3 Position { get; set; }

        public abstract PlayState State { get; }

        protected Emitter(Sound sound, float volume, Vector3 position)
        {
            this.Sound = sound;
            this.Volume = volume;
            this.Position = position;           
        }
       
        public abstract void Play();

        public abstract void Stop();

        public abstract void Pause();
    }
}