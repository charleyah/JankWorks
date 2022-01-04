using System;

using JankWorks.Audio;

using JankWorks.Drivers.OpenAL.Native;
using static JankWorks.Drivers.OpenAL.Native.Functions;

namespace JankWorks.Drivers.OpenAL.Audio
{
    struct ALBuffer
    {
        public uint handle;

        public ALBuffer(uint handle)
        {
            this.handle = handle;
        }

        public void Create()
        {
            uint handle = 0;

            unsafe
            {
                alGenBuffers(1, &handle);
            }

            var error = alGetError();

            if (error != ALError.NoError)
            {
                throw new AudioException($"ALBuffer { error }");
            }

            this.handle = handle;
        }

        public void Write(ReadOnlySpan<byte> pcm, short channels, short sampleSize, int frequency)
        {
            var format = sampleSize switch
            {
                16 when channels == 1 => ALFormat.Mono16,
                16 when channels == 2 => ALFormat.Stereo16,
                8 when channels == 1 => ALFormat.Mono8,
                8 when channels == 2 => ALFormat.Stereo8,
                _ => throw new NotSupportedException($"ALBuffer.Write does not support {sampleSize}-bit PCM with {channels} channels"),
            };

            unsafe
            {
                fixed (byte* data = pcm)
                {
                    alBufferData(this.handle, format, data, pcm.Length, frequency);
                }
            }

            var error = alGetError();

            if (error != ALError.NoError)
            {
                throw new AudioException($"ALBuffer.Write {error}");
            }
        }

        public void Dispose()
        {
            var handle = this.handle;
            unsafe
            {
                alDeleteBuffers(1, &handle);
            }
            this.handle = 0;
        }
    }
}