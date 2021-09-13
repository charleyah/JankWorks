using System;
using System.Numerics;

using JankWorks.Audio;

using JankWorks.Drivers.OpenAL.Native;
using static JankWorks.Drivers.OpenAL.Native.Functions;

namespace JankWorks.Drivers.OpenAL.Audio
{
    sealed class ALEmitter : Emitter
    {
        public override float Volume 
        { 
            get => base.Volume;
            set
            {
                alSourcef(this.handle, ALSourcef.Gain, value);
                base.Volume = value;
            }
        }

        public override bool Loop 
        { 
            get => base.Loop; 
            set
            {
                alSourcei(this.handle, ALSourcei.Looping, value ? 1 : 0);
                base.Loop = value;
            }
        }

        public override Vector3? Position
        {
            get => base.Position;
            set
            {
                var oldval = base.Position;

                if(value is Vector3 pos)
                {
                    if (oldval is null) 
                    { 
                        alSourcei(this.handle, ALSourcei.SourceRelative, 0); 
                    }

                    alSource3f(this.handle, ALSource3f.Position, pos);
                }
                else
                {
                    alSourcei(this.handle, ALSourcei.SourceRelative, 1);

                    var zero = new Vector3(0);
                    alSource3f(this.handle, ALSource3f.Position, zero);
                    alSource3f(this.handle, ALSource3f.Velocity, zero);
                    base.Velocity = zero;
                }
            
                base.Position = value;
            }
        }

        public override Vector3 Velocity 
        {
            get => base.Velocity;
            set
            {
                alSource3f(this.handle, ALSource3f.Velocity, value);
                base.Velocity = value;
            }
        }

        public override Sound Sound 
        { 
            get => base.Sound; 
            set
            {
                int soundHandle = 0;

                if(value is ALSound alsound)
                {
                    soundHandle = (int)alsound.buffer.handle;
                }
                else if (value is ALMusic almusic)
                {
                    soundHandle = (int)almusic.buffer.handle;
                }
                else
                {
                    throw new NotSupportedException();
                }

                if (this.State != PlayState.Stopped)
                {
                    this.Stop();
                }

                alSourcei(this.handle, ALSourcei.Buffer, soundHandle);

                var error = alGetError();
                if(error != ALError.NoError)
                {
                    throw new AudioException($"ALEmitter Sound {error}");
                }

                base.Sound = value;
            }
        }

        private readonly uint handle;

        public ALEmitter(Sound sound) : this(sound, 1f, false, null, new Vector3(0)) { }

        public ALEmitter(Sound sound, float volume, bool loop, Vector3? position, Vector3 velocity)
        {
            uint handle = 0;

            unsafe
            {
                alGenSources(1, &handle);
            }

            var error = alGetError();

            if(error != ALError.NoError)
            {
                throw new AudioException($"ALEmitter { error }");
            }

            this.handle = handle;
            this.Sound = sound;

            this.Volume = volume;
            this.Loop = loop;
            this.Position = position;
            this.Velocity = velocity;
        }

        public override PlayState State
        {
            get
            {
                int state = (int)ALSourceState.Initial;

                alGetSourcei(this.handle, ALGetSourcei.SourceState, ref state);

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

        public override void Play()
        {
            if(this.State != PlayState.Playing)
            {
                alSourceRewind(this.handle);
                alSourcePlay(this.handle);
            }            
        }

        public override void Stop() => alSourceStop(this.handle);
                   
        public override void Pause() => alSourcePause(this.handle);

        public override void Resume() => alSourcePlay(this.handle);

        protected override void Dispose(bool finalising)
        {
            this.Stop();

            var handle = this.handle;

            unsafe
            {
                alDeleteSources(1, &handle);
            }
            base.Dispose(finalising);
        }
    }
}