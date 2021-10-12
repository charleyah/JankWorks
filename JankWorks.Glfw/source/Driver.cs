using System;

using JankWorks.Core;

using JankWorks.Interface;

using JankWorks.Drivers;
using JankWorks.Drivers.Graphics;
using JankWorks.Drivers.Interface;

using JankWorks.Drivers.Glfw.Interface;
using JankWorks.Drivers.Glfw.Native;

using static JankWorks.Drivers.Glfw.Native.Functions;

[assembly: JankWorksDriver(typeof(JankWorks.Drivers.Glfw.Driver))]

namespace JankWorks.Drivers.Glfw
{
    public sealed class Driver : Disposable, IWindowDriver, IMonitorDriver
    {
        private readonly GLFWerrorfun errorHandler;

        public Driver()
        {
            this.errorHandler = new GLFWerrorfun((ec, des) => Console.Out.WriteLine(des));
            Functions.Init();
            glfwInit();
            glfwSetErrorCallback(this.errorHandler);
        }

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

        public Monitor[] GetMonitors()
        {
            unsafe
            {
                void** ptr = (void**)glfwGetMonitors(out var count).ToPointer();

                var monitors = new GlfwMonitor[count];
                for (int index = 0; index < count; index++)
                {
                    monitors[index] = new GlfwMonitor(new IntPtr(*ptr));
                    ptr++;
                }

                return monitors;
            }
        }
        
        public Monitor GetPrimaryMonitor() => new GlfwMonitor(glfwGetPrimaryMonitor());        

        protected override void Dispose(bool finalising)
        {
            glfwSetErrorCallback(null);
            glfwTerminate();
            Functions.loader.Dispose();
            base.Dispose(finalising);
        }
    }
}
