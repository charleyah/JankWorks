using System;
using System.IO;

using NVorbis;

using JankWorks.Audio;

namespace JankWorks.Drivers.OpenAL.Audio.Decoders
{
    sealed class OggVorbisDecoder : Decoder
    {
        private Stream stream;
        private VorbisReader reader;

        public override long TotalSamples => this.reader.TotalSamples;

        public override int SampleRate => this.reader.SampleRate;

        public override int SampleSize => 16;

        public override int Channels => this.reader.Channels;

        public override bool EndOfStream => this.reader.IsEndOfStream;

        public override AudioFormat Format => AudioFormat.OggVorbis;

        private short[] sampleBuffer;

        private const int ReadSampleCount = 128;

        public OggVorbisDecoder(Stream stream, int sampleBufferSize) : base(sampleBufferSize) 
        {
            if (!stream.CanRead)
            {
                throw new ArgumentException("OggVorbisDecoder stream can not be read");
            }

            this.stream = stream;
            this.reader = new VorbisReader(stream, false);

            var bufferSize = this.SampleBufferSize / sizeof(short);
            bufferSize += bufferSize % ReadSampleCount;
            this.sampleBuffer = new short[bufferSize];
        }

        public override void Reset()
        {
            this.reader.SeekTo(0);
        }

        public override void ChangeStream(Stream stream)
        {
            this.reader.Dispose();
            this.stream.Dispose();

            this.stream = stream;
            this.reader = new VorbisReader(stream, false);
        }

        public override void Load(ALBuffer buffer)
        {
            var channels = this.reader.Channels;
            var sampleRate = this.reader.SampleRate;
            var samples = new float[this.reader.TotalSamples];
            var totalSamples = this.reader.ReadSamples(samples);
            
            var samplesIn16Bit = new short[totalSamples];

            for (int i = 0; i < totalSamples; i++)
            {
                var temp = (int)(short.MaxValue * samples[i]);

                if (temp > short.MaxValue)
                {
                    temp = short.MaxValue;
                }
                else if (temp < short.MinValue)
                {
                    temp = short.MinValue;
                }

                samplesIn16Bit[i] = (short)temp;
            }

            unsafe
            {
                fixed (short* samplesPtr = samplesIn16Bit)
                {
                    checked
                    {
                        var sampleData = new ReadOnlySpan<byte>(samplesPtr, samples.Length * sizeof(ushort));
                        buffer.Write(sampleData, (short)channels, (short)this.SampleSize, sampleRate);
                    }
                }
            }
        }

        public override bool Decode(ALBuffer buffer)
        {
            if(this.EndOfStream)
            {
                return false;
            }

            var samplesWritten = 0;

            Span<float> readBuffer = stackalloc float[ReadSampleCount];

            while(samplesWritten < this.sampleBuffer.Length && !this.EndOfStream)
            {
                var read = this.reader.ReadSamples(readBuffer);
                var readSamples = readBuffer.Slice(0, read);

                for(int readIndex = 0; readIndex < readSamples.Length; readIndex++)
                {
                    var temp = (int)(short.MaxValue * readSamples[readIndex]);

                    if (temp > short.MaxValue)
                    {
                        temp = short.MaxValue;
                    }
                    else if (temp < short.MinValue)
                    {
                        temp = short.MinValue;
                    }
                    this.sampleBuffer[samplesWritten++] = (short)temp;
                }
            }

            unsafe
            {
                fixed(short* bufferedPtr = this.sampleBuffer)
                {
                    var buffered = new Span<byte>(bufferedPtr, sizeof(short) * samplesWritten);

                    buffer.Write(buffered, (short)this.Channels, (short)this.SampleSize, this.SampleRate);
                }                
            }

            return true;
        }

        protected override void Dispose(bool finalising)
        {
            this.reader.Dispose();
            this.stream.Dispose();
            base.Dispose(finalising);
        }
    }
}