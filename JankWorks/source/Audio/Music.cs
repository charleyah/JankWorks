using System.IO;

namespace JankWorks.Audio
{
    public abstract class Music : Sound, IPlayable
    {
        public float Volume { get; set; }

        public bool Loop { get; set; }

        public PlayState State { get; protected set; }

        public abstract void Play();

        public abstract void Stop();

        public abstract void Pause();

        public abstract void ChangeTrack(Stream stream, AudioFormat format);
    }
}