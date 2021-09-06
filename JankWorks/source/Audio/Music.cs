using System.IO;

namespace JankWorks.Audio
{
    public abstract class Music : Sound, IPlayable
    {
        public virtual float Volume { get; set; }

        public virtual bool Loop { get; set; }

        public abstract PlayState State { get; }

        public abstract void Play();

        public abstract void Stop();

        public abstract void Pause();

        public abstract void ChangeTrack(Stream stream, AudioFormat format);
    }
}