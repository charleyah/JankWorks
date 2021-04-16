using System;

using JankWorks.Core;

namespace JankWorks.Platform
{
    public abstract class LibraryLoader : Disposable
    {
        public abstract IntPtr LoadFunction(string name);
    }
}
