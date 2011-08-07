using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Gwen.Controls;

namespace Gwen.DragDrop
{
    public static class DragAndDrop
    {
        public static Package CurrentPackage;
        public static Base HoveredControl;
        public static Base SourceControl;

        private static Base LastPressedControl;
        private static Base NewHoveredControl;
        private static Point LastPressedPos;
        private static int m_iMouseX;
        private static int m_iMouseY;

        private static bool OnDrop(int x, int y)
        {
            bool bSuccess = false;

            if (HoveredControl != null)
            {
                HoveredControl.DragAndDrop_HoverLeave(CurrentPackage);
                bSuccess = HoveredControl.DragAndDrop_HandleDrop(CurrentPackage, x, y);
            }

            // Report back to the source control, to tell it if we've been successful.
            SourceControl.DragAndDrop_EndDragging(bSuccess, x, y);

            CurrentPackage = null;
            SourceControl = null;

            return true;
        }

        private static bool ShouldStartDraggingControl( int x, int y )
        {
            // We're not holding a control down..
            if (LastPressedControl == null) 
                return false;

            // Not been dragged far enough
            int iLength = Math.Abs(x - LastPressedPos.X) + Math.Abs(y - LastPressedPos.Y);
            if (iLength < 5) 
                return false;

            // Create the dragging package

            CurrentPackage = LastPressedControl.DragAndDrop_GetPackage(LastPressedPos.X, LastPressedPos.Y);

            // We didn't create a package!
            if (CurrentPackage == null)
            {
                LastPressedControl = null;
                SourceControl = null;
                return false;
            }

            // Now we're dragging something!
            SourceControl = LastPressedControl;
            Global.MouseFocus = null;
            LastPressedControl = null;
            CurrentPackage.DrawControl = null;

            // Some controls will want to decide whether they should be dragged at that moment.
            // This function is for them (it defaults to true)
            if (!SourceControl.DragAndDrop_ShouldStartDrag())
            {
                SourceControl = null;
                CurrentPackage = null;
                return false;
            }

            SourceControl.DragAndDrop_StartDragging(CurrentPackage, LastPressedPos.X, LastPressedPos.Y);

            return true;
        }

        private static void UpdateHoveredControl(Base control, int x, int y)
        {
            //
            // We use this global variable to represent our hovered control
            // That way, if the new hovered control gets deleted in one of the
            // Hover callbacks, we won't be left with a hanging pointer.
            // This isn't ideal - but it's minimal.
            //
            NewHoveredControl = control;

            // Nothing to change..
            if (DragAndDrop.HoveredControl == NewHoveredControl) 
                return;

            // We changed - tell the old hovered control that it's no longer hovered.
            if (HoveredControl!=null && HoveredControl != NewHoveredControl)
                HoveredControl.DragAndDrop_HoverLeave(CurrentPackage);

            // If we're hovering where the control came from, just forget it.
            // By changing it to null here we're not going to show any error cursors
            // it will just do nothing if you drop it.
            if (NewHoveredControl == SourceControl)
                NewHoveredControl = null;

            // Check to see if the new potential control can accept this type of package.
            // If not, ignore it and show an error cursor.
            while (NewHoveredControl!=null && !NewHoveredControl.DragAndDrop_CanAcceptPackage(CurrentPackage))
            {
                // We can't drop on this control, so lets try to drop
                // onto its parent..
                NewHoveredControl = NewHoveredControl.Parent;

                // Its parents are dead. We can't drop it here.
                // Show the NO WAY cursor.
                if (NewHoveredControl == null)
                {
                    Platform.Windows.SetCursor(Cursors.No);
                }
            }

            // Become out new hovered control
            DragAndDrop.HoveredControl = NewHoveredControl;

            // If we exist, tell us that we've started hovering.
            if (HoveredControl!=null)
            {
                HoveredControl.DragAndDrop_HoverEnter(CurrentPackage, x, y);
            }

            NewHoveredControl = null;
        }

        public static bool Start(Base pControl, Package pPackage)
        {
            if (CurrentPackage != null)
            {
                return false;
            }

            CurrentPackage = pPackage;
            SourceControl = pControl;
            return true;
        }

        public static bool OnMouseButton(Base pHoveredControl, int x, int y, bool bDown)
        {
            if (!bDown)
            {
                LastPressedControl = null;

                // Not carrying anything, allow normal actions
                if (CurrentPackage == null)
                    return false;

                // We were carrying something, drop it.
                OnDrop(x, y);
                return true;
            }

            if (pHoveredControl == null) 
                return false;
            if (!pHoveredControl.DragAndDrop_Draggable()) 
                return false;

            // Store the last clicked on control. Don't do anything yet, 
            // we'll check it in OnMouseMoved, and if it moves further than
            // x pixels with the mouse down, we'll start to drag.
            LastPressedPos = new Point(x, y);
            LastPressedControl = pHoveredControl;

            return false;
        }

        public static void OnMouseMoved(Base hoveredControl, int x, int y)
        {
            // Always keep these up to date, they're used to draw the dragged control.
            m_iMouseX = x;
            m_iMouseY = y;

            // If we're not carrying anything, then check to see if we should
            // pick up from a control that we're holding down. If not, then forget it.
            if (CurrentPackage==null && !ShouldStartDraggingControl(x, y))
                return;

            // Swap to this new hovered control and notify them of the change.
            UpdateHoveredControl(hoveredControl, x, y);

            if (HoveredControl == null) 
                return;

            // Update the hovered control every mouse move, so it can show where
            // the dropped control will land etc..
            HoveredControl.DragAndDrop_Hover(CurrentPackage, x, y);

            // Override the cursor - since it might have been set my underlying controls
            // Ideally this would show the 'being dragged' control. TODO
            Platform.Windows.SetCursor(Cursors.Default);

            hoveredControl.Redraw();
        }

        public static void RenderOverlay(Canvas pCanvas, Skin.Base skin)
        {
            if (CurrentPackage == null) 
                return;
            if (CurrentPackage.DrawControl == null) 
                return;

            Point pntOld = skin.Renderer.RenderOffset;

            skin.Renderer.AddRenderOffset(new Rectangle(
                m_iMouseX - SourceControl.X - CurrentPackage.HoldOffset.X,
                m_iMouseY - SourceControl.Y - CurrentPackage.HoldOffset.Y, 0, 0));
            CurrentPackage.DrawControl.DoRender(skin);

            skin.Renderer.RenderOffset = pntOld;
        }

        public static void ControlDeleted(Base control)
        {
            if (SourceControl == control)
            {
                SourceControl = null;
                CurrentPackage = null;
                HoveredControl = null;
                LastPressedControl = null;
            }

            if (LastPressedControl == control)
                LastPressedControl = null;

            if (HoveredControl == control)
                HoveredControl = null;

            if (NewHoveredControl == control)
                NewHoveredControl = null;
        }
    }
}
