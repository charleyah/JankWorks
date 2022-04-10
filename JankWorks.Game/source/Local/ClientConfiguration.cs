using System;
using System.Linq;
using System.Diagnostics;

using JankWorks.Interface;

using JankWorks.Game.Configuration;

namespace JankWorks.Game.Local
{
    public struct ClientConfgiuration
    {
        public Monitor Monitor { get => this.monitor; set => this.monitor = value; }

        public DisplayMode DisplayMode { get => this.displayMode; set => this.displayMode = value; }

        public WindowStyle WindowStyle { get => this.windowStyle; set => this.windowStyle = value; }

        public uint UpdateRate 
        {
            get => this.updateRate;
            set => this.updateRate = Math.Min(MaxUpdateRate, value);
        }

        public bool Vsync { get => this.vsync; set => this.vsync = value; }

        public const uint MaxUpdateRate = 360;

        private Monitor monitor;
        private DisplayMode displayMode;
        private WindowStyle windowStyle;
        private uint updateRate;
        private bool vsync;

        private const string DisplaySection = "Display";
        private const string MonitorEntry = "Monitor";
        private const string WindowStyleEntry = "WindowStyle";

        private const string WidthEntry = "Width";
        private const string HeightEntry = "Height";
        private const string BitsEntry = "Bits";
        private const string RefreshRateEntry = "RefreshRate";

        private const string UpdateRateEntry = "UpdateRate";
        private const string VsyncEntry = "Vsync";

        public void Save(Settings settings)
        {
            settings.SetEntry(MonitorEntry, this.Monitor.Name, DisplaySection);
            settings.SetEntry(WindowStyleEntry, this.WindowStyle.ToString(), DisplaySection);

            var displaymode = this.DisplayMode;
            settings.SetEntry(WidthEntry, displaymode.Width.ToString(), DisplaySection);
            settings.SetEntry(HeightEntry, displaymode.Height.ToString(), DisplaySection);
            settings.SetEntry(BitsEntry, displaymode.BitsPerPixel.ToString(), DisplaySection);
            settings.SetEntry(RefreshRateEntry, displaymode.RefreshRate.ToString(), DisplaySection);

            settings.SetEntry(UpdateRateEntry, this.UpdateRate.ToString(), DisplaySection);
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
                    var displaymode = this.DisplayMode;

                    uint width = settings.GetEntry(WidthEntry, (entry) => uint.Parse(entry), DisplaySection, displaymode.Width);
                    uint height = settings.GetEntry(HeightEntry, (entry) => uint.Parse(entry), DisplaySection, displaymode.Height);
                    uint bits = settings.GetEntry(BitsEntry, (entry) => uint.Parse(entry), DisplaySection, displaymode.BitsPerPixel);
                    uint refreshRate = settings.GetEntry(RefreshRateEntry, (entry) => uint.Parse(entry), DisplaySection, displaymode.RefreshRate);

                    this.DisplayMode = new DisplayMode(width, height, bits, refreshRate);
                }

                this.WindowStyle = settings.GetEntry(WindowStyleEntry, (entry) => Enum.Parse<WindowStyle>(entry), DisplaySection, this.WindowStyle);
                this.UpdateRate = settings.GetEntry(UpdateRateEntry, (entry) => uint.Parse(entry), DisplaySection, this.UpdateRate);
                this.Vsync = settings.GetEntry(VsyncEntry, (entry) => bool.Parse(entry), DisplaySection, this.Vsync);
            }
        }

        public static ClientConfgiuration Default
        {
            get
            {
                var monitor = Monitor.PrimaryMonitor;
                var displaymode = monitor.DisplayMode;

                if(Debugger.IsAttached)
                {
                    return new ClientConfgiuration()
                    {
                        Monitor = monitor,
                        DisplayMode = new DisplayMode(1280, 800, 32, 60),
                        UpdateRate = displaymode.RefreshRate,
                        Vsync = true,
                        WindowStyle = WindowStyle.Windowed
                    };
                }
                else
                {
                    return new ClientConfgiuration()
                    {
                        Monitor = monitor,
                        DisplayMode = displaymode,
                        UpdateRate = displaymode.RefreshRate,
                        Vsync = false,
                        WindowStyle = WindowStyle.FullScreen
                    };
                }
            }            
        }
    }
}
