using System;

using Gwen.Controls;

using SFML.Window;

namespace Gwen.Input
{
    public class SFML
    {
        protected Canvas m_Canvas;
        protected int m_MouseX;
        protected int m_MouseY;

        public SFML()
        {
            // not needed, retained for clarity
            m_MouseX = 0;
            m_MouseY = 0;
        }

        public void Initialize(Canvas c)
        {
            m_Canvas = c;
        }

        public Key TranslateKeyCode(KeyCode sfKey)
        {
            switch (sfKey)
            {
                        case KeyCode.Back:			return Key.Backspace;
                        case KeyCode.Return:		return Key.Return;
                        case KeyCode.Escape:		return Key.Escape;
                        case KeyCode.Tab:			return Key.Tab;
                        case KeyCode.Space:		    return Key.Space;
                        case KeyCode.Up:			return Key.Up;
                        case KeyCode.Down:			return Key.Down;
                        case KeyCode.Left:			return Key.Left;
                        case KeyCode.Right:		    return Key.Right;
                        case KeyCode.Home:			return Key.Home;
                        case KeyCode.End:			return Key.End;
                        case KeyCode.Delete:		return Key.Delete;
                        case KeyCode.LControl:		return Key.Control;
                        case KeyCode.LAlt:			return Key.Alt;
                        case KeyCode.LShift:		return Key.Shift;
                        case KeyCode.RControl:		return Key.Control;
                        case KeyCode.RAlt:			return Key.Alt;
                        case KeyCode.RShift:		return Key.Shift;
            }
            return Key.Invalid;
        }

        public bool ProcessMessage(EventArgs args)
        {
            if (null == m_Canvas) return false;

            if (args is MouseMoveEventArgs)
            {
                MouseMoveEventArgs ev = args as MouseMoveEventArgs;
                int dx = ev.X - m_MouseX;
                int dy = ev.Y - m_MouseY;

                m_MouseX = ev.X;
                m_MouseY = ev.Y;

                return m_Canvas.InputMouseMoved(m_MouseX, m_MouseY, dx, dy);
            }

            if (args is MouseButtonEventArgs)
            {
                MouseButtonEventArgs ev = args as MouseButtonEventArgs;
                return m_Canvas.InputMouseButton((int) ev.Button, ev.Down);
            }

            if (args is MouseWheelEventArgs)
            {
                MouseWheelEventArgs ev = args as MouseWheelEventArgs;
                return m_Canvas.InputMouseWheel(ev.Delta*60);
            }

            if (args is TextEventArgs)
            {
                TextEventArgs ev = args as TextEventArgs;
                // [omeg] following may not fit in 1 char in theory
                return m_Canvas.InputCharacter(ev.Unicode[0]);
            }

            if (args is KeyEventArgs)
            {
                KeyEventArgs ev = args as KeyEventArgs;
                if (ev.Control && ev.Down && ev.Code >= KeyCode.A && ev.Code <= KeyCode.Z)
                {
                    return m_Canvas.InputCharacter((char) ev.Code); // [omeg] works?
                }

                Key iKey = TranslateKeyCode(ev.Code);

                return m_Canvas.InputKey(iKey, ev.Down);

            }

            return false;
        }

    }
}
