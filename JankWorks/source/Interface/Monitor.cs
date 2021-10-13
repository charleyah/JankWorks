
using JankWorks.Drivers;

namespace JankWorks.Interface
{
    public abstract class Monitor
    {
        public string Name { get; protected set; }

        public abstract DisplayMode DisplayMode { get; }

        public abstract DisplayMode[] SupportedDisplayModes { get; }

        public static Monitor PrimaryMonitor => DriverConfiguration.Drivers.monitorApi.GetPrimaryMonitor();

        public static Monitor[] Monitors => DriverConfiguration.Drivers.monitorApi.GetMonitors();
    }
}
