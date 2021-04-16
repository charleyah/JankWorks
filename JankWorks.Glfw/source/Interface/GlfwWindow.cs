using System;
using System.Text;
using System.Numerics;

using JankWorks.Util;
using JankWorks.Graphics;
using JankWorks.Interface;

using static JankWorks.Drivers.Glfw.Api;

namespace JankWorks.Drivers.Glfw.Interface
{
    public sealed class GlfwWindow : Window
    {
        public override IntPtr NativeHandle
        { 
            get
            {
                return glfwGetNativeWindow(this.window);
            }
        }

        public override bool IsOpen => glfwWindowShouldClose(this.window) == GLFW_FALSE;

        private readonly IntPtr window;

        private readonly Decoder utf32Decoder;

        private bool keyRepeatEnabled;

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
                var mode = settings.VideoMode;
                int bitspercolour = (int)mode.BitsPerPixel / 4;

                glfwWindowHint(GLFW_RED_BITS, bitspercolour);
                glfwWindowHint(GLFW_GREEN_BITS, bitspercolour);
                glfwWindowHint(GLFW_BLUE_BITS, bitspercolour);
                glfwWindowHint(GLFW_ALPHA_BITS, bitspercolour);

                glfwWindowHint(GLFW_REFRESH_RATE, (int)mode.RefreshRate);

                this.window = glfwCreateWindow((int)mode.Width, (int)mode.Height, settings.Title, settings.Style == WindowStyle.FullScreen ? glfwMonitor.Handle : IntPtr.Zero, IntPtr.Zero);
            }

            if (this.window == IntPtr.Zero)
            {
                var errorPtr = IntPtr.Zero;
                glfwGetError(errorPtr);
                var errorDesc = new CString(errorPtr);
                throw new ApplicationException(errorDesc);
            }

            this.SetupCallbacks();

            this.Activate();
            glfwSwapInterval(settings.VSync ? 1 : 0);
            glfwSetInputMode(this.window, GLFW_CURSOR, settings.ShowCursor ? GLFW_CURSOR_NORMAL : GLFW_CURSOR_HIDDEN);
        }

        private void SetupCallbacks()
        {
            glfwSetWindowSizeCallback(this.window, this.HandleResizeEvent);
            glfwSetWindowFocusCallback(this.window, this.HandleFocusEvent);
            glfwSetCursorEnterCallback(this.window, this.HandleMouseEnterOrLeaveEvent);
            glfwSetCursorPosCallback(this.window, this.HandleMouseMoveEvent);
            glfwSetMouseButtonCallback(this.window, this.HandleMouseButtonEvent);
            glfwSetScrollCallback(this.window, this.HandleScrollEvent);
            glfwSetKeyCallback(this.window, this.HandleKeyEvent);
            glfwSetCharCallback(this.window, this.HandleTextEvent);
        }

        private void HandleResizeEvent(IntPtr window, int width, int height)
        {
            var vp = new Rectangle(0, 0, width, height);
            this.ResizeHandler.Notify(vp);
        }

        private void HandleFocusEvent(IntPtr window, int focused)
        {
            var handler = focused == GLFW_TRUE ? this.FocusHandler : this.LostFocusHandler;
            handler.Notify();
        }

        private void HandleMouseEnterOrLeaveEvent(IntPtr window, int entered)
        {
            var handler = entered == GLFW_TRUE ? this.MouseEnteredHandler : this.MouseLeftHandler;
            handler.Notify();
        }

        private void HandleMouseMoveEvent(IntPtr window, double xpos, double ypos) 
        => this.MouseMovedHandler.Notify(new Vector2(Convert.ToSingle(xpos), Convert.ToSingle(ypos)));

        private void HandleMouseButtonEvent(IntPtr window, int button, int action, int mods)
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

        private void HandleScrollEvent(IntPtr window, double xpos, double ypos) 
        => this.ScrollHandler.Notify(new Vector2(Convert.ToSingle(xpos), Convert.ToSingle(ypos)));

        private void HandleKeyEvent(IntPtr window, int key, int scancode, int action, int mods)
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

        private void HandleTextEvent(IntPtr window, uint utf32code)
        {
            var ch = char.MinValue;
            unsafe { this.utf32Decoder.GetChars((byte*)&utf32code, sizeof(uint), &ch, 1, true); }
            this.TextEnteredHandler.Notify(ch);
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

        public override void Close() => glfwSetWindowShouldClose(this.window, GLFW_TRUE);
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
