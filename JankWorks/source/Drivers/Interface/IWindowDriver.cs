
using JankWorks.Interface;
using JankWorks.Drivers.Graphics;

namespace JankWorks.Drivers.Interface
{
    public interface IWindowDriver : IDriver
    {
        Window CreateWindow(WindowSettings settings, IGraphicsDriver graphicDriver);
    }
}
