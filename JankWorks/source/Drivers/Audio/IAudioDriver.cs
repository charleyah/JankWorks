using System;

using JankWorks.Audio;

namespace JankWorks.Drivers.Audio
{
    public interface IAudioDriver : IDriver
    {
        AudioDevice GetDefaultAudioDevice();
    }
}
