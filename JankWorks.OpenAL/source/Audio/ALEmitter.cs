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

        public override bool Loop 
        { 
            get
            {
                unsafe
                {
                    int value = default;
                    alGetSourcei(this.handle, ALGetSourcei.Looping, &value);
                    return value == 1;
                }
                
            }
            set
            {
                alSourcei(this.handle, ALSourcei.Looping, value ? 1 : 0);                
            }
        }

        public override Vector3? Position
        {
            get => this.position;
            set
            {
                if(value is null)
                {
                    alSourcei(this.handle, ALSourcei.SourceRelative, 1);
                    alSource3f(this.handle, ALSource3f.Position, 0, 0, 0);
                }
                else if(value is Vector3 pos)
                {
                    alSourcei(this.handle, ALSourcei.SourceRelative, 0);
                    alSource3f(this.handle, ALSource3f.Position, pos.X, pos.Y, pos.Z);
                }
                this.position = value;
            }
        }

        public override Vector3 Direction 
        { 
            get
            {
                unsafe
                {
                    Vector3 value = default;
                    alGetSource3f(this.handle, ALSource3f.Direction, &value.X, &value.Y, &value.Z);
                    return value;
                }                
            }
            set
            {
                alSource3f(this.handle, ALSource3f.Direction, value.X, value.Y, value.Z);                
            }
        }

        public override Vector3 Velocity 
        {
            get
            {
                unsafe
                {
                    Vector3 value = default;
                    alGetSource3f(this.handle, ALSource3f.Velocity, &value.X, &value.Y, &value.Z);
                    return value;
                }                
            }
            set
            {
                alSource3f(this.handle, ALSource3f.Velocity, value.X, value.Y, value.Z);                
            }
        }

        public override float MinDistance
        { 
            get
            {
                unsafe
                {
                    float value = default;
                    alGetSourcef(this.handle, ALSourcef.ReferenceDistance, &value);
                    return value;
                }                
            }
            set
            {
                alSourcef(this.handle, ALSourcef.ReferenceDistance, value);                
            }
        }

        public override float MaxDistance
        {
            get
            {
                unsafe
                {
                    float value = default;
                    alGetSourcef(this.handle, ALSourcef.MaxDistance, &value);
                    return value;
                }                
            }
            set
            {
                alSourcef(this.handle, ALSourcef.MaxDistance, value);                
            }
        }

        public override float DistanceScale
        { 
            get
            {
                unsafe
                {
                    float value = default;
                    alGetSourcef(this.handle, ALSourcef.RolloffFactor, &value);
                    return value;
                }                
            }
            set
            {
                alSourcef(this.handle, ALSourcef.RolloffFactor, value);                
            }
        }

        public override Sound Sound 
        { 
            get => this.sound; 
            set
            {
                int soundHandle = 0;

                if(value is ALSound alsound)
                {
                    soundHandle = (int)alsound.buffer.handle;
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

                this.sound = value;
            }
        }
        private readonly uint handle;
        private Vector3? position;
        private Sound sound;
        
        public ALEmitter(Sound sound) : this(sound, null) { }

        public ALEmitter(Sound sound, Vector3? position)
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
            this.Position = position;
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