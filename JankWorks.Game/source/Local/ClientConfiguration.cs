using System;
using System.Linq;

using JankWorks.Interface;

using JankWorks.Game.Configuration;

namespace JankWorks.Game.Local
{
    public struct ClientConfgiuration
    {
        public Monitor Monitor { get; set; }
        public VideoMode VideoMode { get; set; }
        public WindowStyle WindowStyle { get; set; }
        public float FrameRate { get; set; }
        public bool Vsync { get; set; }

        const string VideoSection = "Video";
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
            settings.SetEntry(MonitorEntry, this.Monitor.Name, VideoSection);
            settings.SetEntry(WindowStyleEntry, this.WindowStyle.ToString(), VideoSection);

            var videomode = this.VideoMode;
            settings.SetEntry(WidthEntry, videomode.Width.ToString(), VideoSection);
            settings.SetEntry(HeightEntry, videomode.Height.ToString(), VideoSection);
            settings.SetEntry(BitsEntry, videomode.BitsPerPixel.ToString(), VideoSection);
            settings.SetEntry(RefreshRateEntry, videomode.RefreshRate.ToString(), VideoSection);

            settings.SetEntry(FrameRateEntry, this.FrameRate.ToString(), VideoSection);
            settings.SetEntry(VsyncEntry, this.Vsync.ToString(), VideoSection);
        }

#nullable disable

        public void Load(Settings settings)
        {
            if (settings.ContainsSection(VideoSection))
            {
                Func<string, Monitor> monitorFinder = (name) => Monitor.Monitors.Where((m) => m.Name.Equals(name)).FirstOrDefault();

                Monitor monitor = settings.GetEntry(MonitorEntry, monitorFinder, VideoSection, default);

                if (monitor != null)
                {
                    this.Monitor = monitor;
                    var videomode = this.VideoMode;

                    uint width = settings.GetEntry(WidthEntry, (entry) => uint.Parse(entry), VideoSection, videomode.Width);
                    uint height = settings.GetEntry(HeightEntry, (entry) => uint.Parse(entry), VideoSection, videomode.Height);
                    uint bits = settings.GetEntry(BitsEntry, (entry) => uint.Parse(entry), VideoSection, videomode.BitsPerPixel);
                    uint refreshRate = settings.GetEntry(RefreshRateEntry, (entry) => uint.Parse(entry), VideoSection, videomode.RefreshRate);

                    this.VideoMode = new VideoMode(width, height, bits, refreshRate);
                }

                this.WindowStyle = settings.GetEntry(WindowStyleEntry, (entry) => Enum.Parse<WindowStyle>(entry), VideoSection, this.WindowStyle);
                this.FrameRate = settings.GetEntry(FrameRateEntry, (entry) => float.Parse(entry), VideoSection, this.FrameRate);
                this.Vsync = settings.GetEntry(VsyncEntry, (entry) => bool.Parse(entry), VideoSection, this.Vsync);
            }
        }

#nullable enable

        public static ClientConfgiuration Default => new ClientConfgiuration()
        {
            Monitor = Monitor.PrimaryMonitor,
            VideoMode = Monitor.PrimaryMonitor.VideoMode,
            FrameRate = 60,
            Vsync = true,
            WindowStyle = WindowStyle.FullScreen
        };
    }
}
