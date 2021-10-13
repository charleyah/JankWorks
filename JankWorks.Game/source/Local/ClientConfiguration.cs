using System;
using System.Linq;
using System.Diagnostics;

using JankWorks.Interface;

using JankWorks.Game.Configuration;

namespace JankWorks.Game.Local
{
    public struct ClientConfgiuration
    {
        public Monitor Monitor { get; set; }

        public VideoMode VideoMode { get; set; }

        public WindowStyle WindowStyle { get; set; }

        public uint FrameRate { get; set; }

        public bool Vsync { get; set; }

        const string DisplaySection = "Display";
        const string MonitorEntry = "Monitor";
        const string WindowStyleEntry = "WindowStyle";

        const string WidthEntry = "Width";
        const string HeightEntry = "Height";
        const string BitsEntry = "Bits";
        const string RefreshRateEntry = "RefreshRate";

        const string FrameRateEntry = "FrameRate";
        const string VsyncEntry = "Vsync";

        public void Save(Settings settings)
        {
            settings.SetEntry(MonitorEntry, this.Monitor.Name, DisplaySection);
            settings.SetEntry(WindowStyleEntry, this.WindowStyle.ToString(), DisplaySection);

            var videomode = this.VideoMode;
            settings.SetEntry(WidthEntry, videomode.Width.ToString(), DisplaySection);
            settings.SetEntry(HeightEntry, videomode.Height.ToString(), DisplaySection);
            settings.SetEntry(BitsEntry, videomode.BitsPerPixel.ToString(), DisplaySection);
            settings.SetEntry(RefreshRateEntry, videomode.RefreshRate.ToString(), DisplaySection);

            settings.SetEntry(FrameRateEntry, this.FrameRate.ToString(), DisplaySection);
            settings.SetEntry(VsyncEntry, this.Vsync.ToString(), DisplaySection);
        }

        public void Load(Settings settings)
        {
            if (settings.ContainsSection(DisplaySection))
            {
                Func<string, Monitor> monitorFinder = (name) => Monitor.Monitors.Where((m) => m.Name.Equals(name)).FirstOrDefault();

                Monitor monitor = settings.GetEntry(MonitorEntry, monitorFinder, DisplaySection, default);

                if (monitor != null)
                {
                    this.Monitor = monitor;
                    var videomode = this.VideoMode;

                    uint width = settings.GetEntry(WidthEntry, (entry) => uint.Parse(entry), DisplaySection, videomode.Width);
                    uint height = settings.GetEntry(HeightEntry, (entry) => uint.Parse(entry), DisplaySection, videomode.Height);
                    uint bits = settings.GetEntry(BitsEntry, (entry) => uint.Parse(entry), DisplaySection, videomode.BitsPerPixel);
                    uint refreshRate = settings.GetEntry(RefreshRateEntry, (entry) => uint.Parse(entry), DisplaySection, videomode.RefreshRate);

                    this.VideoMode = new VideoMode(width, height, bits, refreshRate);
                }

                this.WindowStyle = settings.GetEntry(WindowStyleEntry, (entry) => Enum.Parse<WindowStyle>(entry), DisplaySection, this.WindowStyle);
                this.FrameRate = settings.GetEntry(FrameRateEntry, (entry) => uint.Parse(entry), DisplaySection, this.FrameRate);
                this.Vsync = settings.GetEntry(VsyncEntry, (entry) => bool.Parse(entry), DisplaySection, this.Vsync);
            }
        }

        public static ClientConfgiuration Default
        {
            get
            {
                var monitor = Monitor.PrimaryMonitor;
                var videoMode = monitor.VideoMode;

                if(Debugger.IsAttached)
                {
                    return new ClientConfgiuration()
                    {
                        Monitor = monitor,
                        VideoMode = new VideoMode(1280, 800, 32, 60),
                        FrameRate = videoMode.RefreshRate,
                        Vsync = false,
                        WindowStyle = WindowStyle.Windowed
                    };
                }
                else
                {
                    return new ClientConfgiuration()
                    {
                        Monitor = monitor,
                        VideoMode = videoMode,
                        FrameRate = videoMode.RefreshRate,
                        Vsync = false,
                        WindowStyle = WindowStyle.FullScreen
                    };
                }
            }            
        }
    }
}
