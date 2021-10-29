using System;

namespace JankWorks.Util
{
    public static class BufferExtensions
    {
        public static void WriteInt(this IWriteBuffer<char> buffer, int value)
        {
            Span<char> valueBuffer = stackalloc char[20];
            var written = CharConverter.IntToUnicode(value, valueBuffer);
            buffer.Write(valueBuffer[0..written]);         
        }

        public static void WriteLong(this IWriteBuffer<char> buffer, long value)
        {
            Span<char> valueBuffer = stackalloc char[20];
            var written = CharConverter.LongToUnicode(value, valueBuffer);
            buffer.Write(valueBuffer[0..written]);
        }

        public static void WriteUint(this IWriteBuffer<char> buffer, uint value)
        {
            Span<char> valueBuffer = stackalloc char[20];
            var written = CharConverter.UintToUnicode(value, valueBuffer);
            buffer.Write(valueBuffer[0..written]);
        }

        public static void WriteUlong(this IWriteBuffer<char> buffer, ulong value)
        {
            Span<char> valueBuffer = stackalloc char[20];
            var written = CharConverter.UlongToUnicode(value, valueBuffer);
            buffer.Write(valueBuffer[0..written]);
        }

        public static void WriteFloat(this IWriteBuffer<char> buffer, float value, int precision)
        {
            Span<char> valueBuffer = stackalloc char[20];
            var written = CharConverter.FloatToUnicode(value, valueBuffer, precision);
            buffer.Write(valueBuffer[0..written]);
        }

        public static void WriteDouble(this IWriteBuffer<char> buffer, double value, int precision)
        {
            Span<char> valueBuffer = stackalloc char[20];
            var written = CharConverter.DoubleToUnicode(value, valueBuffer, precision);
            buffer.Write(valueBuffer[0..written]);
        }
    }
}