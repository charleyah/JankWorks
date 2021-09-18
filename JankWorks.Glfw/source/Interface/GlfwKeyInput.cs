using System;

using JankWorks.Interface;

using static JankWorks.Drivers.Glfw.Native.Constants;

namespace JankWorks.Drivers.Glfw.Interface
{
    internal readonly ref struct GlfwKeyInput
    {
        private readonly int key;
        private readonly GlfwModifierKeys modifiers;

        public GlfwKeyInput(int key, int modifiers)
        {
            this.key = key;
            this.modifiers = (GlfwModifierKeys)modifiers;
        }

        public Modifier Modifiers
        {
            get
            {
                var modif = Modifier.None;

                if (this.modifiers.HasFlag(GlfwModifierKeys.Shift)) { modif |= Modifier.Shift; }
                if (this.modifiers.HasFlag(GlfwModifierKeys.Control)) { modif |= Modifier.Control; }
                if (this.modifiers.HasFlag(GlfwModifierKeys.Alt)) { modif |= Modifier.Alt; }
                if (this.modifiers.HasFlag(GlfwModifierKeys.Super)) { modif |= Modifier.System; }
                if (this.modifiers.HasFlag(GlfwModifierKeys.NumLock)) { modif |= Modifier.NumLock; }
                if (this.modifiers.HasFlag(GlfwModifierKeys.CapsLock)) { modif |= Modifier.CapsLock; }

                return modif;
            }
        }

        public Key Key
        {
            get
            {
                // look at this beautiful switch statement...
                switch (this.key)
                {
                    case GLFW_KEY_A:
                    case GLFW_KEY_B:
                    case GLFW_KEY_C:
                    case GLFW_KEY_D:
                    case GLFW_KEY_E:
                    case GLFW_KEY_F:
                    case GLFW_KEY_G:
                    case GLFW_KEY_H:
                    case GLFW_KEY_I:
                    case GLFW_KEY_J:
                    case GLFW_KEY_K:
                    case GLFW_KEY_L:
                    case GLFW_KEY_M:
                    case GLFW_KEY_N:
                    case GLFW_KEY_O:
                    case GLFW_KEY_P:
                    case GLFW_KEY_Q:
                    case GLFW_KEY_R:
                    case GLFW_KEY_S:
                    case GLFW_KEY_T:
                    case GLFW_KEY_U:
                    case GLFW_KEY_V:
                    case GLFW_KEY_W:
                    case GLFW_KEY_X:
                    case GLFW_KEY_Y:
                    case GLFW_KEY_Z:
                        return (Key)key - AlphaKeyOffset;

                    case GLFW_KEY_0:
                    case GLFW_KEY_1:
                    case GLFW_KEY_2:
                    case GLFW_KEY_3:
                    case GLFW_KEY_4:
                    case GLFW_KEY_5:
                    case GLFW_KEY_6:
                    case GLFW_KEY_7:
                    case GLFW_KEY_8:
                    case GLFW_KEY_9:
                        return (Key)key - NumberKeyOffset;

                    case GLFW_KEY_KP_0:
                    case GLFW_KEY_KP_1:
                    case GLFW_KEY_KP_2:
                    case GLFW_KEY_KP_3:
                    case GLFW_KEY_KP_4:
                    case GLFW_KEY_KP_5:
                    case GLFW_KEY_KP_6:
                    case GLFW_KEY_KP_7:
                    case GLFW_KEY_KP_8:
                    case GLFW_KEY_KP_9:
                        return (Key)key - PadNumberKeyOffset;

                    case GLFW_KEY_F1:
                    case GLFW_KEY_F2:
                    case GLFW_KEY_F3:
                    case GLFW_KEY_F4:
                    case GLFW_KEY_F5:
                    case GLFW_KEY_F6:
                    case GLFW_KEY_F7:
                    case GLFW_KEY_F8:
                    case GLFW_KEY_F9:
                    case GLFW_KEY_F10:
                    case GLFW_KEY_F11:
                    case GLFW_KEY_F12:
                    case GLFW_KEY_F13:
                    case GLFW_KEY_F14:
                    case GLFW_KEY_F15:
                    case GLFW_KEY_F16:
                    case GLFW_KEY_F17:
                    case GLFW_KEY_F18:
                    case GLFW_KEY_F19:
                    case GLFW_KEY_F20:
                        return (Key)key - FunctionKeyOffset;

                    case GLFW_KEY_SPACE:
                        return Key.Space;

                    case GLFW_KEY_LEFT_SHIFT:
                        return Key.LeftShift;
                    case GLFW_KEY_LEFT_CONTROL:
                        return Key.LeftCtrl;
                    case GLFW_KEY_LEFT_ALT:
                        return Key.LeftAlt;
                    case GLFW_KEY_LEFT_SUPER:
                        return Key.LeftSystem;

                    case GLFW_KEY_RIGHT_SHIFT:
                        return Key.RightShift;
                    case GLFW_KEY_RIGHT_CONTROL:
                        return Key.RightCtrl;
                    case GLFW_KEY_RIGHT_ALT:
                        return Key.RightAlt;
                    case GLFW_KEY_RIGHT_SUPER:
                        return Key.RightSystem;

                    case GLFW_KEY_BACKSPACE:
                        return Key.Backspace;

                    case GLFW_KEY_ENTER:
                        return Key.Enter;
                    case GLFW_KEY_KP_ENTER:
                        return Key.NumEnter;

                    case GLFW_KEY_TAB:
                        return Key.Tab;
                    case GLFW_KEY_INSERT:
                        return Key.Insert;
                    case GLFW_KEY_DELETE:
                        return Key.Delete;
                    case GLFW_KEY_HOME:
                        return Key.Home;
                    case GLFW_KEY_END:
                        return Key.End;
                    case GLFW_KEY_PAGE_UP:
                        return Key.PageUp;
                    case GLFW_KEY_PAGE_DOWN:
                        return Key.PageDown;

                    case GLFW_KEY_UP:
                        return Key.Up;
                    case GLFW_KEY_DOWN:
                        return Key.Down;
                    case GLFW_KEY_LEFT:
                        return Key.Left;
                    case GLFW_KEY_RIGHT:
                        return Key.Right;

                    case GLFW_KEY_BACKSLASH:
                        return Key.Backslash;
                    case GLFW_KEY_SLASH:
                        return Key.Slash;
                    case GLFW_KEY_PERIOD:
                        return Key.Period;
                    case GLFW_KEY_COMMA:
                        return Key.Comma;
                    case GLFW_KEY_APOSTROPHE:
                        return Key.Apostrophe;
                    case GLFW_KEY_SEMICOLON:
                        return Key.Semicolon;
                    case GLFW_KEY_RIGHT_BRACKET:
                        return Key.RightBracket;
                    case GLFW_KEY_LEFT_BRACKET:
                        return Key.LeftBracket;
                    case GLFW_KEY_EQUAL:
                        return Key.Equal;
                    case GLFW_KEY_MINUS:
                        return Key.Hyphen;
                    case GLFW_KEY_GRAVE_ACCENT:
                        return Key.GraveAccent;
                    case GLFW_KEY_ESCAPE:
                        return Key.Escape;

                    case GLFW_KEY_KP_ADD:
                        return Key.Add;
                    case GLFW_KEY_KP_SUBTRACT:
                        return Key.Subtract;
                    case GLFW_KEY_KP_MULTIPLY:
                        return Key.Multiply;
                    case GLFW_KEY_KP_DIVIDE:
                        return Key.Divide;

                    default:
                        return Key.Unknown;
                }
                // I hate it.
            }
        }

        // consts of the numeric offsets between glfw key values and JankWorks Key enum values.
        private const int NumberKeyOffset = GLFW_KEY_0 - (int)Key.Num0;
        private const int PadNumberKeyOffset = GLFW_KEY_KP_0 - (int)Key.Numpad0;
        private const int AlphaKeyOffset = GLFW_KEY_A - (int)Key.A;
        private const int FunctionKeyOffset = GLFW_KEY_F1 - (int)Key.F1;

        [Flags]
        private enum GlfwModifierKeys
        {
            Shift = 0x0001,

            Control = 0x0002,

            Alt = 0x0004,

            Super = 0x0008,

            CapsLock = 0x0010,

            NumLock = 0x0020
        }
    }
}
