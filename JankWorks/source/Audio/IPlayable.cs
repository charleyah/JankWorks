using System;

namespace JankWorks.Audio
{
    public interface IPlayable : IDisposable
    {
        PlayState State { get; }

        void Play();

        void Stop();

        void Pause();

        void Resume();
    }

    public enum PlayState
    {
        Stopped = 0,
        Playing,
        Paused
    }
}