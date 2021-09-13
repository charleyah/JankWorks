using System;
using System.IO;
using System.Numerics;

using JankWorks.Audio;

using JankWorks.Drivers.OpenAL.Native;
using static JankWorks.Drivers.OpenAL.Native.Functions;

namespace JankWorks.Drivers.OpenAL.Audio
{
    sealed class ALAudioDevice : AudioDevice
    {
        private IntPtr device;
        private IntPtr context;

        public ALAudioDevice(IntPtr device)
        {
            this.device = device;
            unsafe
            {
                var context = alcCreateContext(device, (int*)0);
                if(context == IntPtr.Zero)
                {
                    var error = alGetError();
                    throw new AudioException($"ALAudioDevice { error }");
                }
                else
                {
                    this.context = context;
                }
            }

            this.Volume = 1f;
            this.Position = new Vector3(0);
            this.Velocity = new Vector3(0);
            this.Orientation = new Orientation()
            {
                Direction = Vector3.UnitZ,
                Up = Vector3.UnitY
            };

            alcMakeContextCurrent(this.context);
        }

        public override float Volume 
        { 
            get => base.Volume;
            set
            {
                alListenerf(ALListenerf.Gain, value);
                base.Volume = value;
            }
        }

        public override Vector3 Position 
        {
            get => base.Position; 
            set
            {
                alListener3f(ALListener3f.Position, value);
                base.Position = value;
            }
        }

        public override Vector3 Velocity 
        { 
            get => base.Velocity; 
            set
            {
                alListener3f(ALListener3f.Velocity, value);
                base.Velocity = value;
            }
        }

        public override Orientation Orientation 
        { 
            get => base.Orientation; 
            set
            {                
                unsafe
                {
                    alListenerfv(ALListenerfv.Orientation, (float*)&value);
                }
                
                base.Orientation = value;
            }
        }

        public override Emitter CreateEmitter(Sound sound) => new ALEmitter(sound);

        public override Sound CreateSound(ReadOnlySpan<byte> pcm, short channels, short samples, int frequency)
        {
            var sound = new ALSound();
            sound.Write(pcm, channels, samples, frequency);
            return sound;
        }

        public override Music LoadMusic(Stream stream, AudioFormat format)
        {
            throw new NotImplementedException();
        }

        public override Sound LoadSound(Stream stream, AudioFormat format)
        {
            throw new NotImplementedException();
        }



        protected override void Dispose(bool finalising)
        {
            alcDestroyContext(this.context);
            alcCloseDevice(this.device);

            base.Dispose(finalising);
        }
    }
}