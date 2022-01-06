using System;
using System.Numerics;

using JankWorks.Core;
using JankWorks.Graphics;

using JankWorks.Drivers;

namespace JankWorks.Interface
{
    public abstract class Window : Disposable, IRenderTarget, IInputManager
    {
        public abstract bool IsOpen { get; }
        public abstract IntPtr NativeHandle { get; }

        public WindowSettings InitialSettings { get; private set; }

        public Event<Rectangle> ResizeHandler { get; private set; }
        public event Action<Rectangle> OnResize { add => this.ResizeHandler.Subscribe(value); remove => this.ResizeHandler.Unsubscribe(value); }

        public Event HideHandler { get; private set; }
        public event Action OnHide { add => this.HideHandler.Subscribe(value); remove => this.HideHandler.Unsubscribe(value); }

        public Event ShowHandler { get; private set; }
        public event Action OnShow { add => this.ShowHandler.Subscribe(value); remove => this.ShowHandler.Unsubscribe(value); }
        
        public Event FocusHandler { get; private set; }
        public event Action OnFocus { add => this.FocusHandler.Subscribe(value); remove => this.FocusHandler.Unsubscribe(value); }

        public Event LostFocusHandler { get; private set; }
        public event Action OnLostFocus { add => this.LostFocusHandler.Subscribe(value); remove => this.LostFocusHandler.Unsubscribe(value); }

        public Event MouseEnteredHandler { get; private set; }
        public event Action OnMouseEntered { add => this.MouseEnteredHandler.Subscribe(value); remove => this.MouseEnteredHandler.Unsubscribe(value); }

        public Event MouseLeftHandler { get; private set; }
        public event Action OnMouseLeft { add => this.MouseLeftHandler.Subscribe(value); remove => this.MouseLeftHandler.Unsubscribe(value); }

        public Event<Vector2> MouseMovedHandler { get; private set; }
        public event Action<Vector2> OnMouseMoved { add => this.MouseMovedHandler.Subscribe(value); remove => this.MouseMovedHandler.Unsubscribe(value); }

        public Event<MouseButtonEvent> MouseButtonPressedHandler { get; private set; }
        public event Action<MouseButtonEvent> OnMouseButtonPressed { add => this.MouseButtonPressedHandler.Subscribe(value); remove => this.MouseButtonPressedHandler.Unsubscribe(value); }

        public Event<MouseButtonEvent> MouseButtonReleasedHandler { get; private set; }
        public event Action<MouseButtonEvent> OnMouseButtonReleased { add => this.MouseButtonReleasedHandler.Subscribe(value); remove => this.MouseButtonReleasedHandler.Unsubscribe(value); }

        public Event<Vector2> ScrollHandler { get; private set; }
        public event Action<Vector2> OnScroll { add => this.ScrollHandler.Subscribe(value); remove => this.ScrollHandler.Unsubscribe(value); }

        public Event<KeyEvent> KeyPressedHandler { get; private set; }
        public event Action<KeyEvent> OnKeyPressed { add => this.KeyPressedHandler.Subscribe(value); remove => this.KeyPressedHandler.Unsubscribe(value); }

        public Event<KeyEvent> KeyReleasedHandler { get; private set; }
        public event Action<KeyEvent> OnKeyReleased { add => this.KeyReleasedHandler.Subscribe(value); remove => this.KeyReleasedHandler.Unsubscribe(value); }

        public Event<char> TextEnteredHandler { get; private set; }
        public event Action<char> OnTextEntered { add => this.TextEnteredHandler.Subscribe(value); remove => this.TextEnteredHandler.Unsubscribe(value); }

        protected Window(WindowSettings settings)
        {
            this.InitialSettings = settings;

            this.ResizeHandler = new Event<Rectangle>();
            this.HideHandler = new Event();
            this.ShowHandler = new Event();
            this.FocusHandler = new Event();
            this.LostFocusHandler = new Event();

            this.MouseEnteredHandler = new Event();
            this.MouseLeftHandler = new Event();
            this.MouseMovedHandler = new Event<Vector2>();
            this.MouseButtonPressedHandler = new Event<MouseButtonEvent>();
            this.MouseButtonReleasedHandler = new Event<MouseButtonEvent>();
            this.ScrollHandler = new Event<Vector2>();

            this.KeyPressedHandler = new Event<KeyEvent>();
            this.KeyReleasedHandler = new Event<KeyEvent>();
            this.TextEnteredHandler = new Event<char>();
        }

        public virtual void Show() => this.ShowHandler.Notify();
        public virtual void Hide() => this.HideHandler.Notify();

        public abstract void Close();
        public abstract void Focus();

        public abstract void EnableKeyRepeat(bool enable);
        public abstract void ProcessEvents();
        public abstract void SwapBuffers();

        void IRenderTarget.Render() => this.SwapBuffers();
        public abstract void Activate();
        public abstract void Deactivate();

        protected override void Dispose(bool disposing)
        {
            this.ResizeHandler.ClearSubscribers();
            this.HideHandler.ClearSubscribers();
            this.ShowHandler.ClearSubscribers();
            this.FocusHandler.ClearSubscribers();
            this.LostFocusHandler.ClearSubscribers();

            this.MouseEnteredHandler.ClearSubscribers();
            this.MouseLeftHandler.ClearSubscribers();
            this.MouseMovedHandler.ClearSubscribers();
            this.MouseButtonPressedHandler.ClearSubscribers();
            this.MouseButtonReleasedHandler.ClearSubscribers();
            this.ScrollHandler.ClearSubscribers();

            this.KeyPressedHandler.ClearSubscribers();
            this.KeyReleasedHandler.ClearSubscribers();
            this.TextEnteredHandler.ClearSubscribers();
                

            base.Dispose(disposing);
        }

        public static Window Create(WindowSettings settings)
        {
            var drivers = DriverConfiguration.Drivers;
            return drivers.windowApi.CreateWindow(settings, drivers.graphicsApi);
        }
    }
}
