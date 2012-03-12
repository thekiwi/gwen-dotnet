using System;
using Gwen.Control;
using SFML.Window;

namespace Gwen.Input
{
    /// <summary>
    /// Helper class to avoid modifying SFML itself.
    /// Build it in SFML::RenderWindow.MouseButtonPressed/Released and pass to ProcessMessage.
    /// </summary>
    public class SFMLMouseButtonEventArgs : EventArgs
    {
        /// <summary>
        /// SFML event args.
        /// </summary>
        public readonly MouseButtonEventArgs Args;

        /// <summary>
        /// Indicates whether the button is pressed (true) or released (false).
        /// </summary>
        public readonly bool Down;

        public SFMLMouseButtonEventArgs(MouseButtonEventArgs args, bool down)
        {
            Args = args;
            Down = down;
        }
    }

    /// <summary>
    /// Helper class to avoid modifying SFML itself.
    /// Build it in SFML::RenderWindow.KeyPressed/Released and pass to ProcessMessage.
    /// </summary>
    public class SFMLKeyEventArgs : EventArgs
    {
        public readonly KeyEventArgs Args;

        /// <summary>
        /// Indicates whether the key is pressed (true) or released (false).
        /// </summary>
        public readonly bool Down;

        public SFMLKeyEventArgs(KeyEventArgs args, bool down)
        {
            Args = args;
            Down = down;
        }
    }

    /// <summary>
    /// SFML input handler.
    /// </summary>
    public class SFML
    {
        private Canvas m_Canvas;
        private int m_MouseX;
        private int m_MouseY;
        
        public SFML()
        {
            // not needed, retained for clarity
            m_MouseX = 0;
            m_MouseY = 0;
        }

        /// <summary>
        /// Sets the currently active canvas.
        /// </summary>
        /// <param name="c">Canvas to use.</param>
        public void Initialize(Canvas c)
        {
            m_Canvas = c;
        }

        /// <summary>
        /// Translates control key's SFML key code to GWEN's code.
        /// </summary>
        /// <param name="sfKey">SFML key code.</param>
        /// <returns>GWEN key code.</returns>
        private static Key TranslateKeyCode(Keyboard.Key sfKey)
        {
            switch (sfKey)
            {
                case Keyboard.Key.Back:			return Key.Backspace;
                case Keyboard.Key.Return:		return Key.Return;
                case Keyboard.Key.Escape:		return Key.Escape;
                case Keyboard.Key.Tab:			return Key.Tab;
                case Keyboard.Key.Space:		return Key.Space;
                case Keyboard.Key.Up:			return Key.Up;
                case Keyboard.Key.Down:			return Key.Down;
                case Keyboard.Key.Left:			return Key.Left;
                case Keyboard.Key.Right:		return Key.Right;
                case Keyboard.Key.Home:			return Key.Home;
                case Keyboard.Key.End:			return Key.End;
                case Keyboard.Key.Delete:		return Key.Delete;
                case Keyboard.Key.LControl:		return Key.Control;
                case Keyboard.Key.LAlt:			return Key.Alt;
                case Keyboard.Key.LShift:		return Key.Shift;
                case Keyboard.Key.RControl:		return Key.Control;
                case Keyboard.Key.RAlt:			return Key.Alt;
                case Keyboard.Key.RShift:		return Key.Shift;
            }
            return Key.Invalid;
        }

        /// <summary>
        /// Translates alphanumeric SFML key code to character value.
        /// </summary>
        /// <param name="sfKey">SFML key code.</param>
        /// <returns>Translated character.</returns>
        private static char TranslateChar(Keyboard.Key sfKey)
        {
            if (sfKey >= Keyboard.Key.A && sfKey <= Keyboard.Key.Z)
                return (char)('A' + (int)sfKey);
            return ' ';
        }

        /// <summary>
        /// Main entrypoint for processing input events. Call from your RenderWindow's event handlers.
        /// </summary>
        /// <param name="args">SFML input event args: can be MouseMoveEventArgs, SFMLMouseButtonEventArgs, MouseWheelEventArgs, TextEventArgs, SFMLKeyEventArgs.</param>
        /// <returns>True if the event was handled.</returns>
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

                return m_Canvas.Input_MouseMoved(m_MouseX, m_MouseY, dx, dy);
            }

            if (args is SFMLMouseButtonEventArgs)
            {
                SFMLMouseButtonEventArgs ev = args as SFMLMouseButtonEventArgs;
                return m_Canvas.Input_MouseButton((int) ev.Args.Button, ev.Down);
            }

            if (args is MouseWheelEventArgs)
            {
                MouseWheelEventArgs ev = args as MouseWheelEventArgs;
                return m_Canvas.Input_MouseWheel(ev.Delta*60);
            }

            if (args is TextEventArgs)
            {
                TextEventArgs ev = args as TextEventArgs;
                // [omeg] following may not fit in 1 char in theory
                return m_Canvas.Input_Character(ev.Unicode[0]);
            }

            if (args is SFMLKeyEventArgs)
            {
                SFMLKeyEventArgs ev = args as SFMLKeyEventArgs;

                if (ev.Args.Control && ev.Args.Alt && ev.Args.Code == Keyboard.Key.LControl)
                    return false; // this is AltGr

                char ch = TranslateChar(ev.Args.Code);
                if (ev.Down && InputHandler.DoSpecialKeys(m_Canvas, ch))
                    return false;

                Key key = TranslateKeyCode(ev.Args.Code);
                if (key == Key.Invalid && !ev.Down) // it's not special char and it's been released
                    return InputHandler.HandleAccelerator(m_Canvas, ch);
                    //return m_Canvas.Input_Character(ch);)

                return m_Canvas.Input_Key(key, ev.Down);
            }

            throw new ArgumentException("Invalid event args", "args");
            return false;
        }
    }
}
