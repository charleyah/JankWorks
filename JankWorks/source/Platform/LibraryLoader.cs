using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Loader;

using JankWorks.Core;

namespace JankWorks.Platform
{
    public abstract class LibraryLoader : Disposable
    {
        public abstract IntPtr LoadFunction(string name);
    }
}
