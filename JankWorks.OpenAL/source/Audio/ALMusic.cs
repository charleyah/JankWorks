using System;
using System.IO;

using JankWorks.Audio;
using JankWorks.Drivers.OpenAL.Audio.Decoders;
using JankWorks.Drivers.OpenAL.Native;

using static JankWorks.Drivers.OpenAL.Native.Functions;

namespace JankWorks.Drivers.OpenAL.Audio
{
    sealed class ALMusic : Music
    {
        public override float Volume
        {
            get
            {
                unsafe
                {
                    float value = default;
                    alGetSourcef(this.handle, ALSourcef.Gain, &value);
                    return value;
                }

            }
            set
            {
                alSourcef(this.handle, ALSourcef.Gain, value);
            }
        }

        public override PlayState State
        {
            get
            {
                int state = (int)ALSourceState.Initial;

                unsafe
                {
                    alGetSourcei(this.handle, ALGetSourcei.SourceState, &state);
                }

                return (ALSourceState)state switch
                {
                    ALSourceState.Initial => PlayState.Stopped,
                    ALSourceState.Stopped => PlayState.Stopped,
                    ALSourceState.Playing => PlayState.Playing,
                    ALSourceState.Paused => PlayState.Paused,
                    _ => throw new NotImplementedException()
                };
            }
        }

        private readonly uint handle;
        private ALBuffer bufferOne, bufferTwo;
        private Decoder decoder;

        public ALMusic(Stream stream, AudioFormat format)
        {
            uint handle = 0;

            unsafe
            {
                alGenSources(1, &handle);
            }

            var error = alGetError();

            if (error != ALError.NoError)
            {
                throw new AudioException($"ALMusic { error }");
            }

            this.handle = handle;

            this.bufferOne.Create();
            this.bufferTwo.Create();

            this.decoder = Decoder.Create(stream, format);
        }

        public override void ChangeStream(Stream stream, AudioFormat format)
        {
            if (!stream.CanSeek || !stream.CanRead)
            {
                throw new ArgumentException("ALMusic.ChangeStream stream must be seekable and readable");
            }
            
            this.Stop();

            if (this.decoder.Format == format)
            {
                this.decoder.ChangeStream(stream);
            }
            else
            {
                this.decoder.Dispose();
                this.decoder = Decoder.Create(stream, format);
            }
        }

        public override void Play()
        {
            if (this.State != PlayState.Playing)
            {
                Span<uint> buffers = stackalloc uint[2];
                
                var decoded = this.decoder.Decode(this.bufferOne);
                var queueCount = 0;

                if(decoded)
                {
                    buffers[0] = this.bufferOne.handle;
                    queueCount++;
                }

                decoded = this.decoder.Decode(this.bufferTwo);

                if (decoded)
                {
                    buffers[1] = this.bufferTwo.handle;
                    queueCount++;
                }

                unsafe
                {
                    fixed(uint* buffersptr = buffers)
                    {
                        alSourceQueueBuffers(this.handle, queueCount, buffersptr);
                    }
                }
                
                alSourcePlay(this.handle);
            }
        }

        public override void Stop()
        {
            alSourceStop(this.handle);
            alSourcei(this.handle, ALSourcei.Buffer, 0);
            this.decoder.Reset();
        }

        public override void Pause() => alSourcePause(this.handle);

        public override void Resume() => alSourcePlay(this.handle);

        public override bool Stream()
        {
            if(this.decoder.EndOfStream)
            {
                return false;
            }

            var processed = 0;

            unsafe
            {
                alGetSourcei(this.handle, ALGetSourcei.BuffersProcessed, &processed);

                while (processed > 0)
                {
                    ALBuffer buffer = default;
                    alSourceUnqueueBuffers(this.handle, 1, &buffer.handle);
                    
                    if (this.decoder.Decode(buffer))
                    {
                        alSourceQueueBuffers(this.handle, 1, &buffer.handle);
                    }
                    else
                    {
                        return false;
                    }

                    processed--;
                }
            }

            return true;                  
        }

        protected override void Dispose(bool disposing)
        {
            this.Stop();

            this.bufferOne.Dispose();
            this.bufferTwo.Dispose();

            unsafe
            {
                var soundHandle = this.handle;
                alDeleteSources(1, &soundHandle);
            }

            this.decoder.Dispose();
            
            base.Dispose(disposing);
        }
    }
}