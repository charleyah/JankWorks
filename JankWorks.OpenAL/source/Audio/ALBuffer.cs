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
            unsafe
            {
                fixed (byte* data = pcm)
                {
                    alBufferData(this.handle, GetFormat(channels, sampleSize), data, pcm.Length, frequency);
                }
            }

            var error = alGetError();

            if (error != ALError.NoError)
            {
                throw new AudioException($"ALBuffer.Write {error}");
            }

            ALFormat GetFormat(short channels, short sampleSize)
            {
                if (sampleSize >= 16)
                {
                    return channels > 1 ? ALFormat.Stereo16 : ALFormat.Mono16;
                }
                else
                {
                    return channels > 1 ? ALFormat.Stereo8 : ALFormat.Mono8;
                }
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