using System;

using System.IO;

using JankWorks.Drivers.Interface;
using JankWorks.Drivers.Graphics;
using JankWorks.Drivers.Audio;

using JankWorks.Interface;
using JankWorks.Graphics;
using JankWorks.Audio;

namespace JankWorks.Drivers
{
    public sealed class DriverUnitialisedException : 
    Exception, 
    IMonitorDriver, 
    IWindowDriver, 
    IGraphicsDriver,
    IImageDriver,
    IFontDriver,
    IAudioDriver
    {
        Monitor[] IMonitorDriver.GetMonitors() => throw this;

        Monitor IMonitorDriver.GetPrimaryMonitor() => throw this;

        Window IWindowDriver.CreateWindow(WindowSettings settings, IGraphicsDriver graphicDriver) => throw this;



        Image IImageDriver.LoadFromStream(Stream stream, ImageFormat format) => throw this;

        Image IImageDriver.Create(Vector2i size, ImageFormat format) => throw this;



        GraphicsDevice IGraphicsDriver.CreateGraphicsDevice(SurfaceSettings settings, IRenderTarget renderTarget) => throw this;

        GraphicsApi IGraphicsDriver.GraphicsApi => throw this;

        Font IFontDriver.LoadFontFromStream(Stream stream, FontFormat format) => throw this;

        AudioDevice IAudioDriver.GetDefaultAudioDevice() => throw this;

        public static readonly DriverUnitialisedException driver = new DriverUnitialisedException();
    }
}