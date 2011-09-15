using System;
using System.Drawing;
using System.Linq;
using System.Text;
using Gwen.Control;
using Gwen.DragDrop;

namespace Gwen.Input
{
    public static class Input
    {
        private static KeyData m_KeyData = new KeyData();
        private static float[] m_LastClickTime = new float[MaxMouseButtons];
        private static Point m_LastClickPos;

        public static int MaxMouseButtons { get { return 5; } }
        public static float DoubleClickSpeed { get { return 0.5f; } }
        public static float KeyRepeatRate { get { return 0.03f; } }
        public static float KeyRepeatDelay { get { return 0.5f; } }

        public static bool IsLeftMouseDown { get { return m_KeyData.LeftMouseDown; } }
        public static bool IsRightMouseDown { get { return m_KeyData.RightMouseDown; } }
        public static Point MousePosition; // not property to allow modification of Point fields
        public static bool IsShiftDown { get { return IsKeyDown(Key.Shift); } }
        public static bool IsControlDown { get { return IsKeyDown(Key.Control); } }

        // For use in panels
        public static bool IsKeyDown(Key key)
        {
            return m_KeyData.KeyState[(int)key];
        }

        // Does copy, paste etc
        public static bool DoSpecialKeys(Base canvas, char chr)
        {
            if (null == Global.KeyboardFocus) return false;
            if (Global.KeyboardFocus.GetCanvas() != canvas) return false;
            if (!Global.KeyboardFocus.IsVisible) return false;
            if (!IsControlDown) return false;

            if (chr == 'C' || chr == 'c')
            {
                Global.KeyboardFocus.InputCopy(null);
                return true;
            }

            if (chr == 'V' || chr == 'v')
            {
                Global.KeyboardFocus.InputPaste(null);
                return true;
            }

            if (chr == 'X' || chr == 'x')
            {
                Global.KeyboardFocus.InputCut(null);
                return true;
            }

            if (chr == 'A' || chr == 'a')
            {
                Global.KeyboardFocus.InputSelectAll(null);
                return true;
            }

            return false;
        }

        public static bool HandleAccelerator(Base canvas, char chr)
        {
            //Build the accelerator search string
            StringBuilder accelString = new StringBuilder();
            if (IsControlDown)
                accelString.Append("Ctrl + ");
            if (IsShiftDown)
                accelString.Append("Shift + ");
            // [omeg] todo: alt?

            accelString.Append(chr);
            String acc = accelString.ToString();

            //Debug::Msg("Accelerator string :%S\n", accelString.c_str());)

            if (Global.KeyboardFocus != null && Global.KeyboardFocus.HandleAccelerator(acc))
                return true;

            if (Global.MouseFocus != null && Global.MouseFocus.HandleAccelerator(acc))
                return true;

            if (canvas.HandleAccelerator(acc))
                return true;

            return false;
        }

        // Send input to canvas for study		
        public static void onMouseMoved(Base canvas, int x, int y, int dx, int dy)
        {
            MousePosition.X = x;
            MousePosition.Y = y;

            UpdateHoveredControl(canvas);
        }

        public static void onCanvasThink(Base control)
        {
            if (Global.MouseFocus != null && !Global.MouseFocus.IsVisible)
                Global.MouseFocus = null;

            if (Global.KeyboardFocus != null && (!Global.KeyboardFocus.IsVisible || !Global.KeyboardFocus.KeyboardInputEnabled))
                Global.KeyboardFocus = null;

            if (null == Global.KeyboardFocus) return;
            if (Global.KeyboardFocus.GetCanvas() != control) return;

            float time = Platform.Neutral.GetTimeInSeconds();

            //
            // Simulate Key-Repeats
            //
            for (int i = 0; i < (int)Key.Count; i++)
            {
                if (m_KeyData.KeyState[i] && m_KeyData.Target != Global.KeyboardFocus)
                {
                    m_KeyData.KeyState[i] = false;
                    continue;
                }

                if (m_KeyData.KeyState[i] && time > m_KeyData.NextRepeat[i])
                {
                    m_KeyData.NextRepeat[i] = Platform.Neutral.GetTimeInSeconds() + KeyRepeatRate;

                    if (Global.KeyboardFocus != null)
                    {
                        Global.KeyboardFocus.InputKeyPress((Key)i);
                    }
                }
            }
        }

        public static bool onMouseClicked(Base canvas, int mouseButton, bool down)
        {
            // If we click on a control that isn't a menu we want to close
            // all the open menus. Menus are children of the canvas.
            if (down && (null == Global.HoveredControl || !Global.HoveredControl.IsMenuComponent))
            {
                canvas.CloseMenus();
            }

            if (null == Global.HoveredControl) return false;
            if (Global.HoveredControl.GetCanvas() != canvas) return false;
            if (!Global.HoveredControl.IsVisible) return false;
            if (Global.HoveredControl == canvas) return false;

            if (mouseButton > MaxMouseButtons)
                return false;

            if (mouseButton == 0) 
                m_KeyData.LeftMouseDown = down;
            else if (mouseButton == 1) 
                m_KeyData.RightMouseDown = down;

            // Double click.
            // Todo: Shouldn't double click if mouse has moved significantly
            bool isDoubleClick = false;

            if (down &&
                m_LastClickPos.X == MousePosition.X &&
                m_LastClickPos.Y == MousePosition.Y &&
                (Platform.Neutral.GetTimeInSeconds() - m_LastClickTime[mouseButton]) < DoubleClickSpeed)
            {
                isDoubleClick = true;
            }

            if (down && !isDoubleClick)
            {
                m_LastClickTime[mouseButton] = Platform.Neutral.GetTimeInSeconds();
                m_LastClickPos = MousePosition;
            }

            if (down)
            {
                FindKeyboardFocus(Global.HoveredControl);
            }

            Global.HoveredControl.UpdateCursor();

            // This tells the child it has been touched, which
            // in turn tells its parents, who tell their parents.
            // This is basically so that Windows can pop themselves
            // to the top when one of their children have been clicked.
            if (down)
                Global.HoveredControl.Touch();

#if GWEN_HOOKSYSTEM
            if (bDown)
            {
                if (Hook::CallHook(&Hook::BaseHook::OnControlClicked, Global.HoveredControl, MousePosition.x,
                                   MousePosition.y))
                    return true;
            }
#endif

            switch (mouseButton)
            {
                case 0:
                    {
                        if (DragAndDrop.onMouseButton(Global.HoveredControl, MousePosition.X, MousePosition.Y, down))
                            return true;

                        if (isDoubleClick)
                            Global.HoveredControl.InputMouseDoubleClickLeft(MousePosition.X, MousePosition.Y);
                        else
                            Global.HoveredControl.InputMouseClickLeft(MousePosition.X, MousePosition.Y, down);
                        return true;
                    }

                case 1:
                    {
                        if (isDoubleClick)
                            Global.HoveredControl.InputMouseDoubleClickRight(MousePosition.X, MousePosition.Y);
                        else
                            Global.HoveredControl.InputMouseClickRight(MousePosition.X, MousePosition.Y, down);
                        return true;
                    }
            }

            return false;
        }

        public static bool onKeyEvent(Base canvas, Key key, bool down)
        {
            if (null == Global.KeyboardFocus) return false;
            if (Global.KeyboardFocus.GetCanvas() != canvas) return false;
            if (!Global.KeyboardFocus.IsVisible) return false;

            int iKey = (int)key;
            if (down)
            {
                if (!m_KeyData.KeyState[iKey])
                {
                    m_KeyData.KeyState[iKey] = true;
                    m_KeyData.NextRepeat[iKey] = Platform.Neutral.GetTimeInSeconds() + KeyRepeatDelay;
                    m_KeyData.Target = Global.KeyboardFocus;

                    return Global.KeyboardFocus.InputKeyPress(key);
                }
            }
            else
            {
                if (m_KeyData.KeyState[iKey])
                {
                    m_KeyData.KeyState[iKey] = false;

                    // BUG BUG. This causes shift left arrow in textboxes
                    // to not work. What is disabling it here breaking?
                    //m_KeyData.Target = NULL;

                    return Global.KeyboardFocus.InputKeyPress(key, false);
                }
            }

            return false;
        }

        private static void UpdateHoveredControl(Base inCanvas)
        {
            Base hovered = inCanvas.GetControlAt(MousePosition.X, MousePosition.Y);

            if (hovered != Global.HoveredControl)
            {
                if (Global.HoveredControl != null)
                {
                    var oldHover = Global.HoveredControl;
                    Global.HoveredControl = null;
                    oldHover.InputMouseLeave();
                }

                Global.HoveredControl = hovered;

                if (Global.HoveredControl != null)
                {
                    Global.HoveredControl.InputMouseEnter();
                }
            }

            if (Global.MouseFocus != null && Global.MouseFocus.GetCanvas() == inCanvas)
            {
                if (Global.HoveredControl != null)
                {
                    var oldHover = Global.HoveredControl;
                    Global.HoveredControl = null;
                    oldHover.Redraw();
                }
                Global.HoveredControl = Global.MouseFocus;
            }
        }

        private static void FindKeyboardFocus(Base control)
        {
            if (null == control) return;
            if (control.KeyboardInputEnabled)
            {
                //Make sure none of our children have keyboard focus first - todo recursive
                if (control.Children.Any(child => child == Global.KeyboardFocus))
                {
                    return;
                }

                control.Focus();
                return;
            }

            FindKeyboardFocus(control.Parent);
            return;
        }

    }
}
