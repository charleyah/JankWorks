using System;
using System.IO;

using NVorbis;

namespace JankWorks.Drivers.OpenAL.Audio.Decoders
{
    sealed class OggVorbisDecoder : Decoder
    {
        public override void Load(Stream stream, ALBuffer buffer)
        {
            using var reader = new VorbisReader(stream, false);

            var channels = reader.Channels;
            var sampleRate = reader.SampleRate;            
            var samples = new float[reader.TotalSamples];
            var totalSamples = reader.ReadSamples(samples);

            var samplesIn16Bit = new short[totalSamples];

            for (int i = 0; i < totalSamples; i++)
            {
                var temp = (int)(32767f * samples[i]);
                temp = Math.Min(temp, short.MaxValue);
                temp = Math.Max(temp, short.MinValue);
                samplesIn16Bit[i] = (short)temp;
            }

            unsafe
            {                
                fixed (short* samplesPtr = samplesIn16Bit)
                {                                        
                    checked
                    {
                        var sampleData = new ReadOnlySpan<byte>(samplesPtr, samples.Length * sizeof(ushort));
                        buffer.Write(sampleData, (short)channels, 16, sampleRate);
                    }                    
                }                
            }            
        }

        public override void Load(ReadOnlySpan<byte> data, ALBuffer buffer)
        {
            unsafe
            {
                fixed(byte* dataPtr = data)
                {
                    var stream = new UnmanagedMemoryStream(dataPtr, data.Length);
                    this.Load(stream, buffer);
                }
            }           
        }
    }
}