using System;

namespace JankWorks.Platform
{
    public abstract class SystemEnvironment
    {
        public static SystemEnvironment Current { get; private set; }


        static SystemEnvironment()
        {
            if(OperatingSystem.IsWindows())
            {
                SystemEnvironment.Current = new Windows.WinSystem();
            }
            else
            {
                throw new NotSupportedException();
            }
                    
        }

        public abstract SystemPlatform OS { get; }
        public abstract string Description { get; }

        public abstract LibraryLoader LoadLibrary(params string[] names);
    }

    public enum SystemPlatform
    {
        Windows,
        MacOS,
        Linux
    }
}
