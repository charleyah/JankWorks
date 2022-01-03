using System.IO;

using JankWorks.Core;

namespace JankWorks.Audio
{
    public abstract class Music : Disposable, IPlayable
    {
        public abstract float Volume { get; set; }

        public abstract PlayState State { get; }

        public abstract void Pause();

        public abstract void Play();

        public abstract void Resume();

        public abstract void Stop();

        public abstract bool Stream();

        public abstract void ChangeStream(Stream stream, AudioFormat format);
    }
}