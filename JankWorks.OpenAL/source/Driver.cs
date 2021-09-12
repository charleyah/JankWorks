using System;
using JankWorks.Audio;

using JankWorks.Drivers;
using JankWorks.Drivers.Audio;

[assembly: JankWorksDriver(typeof(JankWorks.Drivers.OpenAL.Driver))]

namespace JankWorks.Drivers.OpenAL
{
    public sealed class Driver : IAudioDriver
    {
        public AudioDevice GetDefaultAudioDevice()
        {
            throw new NotImplementedException();
        }
    }
}
