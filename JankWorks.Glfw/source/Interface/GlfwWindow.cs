using System;
using System.Text;
using System.Numerics;

using JankWorks.Util;
using JankWorks.Graphics;
using JankWorks.Interface;
using JankWorks.Platform;

using JankWorks.Drivers.Glfw.Native;
using static JankWorks.Drivers.Glfw.Native.Constants;
using static JankWorks.Drivers.Glfw.Native.Functions;

namespace JankWorks.Drivers.Glfw.Interface
{
    public sealed class GlfwWindow : Window
    {
        public override IntPtr NativeHandle
        { 
            get
            {
                switch (SystemEnvironment.Current.OS)
                {
                    case SystemPlatform.Windows: return glfwGetWin32Window(this.window);
                    case SystemPlatform.Linux: return glfwGetX11Window(this.window);
                    case SystemPlatform.MacOS: return glfwGetCocoaWindow(this.window);

                    default: throw new PlatformNotSupportedException();
                }
            }
        }

        public override bool IsOpen => glfwWindowShouldClose(this.window) == GLFW_FALSE;

        private readonly IntPtr window;

        private readonly Decoder utf32Decoder;

        private bool keyRepeatEnabled;

        private readonly GLFWwindowsizefun windowResizeDelegate;
        private readonly GLFWwindowfocusfun windowFocusDelegate;
        private readonly GLFWcursorenterfun cursorEnterDelegate;
        private readonly GLFWcursorposfun cursorMoveDelegate;
        private readonly GLFWmousebuttonfun mouseButtonDelegate;
        private readonly GLFWscrollfun scrollDelegate;
        private readonly GLFWkeyfun keyDelegate;
        private readonly GLFWcharfun textDelegate;

        public GlfwWindow(WindowSettings settings) : base(settings)
        {
            var glfwMonitor = settings.Monitor as GlfwMonitor ?? throw new NotSupportedException();
            this.keyRepeatEnabled = false;

            var enc = Encoding.GetEncoding("utf-32", new EncoderReplacementFallback("□"), new DecoderReplacementFallback("□"));
            this.utf32Decoder = enc.GetDecoder();

            glfwDefaultWindowHints();

            glfwWindowHint(GLFW_RESIZABLE, GLFW_FALSE);
            glfwWindowHint(GLFW_VISIBLE, GLFW_FALSE);
            glfwWindowHint(GLFW_DOUBLEBUFFER, GLFW_TRUE);

            switch (settings.Style)
            {
                case WindowStyle.Windowed: 
                    glfwWindowHint(GLFW_DECORATED, GLFW_TRUE); 
                    break;

                case WindowStyle.FullScreen:
                case WindowStyle.Borderless: 
                    glfwWindowHint(GLFW_DECORATED, GLFW_FALSE); 
                    break;
            }

            checked
            {
                var mode = settings.DisplayMode;
                int bitspercolour = (int)mode.BitsPerPixel / 4;

                glfwWindowHint(GLFW_RED_BITS, bitspercolour);
                glfwWindowHint(GLFW_GREEN_BITS, bitspercolour);
                glfwWindowHint(GLFW_BLUE_BITS, bitspercolour);
                glfwWindowHint(GLFW_ALPHA_BITS, bitspercolour);

                glfwWindowHint(GLFW_REFRESH_RATE, (int)mode.RefreshRate);

                var titleutf8 = Encoding.UTF8.GetBytes(settings.Title);

                unsafe
                {
                    fixed(byte* titleptr = titleutf8)
                    {
                        this.window = glfwCreateWindow((int)mode.Width, (int)mode.Height, titleptr, settings.Style == WindowStyle.FullScreen ? glfwMonitor.Handle : IntPtr.Zero, IntPtr.Zero);
                    }
                }                
            }

            if (this.window == IntPtr.Zero)
            {
                var errorPtr = IntPtr.Zero;
                glfwGetError(errorPtr);
                var errorDesc = new CString(errorPtr);
                throw new ApplicationException(errorDesc);
            }

            this.windowResizeDelegate = this.HandleResizeEvent;
            this.windowFocusDelegate = this.HandleFocusEvent;
            this.cursorEnterDelegate = this.HandleMouseEnterOrLeaveEvent;
            this.cursorMoveDelegate = this.HandleMouseMoveEvent;
            this.mouseButtonDelegate = this.HandleMouseButtonEvent;
            this.scrollDelegate = this.HandleScrollEvent;
            this.keyDelegate = this.HandleKeyEvent;
            this.textDelegate = this.HandleTextEvent;

            this.SetupCallbacks();

            this.Activate();
            glfwSwapInterval(settings.VSync ? 1 : 0);
            glfwSetInputMode(this.window, GLFW_CURSOR, settings.ShowCursor ? GLFW_CURSOR_NORMAL : GLFW_CURSOR_HIDDEN);
        }

        private void SetupCallbacks()
        {
            glfwSetWindowSizeCallback(this.window, this.windowResizeDelegate);
            glfwSetWindowFocusCallback(this.window, this.windowFocusDelegate);
            glfwSetCursorEnterCallback(this.window, this.cursorEnterDelegate);
            glfwSetCursorPosCallback(this.window, this.cursorMoveDelegate);
            glfwSetMouseButtonCallback(this.window, this.mouseButtonDelegate);
            glfwSetScrollCallback(this.window, this.scrollDelegate);
            glfwSetKeyCallback(this.window, this.keyDelegate);
            glfwSetCharCallback(this.window, this.textDelegate);
        }

        private void HandleResizeEvent(IntPtr window, int width, int height)
        {
            var vp = new Rectangle(0, 0, width, height);

            if(this.ResizeHandler.HasSubscribers)
            {
                this.ResizeHandler.Notify(vp);
            }
        }

        private void HandleFocusEvent(IntPtr window, int focused)
        {
            var handler = focused == GLFW_TRUE ? this.FocusHandler : this.LostFocusHandler;
            if (handler.HasSubscribers)
            {
                handler.Notify();
            }
        }

        private void HandleMouseEnterOrLeaveEvent(IntPtr window, int entered)
        {            
            var handler = entered == GLFW_TRUE ? this.MouseEnteredHandler : this.MouseLeftHandler;
            if(handler.HasSubscribers)
            {
                handler.Notify();
            }            
        }

        private void HandleMouseMoveEvent(IntPtr window, double xpos, double ypos)
        {
            if(this.MouseMovedHandler.HasSubscribers)
            {
                this.MouseMovedHandler.Notify(new Vector2(Convert.ToSingle(xpos), Convert.ToSingle(ypos)));
            }            
        }
        

        private void HandleMouseButtonEvent(IntPtr window, int button, int action, int mods)
        {
            if (this.MouseButtonPressedHandler.HasSubscribers || this.MouseButtonReleasedHandler.HasSubscribers)
            {
                var commonButton = MouseButton.Other;

                if (button == GLFW_MOUSE_BUTTON_LEFT) { commonButton = MouseButton.Left; }
                else if (button == GLFW_MOUSE_BUTTON_RIGHT) { commonButton = MouseButton.Right; }
                else if (button == GLFW_MOUSE_BUTTON_MIDDLE) { commonButton = MouseButton.Middle; }

                var mbe = new MouseButtonEvent(button, commonButton);

                if (action == GLFW_PRESS)
                {
                    this.MouseButtonPressedHandler.Notify(mbe);
                }
                else if (action == GLFW_RELEASE)
                {
                    this.MouseButtonReleasedHandler.Notify(mbe);
                }
            }
            
        }

        private void HandleScrollEvent(IntPtr window, double xpos, double ypos)
        {
            if(this.ScrollHandler.HasSubscribers)
            {
                this.ScrollHandler.Notify(new Vector2(Convert.ToSingle(xpos), Convert.ToSingle(ypos)));
            }            
        }
        
        private void HandleKeyEvent(IntPtr window, int key, int scancode, int action, int mods)
        {
            if(this.KeyPressedHandler.HasSubscribers || this.KeyReleasedHandler.HasSubscribers)
            {
                var actionIsRepeat = action == GLFW_REPEAT;
                var repeatEnabled = this.keyRepeatEnabled;

                if (actionIsRepeat && !repeatEnabled)
                {
                    return;
                }

                var input = new GlfwKeyInput(key, mods);

                var kye = new KeyEvent(scancode, input.Key, input.Modifiers, actionIsRepeat);

                if (action == GLFW_PRESS || (actionIsRepeat && repeatEnabled))
                {
                    this.KeyPressedHandler.Notify(kye);
                }
                else if (action == GLFW_RELEASE)
                {
                    this.KeyReleasedHandler.Notify(kye);
                }
            }            
        }

        private void HandleTextEvent(IntPtr window, uint utf32code)
        {
            if(this.TextEnteredHandler.HasSubscribers)
            {
                var ch = char.MinValue;
                unsafe { this.utf32Decoder.GetChars((byte*)&utf32code, sizeof(uint), &ch, 1, true); }
                this.TextEnteredHandler.Notify(ch);
            }
            
        }

        private void ClearCallbacks()
        {
            glfwSetWindowSizeCallback(this.window, null);
            glfwSetWindowFocusCallback(this.window, null);
            glfwSetCursorEnterCallback(this.window, null);
            glfwSetCursorPosCallback(this.window, null);
            glfwSetMouseButtonCallback(this.window, null);
            glfwSetScrollCallback(this.window, null);
            glfwSetKeyCallback(this.window, null);
            glfwSetCharCallback(this.window, null);
        }

        public override void EnableKeyRepeat(bool enable) => this.keyRepeatEnabled = enable;

        public override void Close()
        {
            this.ClearCallbacks();
            glfwSetWindowShouldClose(this.window, GLFW_TRUE);
        }
        public override void Focus() => glfwFocusWindow(this.window);

        public override void Hide()
        {
            base.Hide();
            glfwHideWindow(this.window);

        }
        public override void Show()
        {
            glfwShowWindow(this.window);
            base.Show();
        }

        public override void ProcessEvents() => glfwPollEvents();    
        public override void SwapBuffers() => glfwSwapBuffers(this.window);

        public override void Activate() => glfwMakeContextCurrent(this.window);
        public override void Deactivate() => glfwMakeContextCurrent(IntPtr.Zero);

        protected override void Dispose(bool finalising)
        {
            this.ClearCallbacks();
            glfwDestroyWindow(this.window);
            base.Dispose(finalising);
        }
    }
}
