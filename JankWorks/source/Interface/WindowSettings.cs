using System.Reflection;

using JankWorks.Drivers;

namespace JankWorks.Interface
{
    public struct WindowSettings
    {
        public string Title;
        public Monitor Monitor;
        public VideoMode VideoMode;
        public WindowStyle Style;
        public bool VSync;
        public bool ShowCursor;
        public static WindowSettings Default
        {
            get
            {
                var primary = DriverConfiguration.Drivers.monitorApi.GetPrimaryMonitor();

                var settings = new WindowSettings()
                {
                    Title = Assembly.GetEntryAssembly().GetName().Name,
                    Monitor = primary,
                    VideoMode = primary.VideoMode,
                    Style = WindowStyle.Borderless,
                    VSync = true,
                    ShowCursor = true
                };

                return settings;
            }
        }
    }

    public enum WindowStyle
    {
        Windowed,
        Borderless,
        FullScreen
    }
}
