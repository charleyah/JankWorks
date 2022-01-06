using System;
using System.Runtime.InteropServices;
using System.IO;

using JankWorks.Audio;

namespace JankWorks.Drivers.OpenAL.Audio.Decoders
{
    sealed class WavDecoder : Decoder
    {
        public override long TotalSamples => this.totalSamples;

        public override int SampleRate => this.sampleRate;

        public override int SampleSize => this.sampleSize;

        public override int Channels => this.channels;

        public override bool EndOfStream => this.stream.Position >= this.stream.Length;

        public override AudioFormat Format => AudioFormat.Wav;

        private Stream stream;
        private long totalSamples;
        private int sampleRate;
        private int sampleSize;
        private int channels;

        private byte[] sampleBuffer;

        public WavDecoder(Stream stream, int sampleBufferSize) : base(sampleBufferSize)
        {
            this.sampleBuffer = new byte[sampleBufferSize + (sampleBufferSize % sizeof(short))];
            this.ChangeStream(stream);

        }

        public override void ChangeStream(Stream stream)
        {
            if(!stream.CanRead)
            {
                throw new ArgumentException("WavDecoder stream can not be read");
            }
            
            var header = WavHeader.Read(stream);

            if(header.AudioFormat != 1)
            {
                throw new ArgumentException("WavDecoder can not decode compressed wave");
            }

            var sampleSizeInBytes = header.BitsPerSample / 8;

            this.totalSamples = header.SubChunk2Size / sampleSizeInBytes;
            this.sampleRate = (int)header.SampleRate;
            this.sampleSize = header.BitsPerSample;
            this.channels = header.NumChannels;

            if(this.sampleBuffer.Length % sampleSizeInBytes != 0)
            {
                var bufferSize = this.SampleBufferSize;
                Array.Resize(ref this.sampleBuffer, bufferSize + (bufferSize % sampleSizeInBytes));
            }

            this.stream?.Dispose();
            this.stream = stream;
        }

        public override void Reset()
        {
            if(this.stream.CanSeek)
            {
                this.stream.Seek(Marshal.SizeOf<WavHeader>(), SeekOrigin.Begin);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public override bool Decode(ALBuffer buffer)
        {
            if(this.EndOfStream)
            {
                return false;
            }

            var read = this.stream.Read(this.sampleBuffer);

            buffer.Write(sampleBuffer.AsSpan().Slice(0, read), (short)this.channels, (short)this.sampleSize, this.sampleRate);

            return true;
        }

        public override void Load(ALBuffer buffer)
        {
            if (stream is UnmanagedMemoryStream ums)
            {
                ReadOnlySpan<byte> umsData;
                unsafe
                {
                    umsData = new ReadOnlySpan<byte>(ums.PositionPointer, (int)ums.Length);
                }
                buffer.Write(umsData, (short)this.channels, (short)this.sampleSize, this.sampleRate);
            }
            else
            {
                var sampleData = new byte[this.totalSamples * (this.sampleSize / 8)];

                stream.Write(sampleData);

                buffer.Write(sampleData, (short)this.channels, (short)this.sampleSize, this.sampleRate);
            }
        }

        protected override void Dispose(bool disposing)
        {
            this.stream.Dispose();
            base.Dispose(disposing);
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
                }

                WavHeader.VerifyHeader(in header);

                return header;
            }

            public static WavHeader Read(ref ReadOnlySpan<byte> data)
            {
                var header = default(WavHeader);
                var headerSize = sizeof(WavHeader);

                unsafe
                {
                    var hspan = new Span<byte>(&header, headerSize);
                    data.Slice(0, headerSize).CopyTo(hspan);
                }

                WavHeader.VerifyHeader(in header);
                data = data.Slice(headerSize);
                return header;
            }

            private unsafe static void VerifyHeader(in WavHeader header)
            {
                uint expected = default;

                byte* expectedPtr = (byte*)&expected;

                // RIFF in Ascii
                expectedPtr[0] = 82;
                expectedPtr[1] = 73;
                expectedPtr[2] = 70;
                expectedPtr[3] = 70;

                fixed (byte* idPtr = header.RiffHeader.ChunkId)
                {
                    if(*(uint*)idPtr != expected)
                    {
                        throw new InvalidDataException("Invalid RIFF header");
                    }
                }

                // WAVE in Ascii
                expectedPtr[0] = 87;
                expectedPtr[1] = 65;
                expectedPtr[2] = 86;
                expectedPtr[3] = 69;

                fixed (byte* formatPtr = header.RiffHeader.Format)
                {
                    if (*(uint*)formatPtr != expected)
                    {
                        throw new InvalidDataException("Invalid RIFF format");
                    }
                }
            }
        }
    }
}
