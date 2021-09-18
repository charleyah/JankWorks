// Sourced from https://github.com/smack0007/GLFWDotNet

using System;
using System.Security;
using System.Runtime.InteropServices;

namespace JankWorks.Drivers.Glfw.Native
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
    public delegate void GLFWerrorfun(int error, string description);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
    public delegate void GLFWwindowposfun(IntPtr window, int xpos, int ypos);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
    public delegate void GLFWwindowsizefun(IntPtr window, int width, int height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
    public delegate void GLFWwindowclosefun(IntPtr window);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
    public delegate void GLFWwindowrefreshfun(IntPtr window);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
    public delegate void GLFWwindowfocusfun(IntPtr window, int focused);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
    public delegate void GLFWwindowiconifyfun(IntPtr window, int iconified);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
    public delegate void GLFWwindowmaximizefun(IntPtr window, int iconified);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
    public delegate void GLFWframebuffersizefun(IntPtr window, int width, int height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
    public delegate void GLFWwindowcontentscalefun(IntPtr window, float xscale, float yscale);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
    public delegate void GLFWmousebuttonfun(IntPtr window, int button, int action, int mods);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
    public delegate void GLFWcursorposfun(IntPtr window, double xpos, double ypos);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
    public delegate void GLFWcursorenterfun(IntPtr window, int entered);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
    public delegate void GLFWscrollfun(IntPtr window, double xoffset, double yoffset);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
    public delegate void GLFWkeyfun(IntPtr window, int key, int scancode, int action, int mods);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
    public delegate void GLFWcharfun(IntPtr window, uint codepoint);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
    public delegate void GLFWcharmodsfun(IntPtr window, uint codepoint, int mods);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
    public delegate void GLFWdropfun(IntPtr window, int count, IntPtr paths);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
    public delegate void GLFWmonitorfun(IntPtr monitor, int @event);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
    public delegate void GLFWjoystickfun(int jid, int @event);
}
