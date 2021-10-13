using System;

using JankWorks.Util;
using JankWorks.Interface;

using JankWorks.Drivers.Glfw.Native;
using static JankWorks.Drivers.Glfw.Native.Functions;

namespace JankWorks.Drivers.Glfw.Interface
{
    public sealed class GlfwMonitor : Monitor
    {
        internal IntPtr Handle { get; private set; }

        public GlfwMonitor(IntPtr handle)
        {
            this.Handle = handle;
            this.Name = new CString(glfwGetMonitorName(handle));
        }

        public override DisplayMode DisplayMode
        {
            get
            {
                unsafe
                {
                    GLFWvidmode* mode = glfwGetVideoMode(this.Handle);

                    checked
                    {
                        return new DisplayMode((uint)mode->width, (uint)mode->height, (uint)mode->redBits * 4, (uint)mode->refreshRate);
                    }
                }
                
            }
        }
            

        public override DisplayMode[] SupportedDisplayModes
        {
            get
            {
                unsafe
                {
                    ReadOnlySpan<GLFWvidmode> modes = new ReadOnlySpan<GLFWvidmode>(glfwGetVideoModes(this.Handle, out var count), count);

                    var vmodes = new DisplayMode[modes.Length];

                    for (int i = 0; i < vmodes.Length; i++)
                    {
                        var mode = modes[i];
                        checked
                        {
                            vmodes[i] = new DisplayMode((uint)mode.width, (uint)mode.height, (uint)mode.redBits * 4, (uint)mode.refreshRate);
                        }
                    }
                    return vmodes;
                }            
            }
        }
    }
}
