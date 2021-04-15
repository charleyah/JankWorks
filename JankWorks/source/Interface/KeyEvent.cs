using System;
using System.Runtime.CompilerServices;

namespace JankWorks.Interface
{
    [Serializable]
    public readonly struct KeyEvent : IEquatable<KeyEvent>
    {
        public readonly int KeyCode;
        public readonly Key Key;
        public readonly Modifier Modifiers;
        public readonly bool Repeated;
        public KeyEvent(int code, Key key, Modifier modifiers, bool repeated)
        {
            this.KeyCode = code;
            this.Key = key;
            this.Modifiers = modifiers;
            this.Repeated = repeated;
        }
        public override int GetHashCode() => this.KeyCode.GetHashCode() ^ this.Key.GetHashCode() ^ this.Modifiers.GetHashCode();

        public bool Equals(KeyEvent other) => this == other;
        public override bool Equals(object obj) => obj is KeyEvent other && this == other;

        public static bool operator ==(KeyEvent a, KeyEvent b) => a.KeyCode == b.KeyCode && a.Key == b.Key && a.Modifiers == b.Modifiers;
        public static bool operator !=(KeyEvent a, KeyEvent b) => a.KeyCode != b.KeyCode || a.Key != b.Key || a.Modifiers != b.Modifiers;
    }


    [Flags]
    public enum Modifier
    {
        None = 0,
        Shift = 1,
        Control = 2,
        Alt = 4,
        System = 8,
        CapsLock = 16,
        NumLock = 32
    }

    public enum Key : int
    {
        Unknown,

        A,     
        B,     
        C,       
        D,
        E,
        F,
        G,
        H,
        I,
        J,
        K,
        L,
        M,
        N,
        O,
        P,
        Q,
        R,
        S,
        T,
        U,
        V,
        W,
        X,
        Y,
        Z,

        Num0,
        Num1,
        Num2,
        Num3,
        Num4,
        Num5,
        Num6,
        Num7,
        Num8,
        Num9,

        Numpad0,
        Numpad1,
        Numpad2,
        Numpad3,
        Numpad4,
        Numpad5,
        Numpad6,
        Numpad7,
        Numpad8,
        Numpad9,
        NumEnter,


        LeftCtrl,
        LeftShift,
        LeftAlt,
        RightCtrl,
        RightShift,
        RightAlt,

        LeftSystem,
        RightSystem,


        Escape,
        GraveAccent,
        Menu,
        LeftBracket,
        RightBracket,
        Semicolon,
        Comma,
        Period,
        Slash,
        Backslash,
        Tilde,
        Equal,
        Hyphen,
        Apostrophe,
        Space,
        Enter,
        Backspace,
        Tab,
        PageUp,
        PageDown,
        End,
        Home,
        Insert,
        Delete,
        Add,
        Subtract,
        Multiply,
        Divide,
        Left,
        Right,
        Up,
        Down,

        F1,
        F2,
        F3,
        F4,
        F5,
        F6,
        F7,
        F8,
        F9,
        F10,
        F11,
        F12,
        F13,
        F14,
        F15,
        F16,
        F17,
        F18,
        F19,
        F20,
    };

    public static class KeyExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsKeyType(int lower, int upper, int value) => value >= lower && value <= upper;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsModifierKey(this Key key) => IsKeyType((int)Key.LeftCtrl, (int)Key.RightAlt, (int)key);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFunctionKey(this Key key) => IsKeyType((int)Key.F1, (int)Key.F20, (int)key);
    }
}
