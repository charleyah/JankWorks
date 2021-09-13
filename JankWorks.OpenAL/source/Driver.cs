using System;

using JankWorks.Core;
using JankWorks.Audio;

using JankWorks.Drivers;
using JankWorks.Drivers.Audio;

using JankWorks.Drivers.OpenAL.Audio;
using JankWorks.Drivers.OpenAL.Native;
using static JankWorks.Drivers.OpenAL.Native.Functions;

[assembly: JankWorksDriver(typeof(JankWorks.Drivers.OpenAL.Driver))]

namespace JankWorks.Drivers.OpenAL
{
    public sealed class Driver : Disposable, IAudioDriver
    {
        public Driver()
        {
            Functions.Init();
        }

        public AudioDevice GetDefaultAudioDevice()
        {
            var device = alcOpenDevice(default);

            if(device == IntPtr.Zero)
            {
                var error = alGetError();
                throw new AudioException($"GetDefaultAudioDevice { error }");
            }

            try
            {
                return new ALAudioDevice(device);
            }
            catch
            {
                alcCloseDevice(device);
                throw;
            }            
        }

        protected override void Dispose(bool finalising)
        {
            Functions.loader.Dispose();
            base.Dispose(finalising);
        }
    }
}