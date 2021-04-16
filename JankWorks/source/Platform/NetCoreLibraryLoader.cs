using System;
using System.Runtime.InteropServices;

namespace JankWorks.Platform
{
    sealed class NetCoreLibraryLoader : LibraryLoader
    {
        private IntPtr libhandle;
        public NetCoreLibraryLoader(string lib)
        {
            this.libhandle = NativeLibrary.Load(lib);
        }

        public override IntPtr LoadFunction(string name)
        {
            IntPtr func = IntPtr.Zero;
            NativeLibrary.TryGetExport(this.libhandle, name, out func);
            return func;
        }

        protected override void Dispose(bool finalising)
        {
            NativeLibrary.Free(this.libhandle);
            base.Dispose(finalising);
        }
    }
}
