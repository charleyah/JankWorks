
using JankWorks.Drivers;

namespace JankWorks.Interface
{
    public abstract class Monitor
    {
        public string Name { get; protected set; }

        public abstract VideoMode VideoMode { get; }

        public abstract VideoMode[] SupportedVideoModes { get; }

        public static Monitor PrimaryMonitor => DriverConfiguration.Drivers.monitorApi.GetPrimaryMonitor();

        public static Monitor[] Monitors => DriverConfiguration.Drivers.monitorApi.GetMonitors();
    }
}
