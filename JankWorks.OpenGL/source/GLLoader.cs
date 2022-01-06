using System;
using System.Text;
using JankWorks.Platform;

namespace JankWorks.Drivers.OpenGL
{
    sealed unsafe class GLLoader : LibraryLoader
    {
        private LibraryLoader osloader;
        private Encoder utfencoder;
        private delegate* unmanaged<byte*, IntPtr> wglGetProcAddress;

        public GLLoader(LibraryLoader osloader)
        {
            this.osloader = osloader;
            this.utfencoder = Encoding.UTF8.GetEncoder();

            this.wglGetProcAddress = (delegate* unmanaged<byte*, IntPtr>)this.osloader.LoadFunction("wglGetProcAddress");
        }

        public override IntPtr LoadFunction(string name)
        {            
            var fnptr = this.osloader.LoadFunction(name);

            if(fnptr == IntPtr.Zero)
            {
                var upperbound = this.utfencoder.GetByteCount(name, false);
                var length = upperbound + 1;

                Span<byte> utf8Name = stackalloc byte[length];
                this.utfencoder.GetBytes(name, utf8Name, true);
                utf8Name[upperbound] = 0;

                fixed(byte* utf8NamePtr = utf8Name)
                {
                    fnptr = this.wglGetProcAddress(utf8NamePtr);
                }                
            }
            return fnptr;          
        }

        protected override void Dispose(bool disposing)
        {
            this.osloader.Dispose();
            base.Dispose(disposing);
        }
    }
}