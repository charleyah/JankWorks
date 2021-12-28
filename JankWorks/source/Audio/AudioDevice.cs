using System;
using System.IO;

using JankWorks.Drivers;

namespace JankWorks.Audio
{
    public abstract class AudioDevice : Listener
    {        
        public static AudioDevice GetDefault() => DriverConfiguration.Drivers.audioApi.GetDefaultAudioDevice();

        public abstract Sound LoadSound(Stream stream, AudioFormat format);

        public abstract Sound LoadSound(ReadOnlySpan<byte> data, AudioFormat format);

        public virtual Sound CreateSound(Stream pcmStream, short channels, short samples, int frequency)
        {
            if(pcmStream is UnmanagedMemoryStream ums)
            {
                ReadOnlySpan<byte> pcmdata;
                unsafe
                {
                    pcmdata = new ReadOnlySpan<byte>(ums.PositionPointer, (int)ums.Length);
                }

                return this.CreateSound(pcmdata, channels, samples, frequency);
            }
            else
            {
                MemoryStream ms;

                if(pcmStream is MemoryStream pcmms)
                {
                    ms = pcmms;
                }
                else
                {
                    ms = new MemoryStream((int)pcmStream.Length);
                    pcmStream.CopyTo(ms);
                }

                return this.CreateSound(ms.GetBuffer(), channels, samples, frequency);
            }
        }

        public abstract Sound CreateSound(ReadOnlySpan<byte> pcm, short channels, short samples, int frequency);
        
        public abstract Emitter CreateEmitter(Sound sound);
    }

    public enum AudioFormat
    {
        Wav,
        OggVorbis
    }
}