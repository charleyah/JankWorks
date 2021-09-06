using System;
using System.Text;

namespace JankWorks.Util
{
    // the pinnacle of bad string implementations
    public readonly struct CString
    {
        public static Encoding DefaultEncoding => Encoding.UTF8;

        private readonly unsafe byte* chars;

        public int Length
        {
            get
            {
                // a terrible implementation for a terrible strlen 
                unsafe
                {
                    if (this.IsNullPointer)
                    {
                        return 0;
                    }
                    else
                    {
                        byte* cursor = chars;
                        while (*cursor != 0)
                        {
                            cursor++;
                        }
                        return (int)(cursor - chars);

                    }                            
                }
            }
        }

        public bool IsNullPointer
        {
            get
            {
                unsafe { return (int)this.chars == 0; }
            }
        }

        public unsafe Span<byte> Value => new Span<byte>(this.chars, this.Length);

        public unsafe CString(IntPtr ptr) : this((byte*)ptr.ToPointer()) { }
        public unsafe CString(byte* chars) { this.chars = chars; }

        

        public override string ToString() => this.ToString(CString.DefaultEncoding);
        public unsafe string ToString(Encoding encoding)
        { 
            if(this.IsNullPointer)
            {
                return null;
            }
            else
            {
               return encoding.GetString(this.chars, this.Length);
            }            
        }

        
        public int GetCharCount() => this.GetCharCount(CString.DefaultEncoding);
        public unsafe int GetCharCount(Encoding encoding)
        {
            if(this.IsNullPointer)
            {
                return 0;
            }
            else
            {
               return encoding.GetCharCount(this.chars, this.Length);
            }           
        }


        public void Set(ReadOnlySpan<char> chars) => this.Set(chars, CString.DefaultEncoding);
        public void Set(ReadOnlySpan<char> chars, Encoding encoding)
        {
            if (this.IsNullPointer)
            {
                throw new NullReferenceException();
            }

            var byteCount = encoding.GetByteCount(chars);

            if (byteCount <= this.Length)
            {
                var encoder = encoding.GetEncoder();
                unsafe
                {
                    encoder.GetBytes(chars, new Span<byte>(this.chars, byteCount), true);
                }
            }
            else 
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        public static implicit operator string(CString cString) => cString.ToString(CString.DefaultEncoding);

        public static unsafe explicit operator CString(byte* cstr) => new CString(cstr);
    }
}