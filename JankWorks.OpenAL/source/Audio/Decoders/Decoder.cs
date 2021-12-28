using System;
using System.IO;

using JankWorks.Audio;

namespace JankWorks.Drivers.OpenAL.Audio.Decoders
{
    abstract class Decoder
    {
        public abstract void Load(Stream stream, ALBuffer buffer);        

        public abstract void Load(ReadOnlySpan<byte> data, ALBuffer buffer);

        public static Decoder GetDecoder(AudioFormat format)
        {
            return format switch
            {
                AudioFormat.Wav => Decoder.Wav,
                AudioFormat.OggVorbis => throw new NotSupportedException(),
                _ => throw new NotImplementedException()
            };
        }

        public static readonly Decoder Wav = new WavDecoder();
    }
}