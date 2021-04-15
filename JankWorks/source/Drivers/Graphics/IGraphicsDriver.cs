using System;
using System.Collections.Generic;
using System.Text;

using JankWorks.Drivers.Interface;

using JankWorks.Graphics;
using JankWorks.Interface;

namespace JankWorks.Drivers.Graphics
{
    public interface IGraphicsDriver : IDriver
    {
        GraphicsApi GraphicsApi { get; }

        GraphicsDevice CreateGraphicsDevice(SurfaceSettings settings, IRenderTarget renderTarget);
    }

    public enum GraphicsApi
    {
        Other,
        OpenGL,
        Vulkan,
        DirectX,
        Metal
    }

}
