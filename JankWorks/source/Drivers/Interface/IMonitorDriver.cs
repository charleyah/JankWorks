using JankWorks.Interface;

namespace JankWorks.Drivers.Interface
{
    public interface IMonitorDriver : IDriver
    {
        Monitor GetPrimaryMonitor();
        Monitor[] GetMonitors();
    }
}
