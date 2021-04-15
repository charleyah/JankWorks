using System;
using System.Linq;

using JankWorks.Core;

using JankWorks.Drivers;
using JankWorks.Drivers.Graphics;
using JankWorks.Drivers.Interface;

using JankWorks.Interface;

using JankWorks.Drivers.Glfw.Interface;

using static JankWorks.Drivers.Glfw.Api;


[assembly: JankWorksDriver(typeof(JankWorks.Drivers.Glfw.Driver))]

namespace JankWorks.Drivers.Glfw
{
    public sealed class Driver : Disposable, IWindowDriver, IMonitorDriver
    {
        public Driver()
        {
            glfwInit();
            glfwSetErrorCallback(new GLFWerrorfun((ec, des) => Console.Out.WriteLine(des)));
        }

        public string Name => typeof(Driver).FullName;

        public Window CreateWindow(WindowSettings settings, IGraphicsDriver graphicDriver)
        {
            if (graphicDriver.GraphicsApi == GraphicsApi.OpenGL)
            {
                return new GlfwWindow(settings);
            }
            else 
            {
                throw new NotSupportedException();
            }
        }

        public Monitor[] GetMonitors() => (from IntPtr monitorHandle in glfwGetMonitors() select new GlfwMonitor(monitorHandle)).ToArray();
        
        public Monitor GetPrimaryMonitor() => new GlfwMonitor(glfwGetPrimaryMonitor());        

        protected override void Dispose(bool finalising)
        {
            glfwSetErrorCallback(null);
            glfwTerminate();
            base.Dispose(finalising);
        }
    }
}
