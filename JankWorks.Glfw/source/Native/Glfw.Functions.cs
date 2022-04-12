#pragma warning disable IDE1006

using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using JankWorks.Platform;

namespace JankWorks.Drivers.Glfw.Native
{
	static unsafe class Functions
	{
		private static delegate* unmanaged[Cdecl]<int> glfwInitPtr;
		private static delegate* unmanaged[Cdecl]<void> glfwTerminatePtr;
		private static delegate* unmanaged[Cdecl]<int, int, void> glfwInitHintPtr;
		private static delegate* unmanaged[Cdecl]<out int, out int, out int, void> glfwGetVersionPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr> glfwGetVersionStringPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, int> glfwGetErrorPtr;
		private static delegate* unmanaged[Cdecl]<void*, void*> glfwSetErrorCallbackPtr;
		private static delegate* unmanaged[Cdecl]<out int, IntPtr> glfwGetMonitorsPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr> glfwGetPrimaryMonitorPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, out int, out int, void> glfwGetMonitorPosPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, out int, out int, out int, out int, void> glfwGetMonitorWorkareaPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, out int, out int, void> glfwGetMonitorPhysicalSizePtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, out IntPtr, out IntPtr, void> glfwGetMonitorContentScalePtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, IntPtr> glfwGetMonitorNamePtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void> glfwSetMonitorUserPointerPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, IntPtr> glfwGetMonitorUserPointerPtr;
		private static delegate* unmanaged[Cdecl]<void*, void*> glfwSetMonitorCallbackPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, out int, GLFWvidmode*> glfwGetVideoModesPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, GLFWvidmode*> glfwGetVideoModePtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, float, void> glfwSetGammaPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, IntPtr> glfwGetGammaRampPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void> glfwSetGammaRampPtr;
		private static delegate* unmanaged[Cdecl]<void> glfwDefaultWindowHintsPtr;
		private static delegate* unmanaged[Cdecl]<int, int, void> glfwWindowHintPtr;
		private static delegate* unmanaged[Cdecl]<int, byte*, void> glfwWindowHintStringPtr;
		private static delegate* unmanaged[Cdecl]<int, int, byte*, IntPtr, IntPtr, IntPtr> glfwCreateWindowPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, void> glfwDestroyWindowPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, int> glfwWindowShouldClosePtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, int, void> glfwSetWindowShouldClosePtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, byte*, void> glfwSetWindowTitlePtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, int, IntPtr, void> glfwSetWindowIconPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, out int, out int, void> glfwGetWindowPosPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, int, int, void> glfwSetWindowPosPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, out int, out int, void> glfwGetWindowSizePtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, int, int, int, int, void> glfwSetWindowSizeLimitsPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, int, int, void> glfwSetWindowAspectRatioPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, int, int, void> glfwSetWindowSizePtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, out int, out int, void> glfwGetFramebufferSizePtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, out int, out int, out int, out int, void> glfwGetWindowFrameSizePtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, out IntPtr, out IntPtr, void> glfwGetWindowContentScalePtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, float> glfwGetWindowOpacityPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, float, void> glfwSetWindowOpacityPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, void> glfwIconifyWindowPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, void> glfwRestoreWindowPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, void> glfwMaximizeWindowPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, void> glfwShowWindowPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, void> glfwHideWindowPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, void> glfwFocusWindowPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, void> glfwRequestWindowAttentionPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, IntPtr> glfwGetWindowMonitorPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, IntPtr, int, int, int, int, int, void> glfwSetWindowMonitorPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, int, int> glfwGetWindowAttribPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, int, int, void> glfwSetWindowAttribPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void> glfwSetWindowUserPointerPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, IntPtr> glfwGetWindowUserPointerPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, void*, void*> glfwSetWindowPosCallbackPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, void*, void*> glfwSetWindowSizeCallbackPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, void*, void*> glfwSetWindowCloseCallbackPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, void*, void*> glfwSetWindowRefreshCallbackPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, void*, void*> glfwSetWindowFocusCallbackPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, void*, void*> glfwSetWindowIconifyCallbackPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, void*, void*> glfwSetWindowMaximizeCallbackPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, void*, void*> glfwSetFramebufferSizeCallbackPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, void*, void*> glfwSetWindowContentScaleCallbackPtr;
		private static delegate* unmanaged[Cdecl]<void> glfwPollEventsPtr;
		private static delegate* unmanaged[Cdecl]<void> glfwWaitEventsPtr;
		private static delegate* unmanaged[Cdecl]<double, void> glfwWaitEventsTimeoutPtr;
		private static delegate* unmanaged[Cdecl]<void> glfwPostEmptyEventPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, int, int> glfwGetInputModePtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, int, int, void> glfwSetInputModePtr;
		private static delegate* unmanaged[Cdecl]<int> glfwRawMouseMotionSupportedPtr;
		private static delegate* unmanaged[Cdecl]<int, int, IntPtr> glfwGetKeyNamePtr;
		private static delegate* unmanaged[Cdecl]<int, int> glfwGetKeyScancodePtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, int, int> glfwGetKeyPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, int, int> glfwGetMouseButtonPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, out double, out double, void> glfwGetCursorPosPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, double, double, void> glfwSetCursorPosPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, int, int, IntPtr> glfwCreateCursorPtr;
		private static delegate* unmanaged[Cdecl]<int, IntPtr> glfwCreateStandardCursorPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, void> glfwDestroyCursorPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void> glfwSetCursorPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, void*, void*> glfwSetKeyCallbackPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, void*, void*> glfwSetCharCallbackPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, void*, void*> glfwSetCharModsCallbackPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, void*, void*> glfwSetMouseButtonCallbackPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, void*, void*> glfwSetCursorPosCallbackPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, void*, void*> glfwSetCursorEnterCallbackPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, void*, void*> glfwSetScrollCallbackPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, void*, void*> glfwSetDropCallbackPtr;
		private static delegate* unmanaged[Cdecl]<int, int> glfwJoystickPresentPtr;
		private static delegate* unmanaged[Cdecl]<int, out int, IntPtr> glfwGetJoystickAxesPtr;
		private static delegate* unmanaged[Cdecl]<int, out int, IntPtr> glfwGetJoystickButtonsPtr;
		private static delegate* unmanaged[Cdecl]<int, out int, IntPtr> glfwGetJoystickHatsPtr;
		private static delegate* unmanaged[Cdecl]<int, IntPtr> glfwGetJoystickNamePtr;
		private static delegate* unmanaged[Cdecl]<int, IntPtr> glfwGetJoystickGUIDPtr;
		private static delegate* unmanaged[Cdecl]<int, IntPtr, void> glfwSetJoystickUserPointerPtr;
		private static delegate* unmanaged[Cdecl]<int, IntPtr> glfwGetJoystickUserPointerPtr;
		private static delegate* unmanaged[Cdecl]<int, int> glfwJoystickIsGamepadPtr;
		private static delegate* unmanaged[Cdecl]<void*, void*> glfwSetJoystickCallbackPtr;
		private static delegate* unmanaged[Cdecl]<byte*, int> glfwUpdateGamepadMappingsPtr;
		private static delegate* unmanaged[Cdecl]<int, IntPtr> glfwGetGamepadNamePtr;
		private static delegate* unmanaged[Cdecl]<int, out IntPtr, int> glfwGetGamepadStatePtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, byte*, void> glfwSetClipboardStringPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, IntPtr> glfwGetClipboardStringPtr;
		private static delegate* unmanaged[Cdecl]<double> glfwGetTimePtr;
		private static delegate* unmanaged[Cdecl]<double, void> glfwSetTimePtr;
		private static delegate* unmanaged[Cdecl]<ulong> glfwGetTimerValuePtr;
		private static delegate* unmanaged[Cdecl]<ulong> glfwGetTimerFrequencyPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, void> glfwMakeContextCurrentPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr> glfwGetCurrentContextPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, void> glfwSwapBuffersPtr;
		private static delegate* unmanaged[Cdecl]<int, void> glfwSwapIntervalPtr;
		private static delegate* unmanaged[Cdecl]<byte*, int> glfwExtensionSupportedPtr;
		private static delegate* unmanaged[Cdecl]<byte*, IntPtr> glfwGetProcAddressPtr;
		private static delegate* unmanaged[Cdecl]<int> glfwVulkanSupportedPtr;
		private static delegate* unmanaged[Cdecl]<out uint, IntPtr> glfwGetRequiredInstanceExtensionsPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, byte*, IntPtr> glfwGetInstanceProcAddressPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, IntPtr, uint, int> glfwGetPhysicalDevicePresentationSupportPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, out IntPtr, int> glfwCreateWindowSurfacePtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, IntPtr> glfwGetWin32WindowPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, IntPtr> glfwGetX11WindowPtr;
		private static delegate* unmanaged[Cdecl]<IntPtr, IntPtr> glfwGetCocoaWindowPtr;

		public static LibraryLoader loader;

		public static void Init()
		{
			var env = SystemEnvironment.Current;

			Functions.loader = env.OS switch
			{
				SystemPlatform.Windows => env.LoadLibrary("glfw3.dll"),
				SystemPlatform.Linux => env.LoadLibrary("libglfw.so"),
				SystemPlatform.MacOS => env.LoadLibrary("libglfw.3.dylib"),
				_ => throw new NotSupportedException()
			};


			Functions.glfwInitPtr = (delegate* unmanaged[Cdecl]<int>)Functions.LoadFunction("glfwInit");
			Functions.glfwTerminatePtr = (delegate* unmanaged[Cdecl]<void>)Functions.LoadFunction("glfwTerminate");
			Functions.glfwInitHintPtr = (delegate* unmanaged[Cdecl]<int, int, void>)Functions.LoadFunction("glfwInitHint");
			Functions.glfwGetVersionPtr = (delegate* unmanaged[Cdecl]<out int, out int, out int, void>)Functions.LoadFunction("glfwGetVersion");
			Functions.glfwGetVersionStringPtr = (delegate* unmanaged[Cdecl]<IntPtr>)Functions.LoadFunction("glfwGetVersionString");
			Functions.glfwGetErrorPtr = (delegate* unmanaged[Cdecl]<IntPtr, int>)Functions.LoadFunction("glfwGetError");
			Functions.glfwSetErrorCallbackPtr = (delegate* unmanaged[Cdecl]<void*, void*>)Functions.LoadFunction("glfwSetErrorCallback");
			Functions.glfwGetMonitorsPtr = (delegate* unmanaged[Cdecl]<out int, IntPtr>)Functions.LoadFunction("glfwGetMonitors");
			Functions.glfwGetPrimaryMonitorPtr = (delegate* unmanaged[Cdecl]<IntPtr>)Functions.LoadFunction("glfwGetPrimaryMonitor");
			Functions.glfwGetMonitorPosPtr = (delegate* unmanaged[Cdecl]<IntPtr, out int, out int, void>)Functions.LoadFunction("glfwGetMonitorPos");
			Functions.glfwGetMonitorWorkareaPtr = (delegate* unmanaged[Cdecl]<IntPtr, out int, out int, out int, out int, void>)Functions.LoadFunction("glfwGetMonitorWorkarea");
			Functions.glfwGetMonitorPhysicalSizePtr = (delegate* unmanaged[Cdecl]<IntPtr, out int, out int, void>)Functions.LoadFunction("glfwGetMonitorPhysicalSize");
			Functions.glfwGetMonitorContentScalePtr = (delegate* unmanaged[Cdecl]<IntPtr, out IntPtr, out IntPtr, void>)Functions.LoadFunction("glfwGetMonitorContentScale");
			Functions.glfwGetMonitorNamePtr = (delegate* unmanaged[Cdecl]<IntPtr, IntPtr>)Functions.LoadFunction("glfwGetMonitorName");
			Functions.glfwSetMonitorUserPointerPtr = (delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void>)Functions.LoadFunction("glfwSetMonitorUserPointer");
			Functions.glfwGetMonitorUserPointerPtr = (delegate* unmanaged[Cdecl]<IntPtr, IntPtr>)Functions.LoadFunction("glfwGetMonitorUserPointer");
			Functions.glfwSetMonitorCallbackPtr = (delegate* unmanaged[Cdecl]<void*, void*>)Functions.LoadFunction("glfwSetMonitorCallback");
			Functions.glfwGetVideoModesPtr = (delegate* unmanaged[Cdecl]<IntPtr, out int, GLFWvidmode*>)Functions.LoadFunction("glfwGetVideoModes");
			Functions.glfwGetVideoModePtr = (delegate* unmanaged[Cdecl]<IntPtr, GLFWvidmode*>)Functions.LoadFunction("glfwGetVideoMode");
			Functions.glfwSetGammaPtr = (delegate* unmanaged[Cdecl]<IntPtr, float, void>)Functions.LoadFunction("glfwSetGamma");
			Functions.glfwGetGammaRampPtr = (delegate* unmanaged[Cdecl]<IntPtr, IntPtr>)Functions.LoadFunction("glfwGetGammaRamp");
			Functions.glfwSetGammaRampPtr = (delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void>)Functions.LoadFunction("glfwSetGammaRamp");
			Functions.glfwDefaultWindowHintsPtr = (delegate* unmanaged[Cdecl]<void>)Functions.LoadFunction("glfwDefaultWindowHints");
			Functions.glfwWindowHintPtr = (delegate* unmanaged[Cdecl]<int, int, void>)Functions.LoadFunction("glfwWindowHint");
			Functions.glfwWindowHintStringPtr = (delegate* unmanaged[Cdecl]<int, byte*, void>)Functions.LoadFunction("glfwWindowHintString");
			Functions.glfwCreateWindowPtr = (delegate* unmanaged[Cdecl]<int, int, byte*, IntPtr, IntPtr, IntPtr>)Functions.LoadFunction("glfwCreateWindow");
			Functions.glfwDestroyWindowPtr = (delegate* unmanaged[Cdecl]<IntPtr, void>)Functions.LoadFunction("glfwDestroyWindow");
			Functions.glfwWindowShouldClosePtr = (delegate* unmanaged[Cdecl]<IntPtr, int>)Functions.LoadFunction("glfwWindowShouldClose");
			Functions.glfwSetWindowShouldClosePtr = (delegate* unmanaged[Cdecl]<IntPtr, int, void>)Functions.LoadFunction("glfwSetWindowShouldClose");
			Functions.glfwSetWindowTitlePtr = (delegate* unmanaged[Cdecl]<IntPtr, byte*, void>)Functions.LoadFunction("glfwSetWindowTitle");
			Functions.glfwSetWindowIconPtr = (delegate* unmanaged[Cdecl]<IntPtr, int, IntPtr, void>)Functions.LoadFunction("glfwSetWindowIcon");
			Functions.glfwGetWindowPosPtr = (delegate* unmanaged[Cdecl]<IntPtr, out int, out int, void>)Functions.LoadFunction("glfwGetWindowPos");
			Functions.glfwSetWindowPosPtr = (delegate* unmanaged[Cdecl]<IntPtr, int, int, void>)Functions.LoadFunction("glfwSetWindowPos");
			Functions.glfwGetWindowSizePtr = (delegate* unmanaged[Cdecl]<IntPtr, out int, out int, void>)Functions.LoadFunction("glfwGetWindowSize");
			Functions.glfwSetWindowSizeLimitsPtr = (delegate* unmanaged[Cdecl]<IntPtr, int, int, int, int, void>)Functions.LoadFunction("glfwSetWindowSizeLimits");
			Functions.glfwSetWindowAspectRatioPtr = (delegate* unmanaged[Cdecl]<IntPtr, int, int, void>)Functions.LoadFunction("glfwSetWindowAspectRatio");
			Functions.glfwSetWindowSizePtr = (delegate* unmanaged[Cdecl]<IntPtr, int, int, void>)Functions.LoadFunction("glfwSetWindowSize");
			Functions.glfwGetFramebufferSizePtr = (delegate* unmanaged[Cdecl]<IntPtr, out int, out int, void>)Functions.LoadFunction("glfwGetFramebufferSize");
			Functions.glfwGetWindowFrameSizePtr = (delegate* unmanaged[Cdecl]<IntPtr, out int, out int, out int, out int, void>)Functions.LoadFunction("glfwGetWindowFrameSize");
			Functions.glfwGetWindowContentScalePtr = (delegate* unmanaged[Cdecl]<IntPtr, out IntPtr, out IntPtr, void>)Functions.LoadFunction("glfwGetWindowContentScale");
			Functions.glfwGetWindowOpacityPtr = (delegate* unmanaged[Cdecl]<IntPtr, float>)Functions.LoadFunction("glfwGetWindowOpacity");
			Functions.glfwSetWindowOpacityPtr = (delegate* unmanaged[Cdecl]<IntPtr, float, void>)Functions.LoadFunction("glfwSetWindowOpacity");
			Functions.glfwIconifyWindowPtr = (delegate* unmanaged[Cdecl]<IntPtr, void>)Functions.LoadFunction("glfwIconifyWindow");
			Functions.glfwRestoreWindowPtr = (delegate* unmanaged[Cdecl]<IntPtr, void>)Functions.LoadFunction("glfwRestoreWindow");
			Functions.glfwMaximizeWindowPtr = (delegate* unmanaged[Cdecl]<IntPtr, void>)Functions.LoadFunction("glfwMaximizeWindow");
			Functions.glfwShowWindowPtr = (delegate* unmanaged[Cdecl]<IntPtr, void>)Functions.LoadFunction("glfwShowWindow");
			Functions.glfwHideWindowPtr = (delegate* unmanaged[Cdecl]<IntPtr, void>)Functions.LoadFunction("glfwHideWindow");
			Functions.glfwFocusWindowPtr = (delegate* unmanaged[Cdecl]<IntPtr, void>)Functions.LoadFunction("glfwFocusWindow");
			Functions.glfwRequestWindowAttentionPtr = (delegate* unmanaged[Cdecl]<IntPtr, void>)Functions.LoadFunction("glfwRequestWindowAttention");
			Functions.glfwGetWindowMonitorPtr = (delegate* unmanaged[Cdecl]<IntPtr, IntPtr>)Functions.LoadFunction("glfwGetWindowMonitor");
			Functions.glfwSetWindowMonitorPtr = (delegate* unmanaged[Cdecl]<IntPtr, IntPtr, int, int, int, int, int, void>)Functions.LoadFunction("glfwSetWindowMonitor");
			Functions.glfwGetWindowAttribPtr = (delegate* unmanaged[Cdecl]<IntPtr, int, int>)Functions.LoadFunction("glfwGetWindowAttrib");
			Functions.glfwSetWindowAttribPtr = (delegate* unmanaged[Cdecl]<IntPtr, int, int, void>)Functions.LoadFunction("glfwSetWindowAttrib");
			Functions.glfwSetWindowUserPointerPtr = (delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void>)Functions.LoadFunction("glfwSetWindowUserPointer");
			Functions.glfwGetWindowUserPointerPtr = (delegate* unmanaged[Cdecl]<IntPtr, IntPtr>)Functions.LoadFunction("glfwGetWindowUserPointer");
			Functions.glfwSetWindowPosCallbackPtr = (delegate* unmanaged[Cdecl]<IntPtr, void*, void*>)Functions.LoadFunction("glfwSetWindowPosCallback");
			Functions.glfwSetWindowSizeCallbackPtr = (delegate* unmanaged[Cdecl]<IntPtr, void*, void*>)Functions.LoadFunction("glfwSetWindowSizeCallback");
			Functions.glfwSetWindowCloseCallbackPtr = (delegate* unmanaged[Cdecl]<IntPtr, void*, void*>)Functions.LoadFunction("glfwSetWindowCloseCallback");
			Functions.glfwSetWindowRefreshCallbackPtr = (delegate* unmanaged[Cdecl]<IntPtr, void*, void*>)Functions.LoadFunction("glfwSetWindowRefreshCallback");
			Functions.glfwSetWindowFocusCallbackPtr = (delegate* unmanaged[Cdecl]<IntPtr, void*, void*>)Functions.LoadFunction("glfwSetWindowFocusCallback");
			Functions.glfwSetWindowIconifyCallbackPtr = (delegate* unmanaged[Cdecl]<IntPtr, void*, void*>)Functions.LoadFunction("glfwSetWindowIconifyCallback");
			Functions.glfwSetWindowMaximizeCallbackPtr = (delegate* unmanaged[Cdecl]<IntPtr, void*, void*>)Functions.LoadFunction("glfwSetWindowMaximizeCallback");
			Functions.glfwSetFramebufferSizeCallbackPtr = (delegate* unmanaged[Cdecl]<IntPtr, void*, void*>)Functions.LoadFunction("glfwSetFramebufferSizeCallback");
			Functions.glfwSetWindowContentScaleCallbackPtr = (delegate* unmanaged[Cdecl]<IntPtr, void*, void*>)Functions.LoadFunction("glfwSetWindowContentScaleCallback");
			Functions.glfwPollEventsPtr = (delegate* unmanaged[Cdecl]<void>)Functions.LoadFunction("glfwPollEvents");
			Functions.glfwWaitEventsPtr = (delegate* unmanaged[Cdecl]<void>)Functions.LoadFunction("glfwWaitEvents");
			Functions.glfwWaitEventsTimeoutPtr = (delegate* unmanaged[Cdecl]<double, void>)Functions.LoadFunction("glfwWaitEventsTimeout");
			Functions.glfwPostEmptyEventPtr = (delegate* unmanaged[Cdecl]<void>)Functions.LoadFunction("glfwPostEmptyEvent");
			Functions.glfwGetInputModePtr = (delegate* unmanaged[Cdecl]<IntPtr, int, int>)Functions.LoadFunction("glfwGetInputMode");
			Functions.glfwSetInputModePtr = (delegate* unmanaged[Cdecl]<IntPtr, int, int, void>)Functions.LoadFunction("glfwSetInputMode");
			Functions.glfwRawMouseMotionSupportedPtr = (delegate* unmanaged[Cdecl]<int>)Functions.LoadFunction("glfwRawMouseMotionSupported");
			Functions.glfwGetKeyNamePtr = (delegate* unmanaged[Cdecl]<int, int, IntPtr>)Functions.LoadFunction("glfwGetKeyName");
			Functions.glfwGetKeyScancodePtr = (delegate* unmanaged[Cdecl]<int, int>)Functions.LoadFunction("glfwGetKeyScancode");
			Functions.glfwGetKeyPtr = (delegate* unmanaged[Cdecl]<IntPtr, int, int>)Functions.LoadFunction("glfwGetKey");
			Functions.glfwGetMouseButtonPtr = (delegate* unmanaged[Cdecl]<IntPtr, int, int>)Functions.LoadFunction("glfwGetMouseButton");
			Functions.glfwGetCursorPosPtr = (delegate* unmanaged[Cdecl]<IntPtr, out double, out double, void>)Functions.LoadFunction("glfwGetCursorPos");
			Functions.glfwSetCursorPosPtr = (delegate* unmanaged[Cdecl]<IntPtr, double, double, void>)Functions.LoadFunction("glfwSetCursorPos");
			Functions.glfwCreateCursorPtr = (delegate* unmanaged[Cdecl]<IntPtr, int, int, IntPtr>)Functions.LoadFunction("glfwCreateCursor");
			Functions.glfwCreateStandardCursorPtr = (delegate* unmanaged[Cdecl]<int, IntPtr>)Functions.LoadFunction("glfwCreateStandardCursor");
			Functions.glfwDestroyCursorPtr = (delegate* unmanaged[Cdecl]<IntPtr, void>)Functions.LoadFunction("glfwDestroyCursor");
			Functions.glfwSetCursorPtr = (delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void>)Functions.LoadFunction("glfwSetCursor");
			Functions.glfwSetKeyCallbackPtr = (delegate* unmanaged[Cdecl]<IntPtr, void*, void*>)Functions.LoadFunction("glfwSetKeyCallback");
			Functions.glfwSetCharCallbackPtr = (delegate* unmanaged[Cdecl]<IntPtr, void*, void*>)Functions.LoadFunction("glfwSetCharCallback");
			Functions.glfwSetCharModsCallbackPtr = (delegate* unmanaged[Cdecl]<IntPtr, void*, void*>)Functions.LoadFunction("glfwSetCharModsCallback");
			Functions.glfwSetMouseButtonCallbackPtr = (delegate* unmanaged[Cdecl]<IntPtr, void*, void*>)Functions.LoadFunction("glfwSetMouseButtonCallback");
			Functions.glfwSetCursorPosCallbackPtr = (delegate* unmanaged[Cdecl]<IntPtr, void*, void*>)Functions.LoadFunction("glfwSetCursorPosCallback");
			Functions.glfwSetCursorEnterCallbackPtr = (delegate* unmanaged[Cdecl]<IntPtr, void*, void*>)Functions.LoadFunction("glfwSetCursorEnterCallback");
			Functions.glfwSetScrollCallbackPtr = (delegate* unmanaged[Cdecl]<IntPtr, void*, void*>)Functions.LoadFunction("glfwSetScrollCallback");
			Functions.glfwSetDropCallbackPtr = (delegate* unmanaged[Cdecl]<IntPtr, void*, void*>)Functions.LoadFunction("glfwSetDropCallback");
			Functions.glfwJoystickPresentPtr = (delegate* unmanaged[Cdecl]<int, int>)Functions.LoadFunction("glfwJoystickPresent");
			Functions.glfwGetJoystickAxesPtr = (delegate* unmanaged[Cdecl]<int, out int, IntPtr>)Functions.LoadFunction("glfwGetJoystickAxes");
			Functions.glfwGetJoystickButtonsPtr = (delegate* unmanaged[Cdecl]<int, out int, IntPtr>)Functions.LoadFunction("glfwGetJoystickButtons");
			Functions.glfwGetJoystickHatsPtr = (delegate* unmanaged[Cdecl]<int, out int, IntPtr>)Functions.LoadFunction("glfwGetJoystickHats");
			Functions.glfwGetJoystickNamePtr = (delegate* unmanaged[Cdecl]<int, IntPtr>)Functions.LoadFunction("glfwGetJoystickName");
			Functions.glfwGetJoystickGUIDPtr = (delegate* unmanaged[Cdecl]<int, IntPtr>)Functions.LoadFunction("glfwGetJoystickGUID");
			Functions.glfwSetJoystickUserPointerPtr = (delegate* unmanaged[Cdecl]<int, IntPtr, void>)Functions.LoadFunction("glfwSetJoystickUserPointer");
			Functions.glfwGetJoystickUserPointerPtr = (delegate* unmanaged[Cdecl]<int, IntPtr>)Functions.LoadFunction("glfwGetJoystickUserPointer");
			Functions.glfwJoystickIsGamepadPtr = (delegate* unmanaged[Cdecl]<int, int>)Functions.LoadFunction("glfwJoystickIsGamepad");
			Functions.glfwSetJoystickCallbackPtr = (delegate* unmanaged[Cdecl]<void*, void*>)Functions.LoadFunction("glfwSetJoystickCallback");
			Functions.glfwUpdateGamepadMappingsPtr = (delegate* unmanaged[Cdecl]<byte*, int>)Functions.LoadFunction("glfwUpdateGamepadMappings");
			Functions.glfwGetGamepadNamePtr = (delegate* unmanaged[Cdecl]<int, IntPtr>)Functions.LoadFunction("glfwGetGamepadName");
			Functions.glfwGetGamepadStatePtr = (delegate* unmanaged[Cdecl]<int, out IntPtr, int>)Functions.LoadFunction("glfwGetGamepadState");
			Functions.glfwSetClipboardStringPtr = (delegate* unmanaged[Cdecl]<IntPtr, byte*, void>)Functions.LoadFunction("glfwSetClipboardString");
			Functions.glfwGetClipboardStringPtr = (delegate* unmanaged[Cdecl]<IntPtr, IntPtr>)Functions.LoadFunction("glfwGetClipboardString");
			Functions.glfwGetTimePtr = (delegate* unmanaged[Cdecl]<double>)Functions.LoadFunction("glfwGetTime");
			Functions.glfwSetTimePtr = (delegate* unmanaged[Cdecl]<double, void>)Functions.LoadFunction("glfwSetTime");
			Functions.glfwGetTimerValuePtr = (delegate* unmanaged[Cdecl]<ulong>)Functions.LoadFunction("glfwGetTimerValue");
			Functions.glfwGetTimerFrequencyPtr = (delegate* unmanaged[Cdecl]<ulong>)Functions.LoadFunction("glfwGetTimerFrequency");
			Functions.glfwMakeContextCurrentPtr = (delegate* unmanaged[Cdecl]<IntPtr, void>)Functions.LoadFunction("glfwMakeContextCurrent");
			Functions.glfwGetCurrentContextPtr = (delegate* unmanaged[Cdecl]<IntPtr>)Functions.LoadFunction("glfwGetCurrentContext");
			Functions.glfwSwapBuffersPtr = (delegate* unmanaged[Cdecl]<IntPtr, void>)Functions.LoadFunction("glfwSwapBuffers");
			Functions.glfwSwapIntervalPtr = (delegate* unmanaged[Cdecl]<int, void>)Functions.LoadFunction("glfwSwapInterval");
			Functions.glfwExtensionSupportedPtr = (delegate* unmanaged[Cdecl]<byte*, int>)Functions.LoadFunction("glfwExtensionSupported");
			Functions.glfwGetProcAddressPtr = (delegate* unmanaged[Cdecl]<byte*, IntPtr>)Functions.LoadFunction("glfwGetProcAddress");
			Functions.glfwVulkanSupportedPtr = (delegate* unmanaged[Cdecl]<int>)Functions.LoadFunction("glfwVulkanSupported");
			Functions.glfwGetRequiredInstanceExtensionsPtr = (delegate* unmanaged[Cdecl]<out uint, IntPtr>)Functions.LoadFunction("glfwGetRequiredInstanceExtensions");
			Functions.glfwGetInstanceProcAddressPtr = (delegate* unmanaged[Cdecl]<IntPtr, byte*, IntPtr>)Functions.LoadFunction("glfwGetInstanceProcAddress");
			Functions.glfwGetPhysicalDevicePresentationSupportPtr = (delegate* unmanaged[Cdecl]<IntPtr, IntPtr, uint, int>)Functions.LoadFunction("glfwGetPhysicalDevicePresentationSupport");
			Functions.glfwCreateWindowSurfacePtr = (delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, out IntPtr, int>)Functions.LoadFunction("glfwCreateWindowSurface");
			Functions.glfwGetWin32WindowPtr = (delegate* unmanaged[Cdecl]<IntPtr, IntPtr>)Functions.LoadFunction("glfwGetWin32Window");
			Functions.glfwGetX11WindowPtr = (delegate* unmanaged[Cdecl]<IntPtr, IntPtr>)Functions.LoadFunction("glfwGetX11Window");
			Functions.glfwGetCocoaWindowPtr = (delegate* unmanaged[Cdecl]<IntPtr, IntPtr>)Functions.LoadFunction("glfwGetCocoaWindow");
		}

		private static void* LoadFunction(string name) => Functions.loader.LoadFunction(name).ToPointer();


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int glfwInit()
		{
			return Functions.glfwInitPtr();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwTerminate()
		{
			Functions.glfwTerminatePtr();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwInitHint(int hint, int value)
		{
			Functions.glfwInitHintPtr(hint, value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwGetVersion(out int major, out int minor, out int rev)
		{
			Functions.glfwGetVersionPtr(out major, out minor, out rev);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr glfwGetVersionString()
		{
			return Functions.glfwGetVersionStringPtr();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int glfwGetError(IntPtr description)
		{
			return Functions.glfwGetErrorPtr(description);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GLFWerrorfun glfwSetErrorCallback(GLFWerrorfun cbfun)
		{
			var prefun = (IntPtr)Functions.glfwSetErrorCallbackPtr((cbfun != null) ? Marshal.GetFunctionPointerForDelegate(cbfun).ToPointer() : (void*)0);
			return prefun == IntPtr.Zero ? null : Marshal.GetDelegateForFunctionPointer<GLFWerrorfun>(prefun);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr glfwGetMonitors(out int count)
		{
			return Functions.glfwGetMonitorsPtr(out count);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr glfwGetPrimaryMonitor()
		{
			return Functions.glfwGetPrimaryMonitorPtr();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwGetMonitorPos(IntPtr monitor, out int xpos, out int ypos)
		{
			Functions.glfwGetMonitorPosPtr(monitor, out xpos, out ypos);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwGetMonitorWorkarea(IntPtr monitor, out int xpos, out int ypos, out int width, out int height)
		{
			Functions.glfwGetMonitorWorkareaPtr(monitor, out xpos, out ypos, out width, out height);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwGetMonitorPhysicalSize(IntPtr monitor, out int widthMM, out int heightMM)
		{
			Functions.glfwGetMonitorPhysicalSizePtr(monitor, out widthMM, out heightMM);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwGetMonitorContentScale(IntPtr monitor, out IntPtr xscale, out IntPtr yscale)
		{
			Functions.glfwGetMonitorContentScalePtr(monitor, out xscale, out yscale);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr glfwGetMonitorName(IntPtr monitor)
		{
			return Functions.glfwGetMonitorNamePtr(monitor);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwSetMonitorUserPointer(IntPtr monitor, IntPtr pointer)
		{
			Functions.glfwSetMonitorUserPointerPtr(monitor, pointer);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr glfwGetMonitorUserPointer(IntPtr monitor)
		{
			return Functions.glfwGetMonitorUserPointerPtr(monitor);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void* glfwSetMonitorCallback(void* cbfun)
		{
			return Functions.glfwSetMonitorCallbackPtr(cbfun);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GLFWvidmode* glfwGetVideoModes(IntPtr monitor, out int count)
		{
			return Functions.glfwGetVideoModesPtr(monitor, out count);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GLFWvidmode* glfwGetVideoMode(IntPtr monitor)
		{
			return Functions.glfwGetVideoModePtr(monitor);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwSetGamma(IntPtr monitor, float gamma)
		{
			Functions.glfwSetGammaPtr(monitor, gamma);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr glfwGetGammaRamp(IntPtr monitor)
		{
			return Functions.glfwGetGammaRampPtr(monitor);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwSetGammaRamp(IntPtr monitor, IntPtr ramp)
		{
			Functions.glfwSetGammaRampPtr(monitor, ramp);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwDefaultWindowHints()
		{
			Functions.glfwDefaultWindowHintsPtr();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwWindowHint(int hint, int value)
		{
			Functions.glfwWindowHintPtr(hint, value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwWindowHintString(int hint, byte* value)
		{
			Functions.glfwWindowHintStringPtr(hint, value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr glfwCreateWindow(int width, int height, byte* title, IntPtr monitor, IntPtr share)
		{
			return Functions.glfwCreateWindowPtr(width, height, title, monitor, share);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwDestroyWindow(IntPtr window)
		{
			Functions.glfwDestroyWindowPtr(window);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int glfwWindowShouldClose(IntPtr window)
		{
			return Functions.glfwWindowShouldClosePtr(window);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwSetWindowShouldClose(IntPtr window, int value)
		{
			Functions.glfwSetWindowShouldClosePtr(window, value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwSetWindowTitle(IntPtr window, byte* title)
		{
			Functions.glfwSetWindowTitlePtr(window, title);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwSetWindowIcon(IntPtr window, int count, IntPtr images)
		{
			Functions.glfwSetWindowIconPtr(window, count, images);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwGetWindowPos(IntPtr window, out int xpos, out int ypos)
		{
			Functions.glfwGetWindowPosPtr(window, out xpos, out ypos);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwSetWindowPos(IntPtr window, int xpos, int ypos)
		{
			Functions.glfwSetWindowPosPtr(window, xpos, ypos);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwGetWindowSize(IntPtr window, out int width, out int height)
		{
			Functions.glfwGetWindowSizePtr(window, out width, out height);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwSetWindowSizeLimits(IntPtr window, int minwidth, int minheight, int maxwidth, int maxheight)
		{
			Functions.glfwSetWindowSizeLimitsPtr(window, minwidth, minheight, maxwidth, maxheight);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwSetWindowAspectRatio(IntPtr window, int numer, int denom)
		{
			Functions.glfwSetWindowAspectRatioPtr(window, numer, denom);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwSetWindowSize(IntPtr window, int width, int height)
		{
			Functions.glfwSetWindowSizePtr(window, width, height);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwGetFramebufferSize(IntPtr window, out int width, out int height)
		{
			Functions.glfwGetFramebufferSizePtr(window, out width, out height);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwGetWindowFrameSize(IntPtr window, out int left, out int top, out int right, out int bottom)
		{
			Functions.glfwGetWindowFrameSizePtr(window, out left, out top, out right, out bottom);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwGetWindowContentScale(IntPtr window, out IntPtr xscale, out IntPtr yscale)
		{
			Functions.glfwGetWindowContentScalePtr(window, out xscale, out yscale);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float glfwGetWindowOpacity(IntPtr window)
		{
			return Functions.glfwGetWindowOpacityPtr(window);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwSetWindowOpacity(IntPtr window, float opacity)
		{
			Functions.glfwSetWindowOpacityPtr(window, opacity);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwIconifyWindow(IntPtr window)
		{
			Functions.glfwIconifyWindowPtr(window);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwRestoreWindow(IntPtr window)
		{
			Functions.glfwRestoreWindowPtr(window);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwMaximizeWindow(IntPtr window)
		{
			Functions.glfwMaximizeWindowPtr(window);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwShowWindow(IntPtr window)
		{
			Functions.glfwShowWindowPtr(window);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwHideWindow(IntPtr window)
		{
			Functions.glfwHideWindowPtr(window);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwFocusWindow(IntPtr window)
		{
			Functions.glfwFocusWindowPtr(window);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwRequestWindowAttention(IntPtr window)
		{
			Functions.glfwRequestWindowAttentionPtr(window);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr glfwGetWindowMonitor(IntPtr window)
		{
			return Functions.glfwGetWindowMonitorPtr(window);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwSetWindowMonitor(IntPtr window, IntPtr monitor, int xpos, int ypos, int width, int height, int refreshRate)
		{
			Functions.glfwSetWindowMonitorPtr(window, monitor, xpos, ypos, width, height, refreshRate);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int glfwGetWindowAttrib(IntPtr window, int attrib)
		{
			return Functions.glfwGetWindowAttribPtr(window, attrib);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwSetWindowAttrib(IntPtr window, int attrib, int value)
		{
			Functions.glfwSetWindowAttribPtr(window, attrib, value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwSetWindowUserPointer(IntPtr window, IntPtr pointer)
		{
			Functions.glfwSetWindowUserPointerPtr(window, pointer);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr glfwGetWindowUserPointer(IntPtr window)
		{
			return Functions.glfwGetWindowUserPointerPtr(window);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GLFWwindowposfun glfwSetWindowPosCallback(IntPtr window, GLFWwindowposfun cbfun)
		{
			var prefun = (IntPtr)Functions.glfwSetWindowPosCallbackPtr(window, (cbfun != null) ? Marshal.GetFunctionPointerForDelegate(cbfun).ToPointer() : (void*)0);
			return prefun == IntPtr.Zero ? null : Marshal.GetDelegateForFunctionPointer<GLFWwindowposfun>(prefun);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GLFWwindowsizefun glfwSetWindowSizeCallback(IntPtr window, GLFWwindowsizefun cbfun)
		{
			var prefun = (IntPtr)Functions.glfwSetWindowSizeCallbackPtr(window, (cbfun != null) ? Marshal.GetFunctionPointerForDelegate(cbfun).ToPointer() : (void*)0);
			return prefun == IntPtr.Zero ? null : Marshal.GetDelegateForFunctionPointer<GLFWwindowsizefun>(prefun);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GLFWwindowclosefun glfwSetWindowCloseCallback(IntPtr window, GLFWwindowclosefun cbfun)
		{
			var prefun = (IntPtr)Functions.glfwSetWindowCloseCallbackPtr(window, (cbfun != null) ? Marshal.GetFunctionPointerForDelegate(cbfun).ToPointer() : (void*)0);
			return prefun == IntPtr.Zero ? null : Marshal.GetDelegateForFunctionPointer<GLFWwindowclosefun>(prefun);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GLFWwindowrefreshfun glfwSetWindowRefreshCallback(IntPtr window, GLFWwindowrefreshfun cbfun)
		{
			var prefun = (IntPtr)Functions.glfwSetWindowRefreshCallbackPtr(window, (cbfun != null) ? Marshal.GetFunctionPointerForDelegate(cbfun).ToPointer() : (void*)0);
			return prefun == IntPtr.Zero ? null : Marshal.GetDelegateForFunctionPointer<GLFWwindowrefreshfun>(prefun);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GLFWwindowfocusfun glfwSetWindowFocusCallback(IntPtr window, GLFWwindowfocusfun cbfun)
		{
			var prefun = (IntPtr)Functions.glfwSetWindowFocusCallbackPtr(window, (cbfun != null) ? Marshal.GetFunctionPointerForDelegate(cbfun).ToPointer() : (void*)0);
			return prefun == IntPtr.Zero ? null : Marshal.GetDelegateForFunctionPointer<GLFWwindowfocusfun>(prefun);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GLFWwindowiconifyfun glfwSetWindowIconifyCallback(IntPtr window, GLFWwindowiconifyfun cbfun)
		{
			var prefun = (IntPtr)Functions.glfwSetWindowIconifyCallbackPtr(window, (cbfun != null) ? Marshal.GetFunctionPointerForDelegate(cbfun).ToPointer() : (void*)0);
			return prefun == IntPtr.Zero ? null : Marshal.GetDelegateForFunctionPointer<GLFWwindowiconifyfun>(prefun);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GLFWwindowmaximizefun glfwSetWindowMaximizeCallback(IntPtr window, GLFWwindowmaximizefun cbfun)
		{
			var prefun = (IntPtr)Functions.glfwSetWindowMaximizeCallbackPtr(window, (cbfun != null) ? Marshal.GetFunctionPointerForDelegate(cbfun).ToPointer() : (void*)0);
			return prefun == IntPtr.Zero ? null : Marshal.GetDelegateForFunctionPointer<GLFWwindowmaximizefun>(prefun);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GLFWframebuffersizefun glfwSetFramebufferSizeCallback(IntPtr window, GLFWframebuffersizefun cbfun)
		{
			var prefun = (IntPtr)Functions.glfwSetFramebufferSizeCallbackPtr(window, (cbfun != null) ? Marshal.GetFunctionPointerForDelegate(cbfun).ToPointer() : (void*)0);
			return prefun == IntPtr.Zero ? null : Marshal.GetDelegateForFunctionPointer<GLFWframebuffersizefun>(prefun);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GLFWwindowcontentscalefun glfwSetWindowContentScaleCallback(IntPtr window, GLFWwindowcontentscalefun cbfun)
		{
			var prefun = (IntPtr)Functions.glfwSetWindowContentScaleCallbackPtr(window, (cbfun != null) ? Marshal.GetFunctionPointerForDelegate(cbfun).ToPointer() : (void*)0);
			return prefun == IntPtr.Zero ? null : Marshal.GetDelegateForFunctionPointer<GLFWwindowcontentscalefun>(prefun);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwPollEvents()
		{
			Functions.glfwPollEventsPtr();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwWaitEvents()
		{
			Functions.glfwWaitEventsPtr();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwWaitEventsTimeout(double timeout)
		{
			Functions.glfwWaitEventsTimeoutPtr(timeout);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwPostEmptyEvent()
		{
			Functions.glfwPostEmptyEventPtr();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int glfwGetInputMode(IntPtr window, int mode)
		{
			return Functions.glfwGetInputModePtr(window, mode);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwSetInputMode(IntPtr window, int mode, int value)
		{
			Functions.glfwSetInputModePtr(window, mode, value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int glfwRawMouseMotionSupported()
		{
			return Functions.glfwRawMouseMotionSupportedPtr();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr glfwGetKeyName(int key, int scancode)
		{
			return Functions.glfwGetKeyNamePtr(key, scancode);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int glfwGetKeyScancode(int key)
		{
			return Functions.glfwGetKeyScancodePtr(key);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int glfwGetKey(IntPtr window, int key)
		{
			return Functions.glfwGetKeyPtr(window, key);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int glfwGetMouseButton(IntPtr window, int button)
		{
			return Functions.glfwGetMouseButtonPtr(window, button);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwGetCursorPos(IntPtr window, out double xpos, out double ypos)
		{
			Functions.glfwGetCursorPosPtr(window, out xpos, out ypos);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwSetCursorPos(IntPtr window, double xpos, double ypos)
		{
			Functions.glfwSetCursorPosPtr(window, xpos, ypos);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr glfwCreateCursor(IntPtr image, int xhot, int yhot)
		{
			return Functions.glfwCreateCursorPtr(image, xhot, yhot);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr glfwCreateStandardCursor(int shape)
		{
			return Functions.glfwCreateStandardCursorPtr(shape);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwDestroyCursor(IntPtr cursor)
		{
			Functions.glfwDestroyCursorPtr(cursor);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwSetCursor(IntPtr window, IntPtr cursor)
		{
			Functions.glfwSetCursorPtr(window, cursor);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GLFWkeyfun glfwSetKeyCallback(IntPtr window, GLFWkeyfun cbfun)
		{
			var prefun = (IntPtr)Functions.glfwSetKeyCallbackPtr(window, (cbfun != null) ? Marshal.GetFunctionPointerForDelegate(cbfun).ToPointer() : (void*)0);
			return prefun == IntPtr.Zero ? null : (prefun == IntPtr.Zero) ? null : Marshal.GetDelegateForFunctionPointer<GLFWkeyfun>(prefun);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GLFWcharfun glfwSetCharCallback(IntPtr window, GLFWcharfun cbfun)
		{
			var prefun = (IntPtr)Functions.glfwSetCharCallbackPtr(window, (cbfun != null) ? Marshal.GetFunctionPointerForDelegate(cbfun).ToPointer() : (void*)0);
			return prefun == IntPtr.Zero ? null : Marshal.GetDelegateForFunctionPointer<GLFWcharfun>(prefun);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GLFWcharmodsfun glfwSetCharModsCallback(IntPtr window, GLFWcharmodsfun cbfun)
		{
			var prefun = (IntPtr)Functions.glfwSetCharModsCallbackPtr(window, (cbfun != null) ? Marshal.GetFunctionPointerForDelegate(cbfun).ToPointer() : (void*)0);
			return Marshal.GetDelegateForFunctionPointer<GLFWcharmodsfun>(prefun);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GLFWmousebuttonfun glfwSetMouseButtonCallback(IntPtr window, GLFWmousebuttonfun cbfun)
		{
			var prefun = (IntPtr)Functions.glfwSetMouseButtonCallbackPtr(window, (cbfun != null) ? Marshal.GetFunctionPointerForDelegate(cbfun).ToPointer() : (void*)0);
			return prefun == IntPtr.Zero ? null : Marshal.GetDelegateForFunctionPointer<GLFWmousebuttonfun>(prefun);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GLFWcursorposfun glfwSetCursorPosCallback(IntPtr window, GLFWcursorposfun cbfun)
		{
			var prefun = (IntPtr)Functions.glfwSetCursorPosCallbackPtr(window, (cbfun != null) ? Marshal.GetFunctionPointerForDelegate(cbfun).ToPointer() : (void*)0);
			return prefun == IntPtr.Zero ? null : Marshal.GetDelegateForFunctionPointer<GLFWcursorposfun>(prefun);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GLFWcursorenterfun glfwSetCursorEnterCallback(IntPtr window, GLFWcursorenterfun cbfun)
		{
			var prefun = (IntPtr)Functions.glfwSetCursorEnterCallbackPtr(window, (cbfun != null) ? Marshal.GetFunctionPointerForDelegate(cbfun).ToPointer() : (void*)0);
			return prefun == IntPtr.Zero ? null : Marshal.GetDelegateForFunctionPointer<GLFWcursorenterfun>(prefun);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GLFWscrollfun glfwSetScrollCallback(IntPtr window, GLFWscrollfun cbfun)
		{
			var prefun = (IntPtr)Functions.glfwSetScrollCallbackPtr(window, (cbfun != null) ? Marshal.GetFunctionPointerForDelegate(cbfun).ToPointer() : (void*)0);
			return prefun == IntPtr.Zero ? null : Marshal.GetDelegateForFunctionPointer<GLFWscrollfun>(prefun);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GLFWdropfun glfwSetDropCallback(IntPtr window, GLFWdropfun cbfun)
		{
			var prefun = (IntPtr)Functions.glfwSetDropCallbackPtr(window, (cbfun != null) ? Marshal.GetFunctionPointerForDelegate(cbfun).ToPointer() : (void*)0);
			return Marshal.GetDelegateForFunctionPointer<GLFWdropfun>(prefun);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int glfwJoystickPresent(int jid)
		{
			return Functions.glfwJoystickPresentPtr(jid);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr glfwGetJoystickAxes(int jid, out int count)
		{
			return Functions.glfwGetJoystickAxesPtr(jid, out count);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr glfwGetJoystickButtons(int jid, out int count)
		{
			return Functions.glfwGetJoystickButtonsPtr(jid, out count);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr glfwGetJoystickHats(int jid, out int count)
		{
			return Functions.glfwGetJoystickHatsPtr(jid, out count);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr glfwGetJoystickName(int jid)
		{
			return Functions.glfwGetJoystickNamePtr(jid);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr glfwGetJoystickGUID(int jid)
		{
			return Functions.glfwGetJoystickGUIDPtr(jid);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwSetJoystickUserPointer(int jid, IntPtr pointer)
		{
			Functions.glfwSetJoystickUserPointerPtr(jid, pointer);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr glfwGetJoystickUserPointer(int jid)
		{
			return Functions.glfwGetJoystickUserPointerPtr(jid);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int glfwJoystickIsGamepad(int jid)
		{
			return Functions.glfwJoystickIsGamepadPtr(jid);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void* glfwSetJoystickCallback(void* cbfun)
		{
			return Functions.glfwSetJoystickCallbackPtr(cbfun);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int glfwUpdateGamepadMappings(byte* @string)
		{
			return Functions.glfwUpdateGamepadMappingsPtr(@string);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr glfwGetGamepadName(int jid)
		{
			return Functions.glfwGetGamepadNamePtr(jid);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int glfwGetGamepadState(int jid, out IntPtr state)
		{
			return Functions.glfwGetGamepadStatePtr(jid, out state);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwSetClipboardString(IntPtr window, byte* @string)
		{
			Functions.glfwSetClipboardStringPtr(window, @string);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr glfwGetClipboardString(IntPtr window)
		{
			return Functions.glfwGetClipboardStringPtr(window);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double glfwGetTime()
		{
			return Functions.glfwGetTimePtr();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwSetTime(double time)
		{
			Functions.glfwSetTimePtr(time);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong glfwGetTimerValue()
		{
			return Functions.glfwGetTimerValuePtr();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong glfwGetTimerFrequency()
		{
			return Functions.glfwGetTimerFrequencyPtr();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwMakeContextCurrent(IntPtr window)
		{
			Functions.glfwMakeContextCurrentPtr(window);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr glfwGetCurrentContext()
		{
			return Functions.glfwGetCurrentContextPtr();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwSwapBuffers(IntPtr window)
		{
			Functions.glfwSwapBuffersPtr(window);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void glfwSwapInterval(int interval)
		{
			Functions.glfwSwapIntervalPtr(interval);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int glfwExtensionSupported(byte* extension)
		{
			return Functions.glfwExtensionSupportedPtr(extension);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr glfwGetProcAddress(byte* procname)
		{
			return Functions.glfwGetProcAddressPtr(procname);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int glfwVulkanSupported()
		{
			return Functions.glfwVulkanSupportedPtr();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr glfwGetRequiredInstanceExtensions(out uint count)
		{
			return Functions.glfwGetRequiredInstanceExtensionsPtr(out count);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr glfwGetInstanceProcAddress(IntPtr instance, byte* procname)
		{
			return Functions.glfwGetInstanceProcAddressPtr(instance, procname);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int glfwGetPhysicalDevicePresentationSupport(IntPtr instance, IntPtr device, uint queuefamily)
		{
			return Functions.glfwGetPhysicalDevicePresentationSupportPtr(instance, device, queuefamily);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int glfwCreateWindowSurface(IntPtr instance, IntPtr window, IntPtr allocator, out IntPtr surface)
		{
			return Functions.glfwCreateWindowSurfacePtr(instance, window, allocator, out surface);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr glfwGetWin32Window(IntPtr window)
		{
			return Functions.glfwGetWin32WindowPtr(window);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr glfwGetX11Window(IntPtr window)
		{
			return Functions.glfwGetX11WindowPtr(window);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr glfwGetCocoaWindow(IntPtr window)
		{
			return Functions.glfwGetCocoaWindowPtr(window);
		}
	}
}