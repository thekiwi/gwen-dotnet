using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Gwen.Controls;
using Gwen.DragDrop;

namespace Gwen.Input
{
    public static class Input
    {
        private static KeyData KeyData = new KeyData();
        private static float [] g_fLastClickTime = new float[MaxMouseButtons];
        private static Point g_pntLastClickPos;

        public static int MaxMouseButtons { get { return 5; } }
        public static float DoubleClickSpeed { get { return 0.5f; } }
        public static float KeyRepeatRate { get { return 0.03f; } }
        public static float KeyRepeatDelay { get { return 0.5f; } }

        public static bool IsLeftMouseDown { get { return KeyData.LeftMouseDown; } }
        public static bool IsRightMouseDown { get { return KeyData.RightMouseDown; } }
        public static Point MousePosition; // not property to allow modification of Point fields
        public static bool IsShiftDown { get { return IsKeyDown(Key.Shift); } }
        public static bool IsControlDown { get { return IsKeyDown(Key.Control); } }

        // For use in panels
        public static bool IsKeyDown(Key key)
        {
            return KeyData.KeyState[(int)key];
        }

        // Does copy, paste etc
        public static bool DoSpecialKeys(Base canvas, char chr)
        {
            if (null==Global.KeyboardFocus) return false;
            if (Global.KeyboardFocus.GetCanvas() != canvas) return false;
            if (!Global.KeyboardFocus.IsVisible) return false;
            if (!IsControlDown) return false;

            if (chr == 'C' || chr == 'c' )
            {
                Global.KeyboardFocus.onCopy(null);
                return true;
            }

            if (chr == 'V' || chr == 'v' )
            {
                Global.KeyboardFocus.onPaste(null);
                return true;
            }

            if (chr == 'X' || chr == 'x' )
            {
                Global.KeyboardFocus.onCut(null);
                return true;
            }

            if (chr == 'A' || chr == 'a' )
            {
                Global.KeyboardFocus.onSelectAll(null);
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

            if (Global.KeyboardFocus!=null && Global.KeyboardFocus.HandleAccelerator(acc))
                return true;

            if (Global.MouseFocus!=null && Global.MouseFocus.HandleAccelerator(acc))
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
            if (Global.MouseFocus!=null && !Global.MouseFocus.IsVisible)
                Global.MouseFocus = null;

            if (Global.KeyboardFocus!=null && (!Global.KeyboardFocus.IsVisible || !Global.KeyboardFocus.KeyboardInputEnabled))
                Global.KeyboardFocus = null;

            if (null==Global.KeyboardFocus) return;
            if (Global.KeyboardFocus.GetCanvas() != control) return;

            float fTime = Platform.Windows.GetTimeInSeconds();

            //
            // Simulate Key-Repeats
            //
            for (int i = 0; i < (int)Key.Count; i++)
            {
                if (KeyData.KeyState[i] && KeyData.Target != Global.KeyboardFocus)
                {
                    KeyData.KeyState[i] = false;
                    continue;
                }

                if (KeyData.KeyState[i] && fTime > KeyData.NextRepeat[i])
                {
                    KeyData.NextRepeat[i] = Gwen.Platform.Windows.GetTimeInSeconds() + KeyRepeatRate;

                    if (Global.KeyboardFocus != null)
                    {
                        Global.KeyboardFocus.onKeyPress((Key)i);
                    }
                }
            }
        }

        public static bool onMouseClicked(Base canvas, int iMouseButton, bool bDown)
        {
            // If we click on a control that isn't a menu we want to close
            // all the open menus. Menus are children of the canvas.
            if (bDown && (null == Global.HoveredControl || !Global.HoveredControl.IsMenuComponent))
            {
                canvas.CloseMenus();
            }

            if (null == Global.HoveredControl) return false;
            if (Global.HoveredControl.GetCanvas() != canvas) return false;
            if (!Global.HoveredControl.IsVisible) return false;
            if (Global.HoveredControl == canvas) return false;

            if (iMouseButton > MaxMouseButtons)
                return false;

            if (iMouseButton == 0) KeyData.LeftMouseDown = bDown;
            else if (iMouseButton == 1) KeyData.RightMouseDown = bDown;

            // Double click.
            // Todo: Shouldn't double click if mouse has moved significantly
            bool bIsDoubleClick = false;

            if (bDown &&
                g_pntLastClickPos.X == MousePosition.X &&
                g_pntLastClickPos.Y == MousePosition.Y &&
                (Platform.Windows.GetTimeInSeconds() - g_fLastClickTime[iMouseButton]) < DoubleClickSpeed)
            {
                bIsDoubleClick = true;
            }

            if (bDown && !bIsDoubleClick)
            {
                g_fLastClickTime[iMouseButton] = Platform.Windows.GetTimeInSeconds();
                g_pntLastClickPos = MousePosition;
            }

            if (bDown)
            {
                FindKeyboardFocus(Global.HoveredControl);
            }

            Global.HoveredControl.UpdateCursor();

            // This tells the child it has been touched, which
            // in turn tells its parents, who tell their parents.
            // This is basically so that Windows can pop themselves
            // to the top when one of their children have been clicked.
            if (bDown)
                Global.HoveredControl.Touch();

#if GWEN_HOOKSYSTEM
            if (bDown)
            {
                if (Hook::CallHook(&Hook::BaseHook::OnControlClicked, Global.HoveredControl, MousePosition.x,
                                   MousePosition.y))
                    return true;
            }
#endif

            switch (iMouseButton)
            {
                case 0:
                    {
                        if (DragAndDrop.onMouseButton(Global.HoveredControl, MousePosition.X, MousePosition.Y, bDown))
                            return true;

                        if (bIsDoubleClick)
                            Global.HoveredControl.onMouseDoubleClickLeft(MousePosition.X, MousePosition.Y);
                        else
                            Global.HoveredControl.onMouseClickLeft(MousePosition.X, MousePosition.Y, bDown);
                        return true;
                    }

                case 1:
                    {
                        if (bIsDoubleClick)
                            Global.HoveredControl.onMouseDoubleClickRight(MousePosition.X, MousePosition.Y);
                        else
                            Global.HoveredControl.onMouseClickRight(MousePosition.X, MousePosition.Y, bDown);
                        return true;
                    }
            }

            return false;
        }

        public static bool onKeyEvent(Base canvas, Key key, bool down)
        {
            if (null==Global.KeyboardFocus) return false;
            if (Global.KeyboardFocus.GetCanvas() != canvas) return false;
            if (!Global.KeyboardFocus.IsVisible) return false;

            int iKey = (int) key;
            if (down)
            {
                if (!KeyData.KeyState[iKey])
                {
                    KeyData.KeyState[iKey] = true;
                    KeyData.NextRepeat[iKey] = Platform.Windows.GetTimeInSeconds() + KeyRepeatDelay;
                    KeyData.Target = Global.KeyboardFocus;

                    return Global.KeyboardFocus.onKeyPress(key);
                }
            }
            else
            {
                if (KeyData.KeyState[iKey])
                {
                    KeyData.KeyState[iKey] = false;

                    // BUG BUG. This causes shift left arrow in textboxes
                    // to not work. What is disabling it here breaking?
                    //KeyData.Target = NULL;

                    return Global.KeyboardFocus.onKeyRelease(key);
                }
            }

            return false;
        }

        private static void UpdateHoveredControl(Base pInCanvas)
        {
            Base pHovered = pInCanvas.GetControlAt(MousePosition.X, MousePosition.Y);

            if (Global.HoveredControl!=null && pHovered != Global.HoveredControl)
            {
                Global.HoveredControl.onMouseLeave();

                pInCanvas.Redraw();
            }

            if (pHovered != Global.HoveredControl)
            {
                Global.HoveredControl = pHovered;

                if (Global.HoveredControl != null)
                    Global.HoveredControl.onMouseEnter();

                pInCanvas.Redraw();
            }

            if (Global.MouseFocus!=null && Global.MouseFocus.GetCanvas() == pInCanvas)
            {
                Global.HoveredControl = Global.MouseFocus;
            }
        }

        private static void FindKeyboardFocus( Base pControl )
        {
            if (null==pControl) return;
            if (pControl.KeyboardInputEnabled)
            {
                //Make sure none of our children have keyboard focus first - todo recursive
                if (pControl.Children.Any(child => child == Global.KeyboardFocus))
                {
                    return;
                }

                pControl.Focus();
                return;
            }

            FindKeyboardFocus(pControl.Parent);
            return;
        }

    }
}
