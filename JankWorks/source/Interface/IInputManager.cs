using System;
using System.Numerics;

using JankWorks.Core;

namespace JankWorks.Interface
{
    public interface IInputManager
    {
        Event HideHandler { get; }
        event Action OnHide;

        Event ShowHandler { get; }
        event Action OnShow;

        Event FocusHandler { get;}
        event Action OnFocus;

        Event LostFocusHandler { get; }
        event Action OnLostFocus;

        Event MouseEnteredHandler { get; }
        event Action OnMouseEntered;

        Event MouseLeftHandler { get; }
        event Action OnMouseLeft;

        Event<Vector2> MouseMovedHandler { get; }
        event Action<Vector2> OnMouseMoved;

        Event<MouseButtonEvent> MouseButtonPressedHandler { get; }
        event Action<MouseButtonEvent> OnMouseButtonPressed;

        Event<MouseButtonEvent> MouseButtonReleasedHandler { get; }
        event Action<MouseButtonEvent> OnMouseButtonReleased;

        Event<Vector2> ScrollHandler { get; }
        event Action<Vector2> OnScroll;

        Event<KeyEvent> KeyPressedHandler { get; }
        event Action<KeyEvent> OnKeyPressed;

        Event<KeyEvent> KeyReleasedHandler { get; }
        event Action<KeyEvent> OnKeyReleased;

        Event<char> TextEnteredHandler { get; }
        event Action<char> OnTextEntered;
    }
}