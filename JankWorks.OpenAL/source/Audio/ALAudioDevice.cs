using System;
using System.Runtime.InteropServices;
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

            alcMakeContextCurrent(this.context);
            alDistanceModel(ALDistanceModel.LinearDistanceClamped);

            this.Volume = 1f;
            this.Position = Vector3.Zero;
            this.Velocity = Vector3.Zero;
            this.Orientation = new Orientation()
            {
                Direction = Vector3.UnitZ,
                Up = Vector3.UnitY
            };
        }

        public override float Volume 
        { 
            get
            {
                unsafe
                {
                    float value = default;
                    alGetListenerf(ALListenerf.Gain, &value);
                    return value;
                }
                
            }
            set
            {
                alListenerf(ALListenerf.Gain, value);
            }
        }

        public override Vector3 Position 
        {
            get
            {                
                unsafe
                {
                    Vector3 value = default;
                    alGetListener3f(ALListener3f.Position, &value.X, &value.Y, &value.Z);
                    return value;                        
                }
            }
            set
            {
                alListener3f(ALListener3f.Position, value.X, value.Y, value.Z);
            }
        }

        public override Vector3 Velocity 
        { 
            get
            {
                unsafe
                {
                    Vector3 value = default;
                    alGetListener3f(ALListener3f.Velocity, &value.X, &value.Y, &value.Z);
                    return value;
                }
            }
            set
            {
                alListener3f(ALListener3f.Velocity, value.X, value.Y, value.Z);                
            }
        }

        public override Orientation Orientation 
        { 
            get
            {
                unsafe
                {
                    Orientation value = default;
                    alGetListenerfv(ALListenerfv.Orientation, (float*)&value);
                    return value;
                }
            }
            set
            {                
                unsafe
                {
                    alListenerfv(ALListenerfv.Orientation, (float*)&value);
                }
            }
        }

        public override Emitter CreateEmitter(Sound sound) => new ALEmitter(sound);

        public override Sound CreateSound(ReadOnlySpan<byte> pcm, short channels, short samples, int frequency)
        {
            var sound = new ALSound();
            sound.Write(pcm, channels, samples, frequency);
            return sound;
        }

        public override Sound LoadSound(Stream stream, AudioFormat format)
        {
            if(format == AudioFormat.Wav)
            {
                var sound = new ALSound();
                ALAudioDevice.LoadWav(stream, sound.buffer);
                return sound;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        internal static void LoadWav(Stream stream, ALBuffer buffer)
        {
            var header = WavHeader.Read(stream);

            if (header.AudioFormat != 1)
            {
                throw new NotSupportedException("ALAudioDevice only supports 16-bit pcm wav format");
            }

            int dataLength = (int)header.SubChunk2Size;

            if(stream is UnmanagedMemoryStream ums)
            {
                ReadOnlySpan<byte> umsData;
                unsafe
                {
                    umsData = new ReadOnlySpan<byte>(ums.PositionPointer, dataLength);
                }
                buffer.Write(umsData, (short)header.NumChannels, (short)header.BitsPerSample, (int)header.SampleRate);
            }
            else
            {
                MemoryStream ms;

                if(stream is MemoryStream sms)
                {
                    ms = sms;
                }
                else
                {
                    ms = new MemoryStream(dataLength);
                    stream.Read(ms.GetBuffer(), 0, dataLength);
                }

                buffer.Write(ms.GetBuffer(), (short)header.NumChannels, (short)header.BitsPerSample, (int)header.SampleRate);
            }            
        }

        protected override void Dispose(bool finalising)
        {
            alcDestroyContext(this.context);
            alcCloseDevice(this.device);

            base.Dispose(finalising);
        }
    }



    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct RiffHeader
    {
        public const int IDSize = 4;

        public fixed byte ChunkId[RiffHeader.IDSize];
        public uint ChunkSize;
        public fixed byte Format[RiffHeader.IDSize];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct WavHeader
    {
        public RiffHeader RiffHeader;
        public fixed byte SubChunk1ID[RiffHeader.IDSize];
        public uint SubChunk1Size;
        public ushort AudioFormat;
        public ushort NumChannels;
        public uint SampleRate;
        public uint ByteRate;
        public ushort BlockAlign;
        public ushort BitsPerSample;
        public fixed byte SubChunk2ID[RiffHeader.IDSize];
        public uint SubChunk2Size;

        public static WavHeader Read(Stream stream)
        {
            var header = default(WavHeader);

            unsafe
            {
                var hspan = new Span<byte>(&header, sizeof(WavHeader));
                stream.Read(hspan);

                Span<byte> expectedId = stackalloc byte[RiffHeader.IDSize];

                Span<byte> actualId = new Span<byte>(header.RiffHeader.ChunkId, RiffHeader.IDSize);
                WriteRiff(expectedId);
                if (!expectedId.SequenceEqual(actualId))
                {
                    throw new InvalidDataException("Invalid RIFF header");
                }

                actualId = new Span<byte>(header.RiffHeader.Format, RiffHeader.IDSize);
                WriteWave(expectedId);
                if (!expectedId.SequenceEqual(actualId))
                {
                    throw new InvalidDataException("Invalid WAVE header");
                }
            }

            return header;

            void WriteRiff(Span<byte> data)
            {
                // RIFF in Ascii
                data[0] = 82;
                data[1] = 73;
                data[2] = 70;
                data[3] = 70;
            }

            void WriteWave(Span<byte> data)
            {
                // WAVE in Ascii
                data[0] = 87;
                data[1] = 65;
                data[2] = 86;
                data[3] = 69;
            }
        }
    }
}