using JankWorks.Drivers;
using System.IO;

namespace JankWorks.Audio
{
    public abstract class AudioDevice : Listener
    {
        public static AudioDevice GetDefault() => DriverConfiguration.Drivers.audioApi.GetDefaultAudioDevice();

        public abstract Sound LoadSound(Stream stream, AudioFormat format);

        public abstract Music LoadMusic(Stream stream, AudioFormat format);
    }


    public enum AudioFormat
    {
        Wav,
        Ogg,
        Mp3
    }
}