using System;

namespace JankWorks.Interface
{
    [Serializable]
    public readonly struct MouseButtonEvent : IEquatable<MouseButtonEvent>
    {
        public readonly int ButtonCode;
        public readonly MouseButton Button;

        public MouseButtonEvent(int buttonCode, MouseButton button)
        {
            this.ButtonCode = buttonCode;
            this.Button = button;
        }

        public override int GetHashCode() => this.ButtonCode.GetHashCode() ^ this.Button.GetHashCode();
        public override bool Equals(object obj) => obj is MouseButtonEvent other && this == other;
        public bool Equals(MouseButtonEvent other) => this == other;
        public static bool operator ==(MouseButtonEvent a, MouseButtonEvent b) => a.ButtonCode == b.ButtonCode && a.Button == b.Button;
        public static bool operator !=(MouseButtonEvent a, MouseButtonEvent b) => a.ButtonCode != b.ButtonCode || a.Button != b.Button;
    }

    public enum MouseButton : int
    {
        Left,
        Right,
        Middle,
        Other
    }
}
