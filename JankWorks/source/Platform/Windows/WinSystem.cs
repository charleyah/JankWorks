using System;
using System.IO;

namespace JankWorks.Platform.Windows
{
    sealed class WinSystem : SystemEnvironment
    {
        public override string Description => "Windows";

        public override SystemPlatform OS => SystemPlatform.Windows;
        
        public override LibraryLoader LoadLibrary(params string[] names)
        {
            var upperbound = names.GetUpperBound(0);
            for (int index = 0; index <= upperbound; index++)
            {
                var lib = this.FindLibrary(names[index], index == upperbound);

                if(lib != null)
                {
                    return lib;
                }
            }
            throw new DllNotFoundException(string.Concat(names));
        }

        private LibraryLoader FindLibrary(string name, bool fallback)
        {
            var basepath = AppDomain.CurrentDomain.BaseDirectory;

            var winpath = Path.Combine(basepath, "runtimes", "win", "native", name);
            var win64path = Path.Combine(basepath, "runtimes", "win-x64", "native", name);
            var win32path = Path.Combine(basepath, "runtimes", "win-x86", "native", name);


            if (File.Exists(winpath)) { return new NetCoreLibraryLoader(winpath); }
            else if (Environment.Is64BitProcess == true && File.Exists(win64path)) { return new NetCoreLibraryLoader(win64path); }
            else if (Environment.Is64BitProcess == false && File.Exists(win32path)) { return new NetCoreLibraryLoader(win32path); }
            else if(fallback) 
            { 
                return new NetCoreLibraryLoader(name); 
            }
            else
            {
                return null;
            }
        }
    }
}
