using System;

namespace JankWorks.Platform.Windows
{
    sealed class WinSystem : SystemEnvironment
    {
        public override string Description => "Windows";

        public override SystemPlatform OS => SystemPlatform.Windows;

        public override LibraryLoader LoadLibrary(string name) => new NetCoreLibraryLoader(name);
    }
}
