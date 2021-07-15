using System;

using JankWorks.Platform;

namespace JankWorks.Game.Platform
{
    internal abstract class PlatformApi
    {
        public static PlatformApi Instance { get; private set; }

        static PlatformApi()
        {
            var sys = SystemEnvironment.Current.OS;

            switch(sys)
            {
                case SystemPlatform.Windows:
                    PlatformApi.Instance = new Windows.WindowsApi();
                    break;
                default:
                    PlatformApi.Instance = new FallbackApi();
                    break;
            }
        }

        public abstract void Sleep(TimeSpan time);
    }
}
