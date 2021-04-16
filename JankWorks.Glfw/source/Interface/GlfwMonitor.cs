using System;
using System.Text;

using JankWorks.Util;
using JankWorks.Interface;

using static JankWorks.Drivers.Glfw.Api;

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

        public override VideoMode VideoMode
        {
            get
            {
                var mode = glfwGetVideoMode(this.Handle);

                checked
                {
                    return new VideoMode((uint)mode.width, (uint)mode.height, (uint)mode.redBits * 4, (uint)mode.refreshRate);
                }
            }
        }
            

        public override VideoMode[] SupportedVideoModes
        {
            get
            {
                var modes = glfwGetVideoModes(this.Handle);

                var vmodes = new VideoMode[modes.Length];

                for (int i = 0; i < vmodes.Length; i++)
                {
                    var mode = modes[i];
                    checked
                    {
                        vmodes[i] = new VideoMode((uint)mode.width, (uint)mode.height, (uint)mode.redBits * 4, (uint)mode.refreshRate);
                    }
                }
                return vmodes;
            }
        }
    }
}
