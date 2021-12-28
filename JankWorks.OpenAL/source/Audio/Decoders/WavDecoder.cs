using System;
using System.Runtime.InteropServices;
using System.IO;

namespace JankWorks.Drivers.OpenAL.Audio.Decoders
{
    sealed class WavDecoder : Decoder
    {
        public override void Load(Stream stream, ALBuffer buffer)
        {
            var header = WavHeader.Read(stream);
           
            if (header.AudioFormat != 1)
            {
                throw new NotSupportedException("WavDecoder only supports 16-bit pcm format");
            }

            int dataLength = (int)header.SubChunk2Size;

            if (stream is UnmanagedMemoryStream ums)
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

                if (stream is MemoryStream sms)
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

        public override void Load(ReadOnlySpan<byte> data, ALBuffer buffer)
        {
            var header = WavHeader.Read(ref data);

            if (header.AudioFormat != 1)
            {
                throw new NotSupportedException("WavDecoder only supports 16-bit pcm format");
            }
            
            buffer.Write(data, (short)header.NumChannels, (short)header.BitsPerSample, (int)header.SampleRate);
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
