using System;
using System.IO;

namespace JankWorks.Platform.Windows
{
    sealed class WinSystem : SystemEnvironment
    {
        public override string Description => "Windows";

        public override SystemPlatform OS => SystemPlatform.Windows;
        
        public override LibraryLoader LoadLibrary(string name)
        {
            var basepath = AppDomain.CurrentDomain.BaseDirectory;

            var winpath = Path.Combine(basepath, "runtimes", "win", "native", name);
            var win64path = Path.Combine(basepath, "runtimes", "win-x64", "native", name);
            var win32path = Path.Combine(basepath, "runtimes", "win-x86", "native", name);

            if (File.Exists(winpath)) { return new NetCoreLibraryLoader(winpath); }
            else if (File.Exists(win64path)) { return new NetCoreLibraryLoader(win64path); }
            else if (File.Exists(win32path)) { return new NetCoreLibraryLoader(win32path); }
            else { return new NetCoreLibraryLoader(name); }
        }
    }
}
