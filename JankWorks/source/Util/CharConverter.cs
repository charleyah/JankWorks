using System;

namespace JankWorks.Util
{
    public static class CharConverter
    {
        public static int IntToUnicode(int value, Span<char> chars)
        {
            unsafe
            {
                fixed(char* ptr = chars)
                {
                    return CharConverter.IntToUnicode(value, ptr, chars.Length, 0);
                }
            }
        }

        public static int IntToUnicode(int value, char[] chars, int offset)
        {
            if (value < 0)
            {
                int written = CharConverter.UintToUnicode((uint)-value, chars, offset + 1);

                // Add the minus afterwards to avoid modifying the char array incase of out of bounds.
                chars[offset] = '-';
                return ++written;
            }
            else
            {
                return CharConverter.UintToUnicode((uint)value, chars, offset);
            }
        }

        public static unsafe int IntToUnicode(int value, char* chars, int length, int offset)
        {
            if (value < 0)
            {
                int written = CharConverter.UintToUnicode((uint)-value, chars, length, offset + 1);

                // Add the minus afterwards to avoid modifying the char array incase of out of bounds.
                chars[offset] = '-';
                return ++written;
            }
            else
            {
                return CharConverter.UintToUnicode((uint)value, chars, length, offset);
            }
        }

        public static int LongToUnicode(long value, Span<char> chars)
        {
            unsafe
            {
                fixed (char* ptr = chars)
                {
                    return CharConverter.LongToUnicode(value, ptr, chars.Length, 0);
                }
            }
        }

        public static int LongToUnicode(long value, char[] chars, int offset)
        {
            if (value < 0)
            {
                int written = CharConverter.UlongToUnicode((ulong)-value, chars, offset + 1);

                // Add the minus afterwards to avoid modifying the char array incase of out of bounds.
                chars[offset] = '-';
                return ++written;
            }
            else
            {
                return CharConverter.UlongToUnicode((ulong)value, chars, offset);
            }
        }

        public static unsafe int LongToUnicode(long value, char* chars, int length, int offset)
        {
            if (value < 0)
            {
                int written = CharConverter.UlongToUnicode((ulong)-value, chars, length, offset + 1);

                // Add the minus afterwards to avoid modifying the char array incase of out of bounds.
                chars[offset] = '-';
                return ++written;
            }
            else
            {
                return CharConverter.UlongToUnicode((ulong)value, chars, length, offset);
            }
        }

        public static int UintToUnicode(uint value, Span<char> chars)
        {
            unsafe
            {
                fixed (char* ptr = chars)
                {
                    return CharConverter.UintToUnicode(value, ptr, chars.Length, 0);
                }
            }
        }

        public static unsafe int UintToUnicode(uint value, char[] chars, int offset)
        {
            fixed (char* charptr = chars)
            {
                return CharConverter.UintToUnicode(value, charptr, chars.Length, offset);
            }
        }

        public static unsafe int UintToUnicode(uint value, char* chars, int length, int offset)
        {
            int written = 0;

            char* charbuffer = stackalloc char[12];

            int cbIndex = 0;

            // reverse loop through individual digits of value in base 10
            do
            {
                uint digit = value % 10;

                // 48 is the unicode/ascii value of zero and assuming digit is 0-9 we can simply add 48 to write the correct char value.
                charbuffer[cbIndex++] = (char)(48 + digit);
                value /= 10;
            }
            while (value != 0);

            // With cbIndex holding the total number of chars we can now do a bounds check.
            if (cbIndex > (length - offset)) { throw new IndexOutOfRangeException(); }

            do
            {
                // write the char buffer reversed to the char pointer
                chars[offset + written++] = charbuffer[--cbIndex];
            }
            while (cbIndex > 0);

            return written;
        }

        public static int UlongToUnicode(ulong value, Span<char> chars)
        {
            unsafe
            {
                fixed (char* ptr = chars)
                {
                    return CharConverter.UlongToUnicode(value, ptr, chars.Length, 0);
                }
            }
        }

        public static unsafe int UlongToUnicode(ulong value, char[] chars, int offset)
        {
            fixed (char* charptr = chars)
            {
                return CharConverter.UlongToUnicode(value, charptr, chars.Length, offset);
            }
        }

        public static unsafe int UlongToUnicode(ulong value, char* chars, int length, int offset)
        {
            int written = 0;

            char* charbuffer = stackalloc char[20];

            int cbIndex = 0;

            // reverse loop through individual digits of value in base 10
            do
            {
                ulong digit = value % 10;

                // 48 is the unicode/ascii value of zero and assuming digit is 0-9 we can simply add 48 to write the correct char value.
                charbuffer[cbIndex++] = (char)(48 + digit);
                value /= 10;
            }
            while (value != 0);

            // With cbIndex holding the total number of chars we can now do a bounds check.
            if (cbIndex > (length - offset)) { throw new IndexOutOfRangeException(); }

            do
            {
                // write the char buffer reversed to the char pointer
                chars[offset + written++] = charbuffer[--cbIndex];
            }
            while (cbIndex > 0);

            return written;
        }

        public static int FloatToUnicode(float value, Span<char> chars, int precision)
        => CharConverter.DoubleToUnicode(value, chars, precision);

        public static int FloatToUnicode(float value, char[] chars, int offset, int precision)
        => CharConverter.DoubleToUnicode(value, chars, offset, precision);

        public static unsafe int FloatToUnicode(float value, char* chars, int length, int offset, int precision)
        => CharConverter.DoubleToUnicode(value, chars, length, offset, precision);

        public unsafe static int DoubleToUnicode(double value, Span<char> chars, int precision)
        {
            fixed (char* ptr = chars)
            {
                return CharConverter.DoubleToUnicode(value, ptr, chars.Length, 0, precision);
            }                
        }

        public static unsafe int DoubleToUnicode(double value, char[] chars, int offset, int precision)
        {
            fixed (char* charptr = chars)
            {
                return CharConverter.DoubleToUnicode(value, charptr, chars.Length, offset, precision);
            }
        }

        public static unsafe int DoubleToUnicode(double value, char* chars, int length, int offset, int precision)
        {
            // TODO: This conversion of double to chars needs some redoing as it can't handle values requiring scientific notation and can probably be simplified

            int written = 0;
            int space = length - offset;

            // Handle constants first
            if (double.IsNaN(value))
            {
                if (space < 3) { throw new IndexOutOfRangeException(); }

                chars[offset + written++] = 'N';
                chars[offset + written++] = 'a';
                chars[offset + written++] = 'N';
                return written;
            }
            else if (double.IsPositiveInfinity(value))
            {
                if (space < 1) { throw new IndexOutOfRangeException(); }
                chars[offset + written++] = '∞';
                return written;
            }
            else if (double.IsNegativeInfinity(value))
            {
                if (space < 2) { throw new IndexOutOfRangeException(); }
                chars[offset + written++] = '-';
                chars[offset + written++] = '∞';
                return written;
            }

            // correct precision to be a positive number and no higher than 15 for rounding purposes.
            precision = Math.Max(0, precision);
            precision = Math.Min(15, precision);

            ulong ipart = 0;
            ulong fpart = 0;
            bool isNeg = value < 0;

            double vtrunc = Math.Truncate(value);

            if (isNeg)
            {
                ipart = (ulong)-(long)vtrunc;
                fpart = (ulong)-(long)(Math.Round(value - vtrunc, precision) * Math.Pow(10, precision));
            }
            else
            {
                ipart = (ulong)vtrunc;
                fpart = (ulong)(Math.Round(value - vtrunc, precision) * Math.Pow(10, precision));
            }

            char* charbuffer = stackalloc char[20];

            int cbIndex = 0;

            // Start by handling the precision if requested.
            if (precision > 0)
            {
                // We loop until precision has been met instead of when there is no one digits to iterate. This pads out the result with zeros.
                do
                {
                    ulong digit = fpart % 10;
                    // 48 is the unicode/ascii value of zero and assuming digit is 0-9 we can simply add 48 to write the correct char value.
                    charbuffer[cbIndex++] = (char)(48 + digit);
                    fpart /= 10;
                    precision--;
                }
                while (precision > 0);

                charbuffer[cbIndex++] = '.';
            }

            do
            {
                // Just like above accept iterating through the integer part of the floating value.
                ulong digit = ipart % 10;
                charbuffer[cbIndex++] = (char)(48 + digit);
                ipart /= 10;
            }
            while (ipart != 0);

            if (isNeg) { charbuffer[cbIndex++] = '-'; }

            // With cbIndex holding the total number of chars we can now do a bounds check.
            if (cbIndex > space) { throw new IndexOutOfRangeException(); }

            do
            {
                // Write the reversed result to the char pointer
                chars[offset + written++] = charbuffer[--cbIndex];
            }
            while (cbIndex > 0);

            return written;
        }
    }
}