using System;

namespace JankWorks.Util
{
    public static class BufferExtensions
    {
        public static void WriteInt(this Buffer<char> buffer, int value)
        {
            Span<char> valueBuffer = stackalloc char[20];
            var written = CharConverter.IntToUnicode(value, valueBuffer);
            buffer.Write(valueBuffer[0..written]);         
        }

        public static void WriteLong(this Buffer<char> buffer, long value)
        {
            Span<char> valueBuffer = stackalloc char[20];
            var written = CharConverter.LongToUnicode(value, valueBuffer);
            buffer.Write(valueBuffer[0..written]);
        }

        public static void WriteUint(this Buffer<char> buffer, uint value)
        {
            Span<char> valueBuffer = stackalloc char[20];
            var written = CharConverter.UintToUnicode(value, valueBuffer);
            buffer.Write(valueBuffer[0..written]);
        }

        public static void WriteUlong(this Buffer<char> buffer, ulong value)
        {
            Span<char> valueBuffer = stackalloc char[20];
            var written = CharConverter.UlongToUnicode(value, valueBuffer);
            buffer.Write(valueBuffer[0..written]);
        }

        public static void WriteFloat(this Buffer<char> buffer, float value, int precision)
        {
            Span<char> valueBuffer = stackalloc char[20];
            var written = CharConverter.FloatToUnicode(value, valueBuffer, precision);
            buffer.Write(valueBuffer[0..written]);
        }

        public static void WriteDouble(this Buffer<char> buffer, double value, int precision)
        {
            Span<char> valueBuffer = stackalloc char[20];
            var written = CharConverter.DoubleToUnicode(value, valueBuffer, precision);
            buffer.Write(valueBuffer[0..written]);
        }
    }
}