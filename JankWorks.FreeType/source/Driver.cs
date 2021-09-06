using System;
using System.IO;

using JankWorks.Core;
using JankWorks.Graphics;

using JankWorks.Drivers;
using JankWorks.Drivers.Graphics;

using JankWorks.Drivers.FreeType.Graphics;
using JankWorks.Drivers.FreeType.Native;

using static JankWorks.Drivers.FreeType.Native.Functions;

[assembly: JankWorksDriver(typeof(JankWorks.Drivers.FreeType.Driver))]

namespace JankWorks.Drivers.FreeType
{
    public class Driver : Disposable, IFontDriver
    {
        FT_Library library;

        public Driver()
        {
            Functions.Init();

            FT_Init_FreeType(out this.library);
        }
        public Font LoadFontFromStream(Stream stream, FontFormat format)
        {
            FT_Face face;
            FT_Error error = FT_Error.FT_Err_Ok;
            IDisposable source;

            if (stream is UnmanagedMemoryStream ums)
            {
                unsafe
                {
                    error = FT_New_Memory_Face(this.library, (IntPtr)ums.PositionPointer, (int)ums.Length, 0, out face);
                }
                source = ums;
            }
            else
            {
                MemoryStream memoryStream;

                if (stream is MemoryStream ms)
                {
                    memoryStream = ms;
                }
                else
                {
                    int sourceLength;
                    checked
                    {
                        sourceLength = (int)stream.Length;
                    }
                    memoryStream = new MemoryStream(sourceLength);
                    stream.CopyTo(memoryStream);
                }

                unsafe
                {
                    fixed (byte* ptr = memoryStream.GetBuffer())
                    {
                        error = FT_New_Memory_Face(this.library, (IntPtr)ptr, (int)memoryStream.Length, 0, out face);
                    }
                }
                source = memoryStream;
            }


            if(error != FT_Error.FT_Err_Ok)
            {
                throw new ApplicationException(error.ToString());
            }

            return new FreeTypeFont(face, source);
        }

        protected override void Dispose(bool finalising)
        {
            FT_Done_FreeType(this.library);
            base.Dispose(finalising);
        }
    }
}