using System;
using System.Collections.Generic;
using System.Drawing;
using Gwen.Anim;
using Gwen.DragDrop;

namespace Gwen.Controls
{
    public class Canvas : Base
    {
        private bool m_bNeedsRedraw;
        private bool m_bAnyDelete;
        private double m_fScale;

        private List<Base> m_DeleteList;
        private HashSet<Base> m_DeleteSet;

        private Color m_BackgroundColor;

        internal Base FirstTab;
        internal Base NextTab;

        public double Scale
        {
            get { return m_fScale; }
            set
            {
                if (m_fScale == value)
                    return;

                m_fScale = value;

                if (m_Skin != null && m_Skin.Renderer != null)
                    m_Skin.Renderer.Scale = m_fScale;

                onScaleChanged();
                Redraw();
            }
        }
        public bool DrawBackground { get { return m_bDrawBackground; } set { m_bDrawBackground = value; } }
        public Color BackgroundColor { get { return m_BackgroundColor; } set { m_BackgroundColor = value; } }

        // In most situations you will be rendering the canvas
        // every frame. But in some situations you will only want
        // to render when there have been changes. You can do this
        // by checking NeedsRedraw().
        public bool NeedsRedraw { get { return m_bNeedsRedraw; } set { m_bNeedsRedraw = value; } }

        public Canvas(Skin.Base skin)
        {
            SetBounds(0, 0, 10000, 10000);
            SetSkin(skin);
            Scale = 1.0f;
            BackgroundColor = Color.FromArgb(255, 255, 255, 255);
            DrawBackground = false;
        }

        // was private+friend
        internal void PreDelete(Base control)
        {
            if (m_bAnyDelete)
            {
                if (m_DeleteSet.Contains(control))
                {
                    m_DeleteList.Remove(control);
                    m_DeleteSet.Remove(control);
                    m_bAnyDelete = m_DeleteSet.Count > 0;
                }
            }
        }

        // Childpanels call parent->GetCanvas() until they get to 
        // this top level function.
        public override Canvas GetCanvas()
        {
            return this;
        }

        // For additional initialization 
        // (which is sometimes not appropriate in the constructor)
        protected virtual void Initialize()
        {

        }

        // You should call this to render your canvas.
        public virtual void RenderCanvas()
        {
            DoThink();

            Renderer.Base render = m_Skin.Renderer;

            render.Begin();

            RecurseLayout(m_Skin);

            render.ClipRegion = Bounds;
            render.RenderOffset = Point.Empty;
            render.Scale = Scale;

            if (m_bDrawBackground)
            {
                render.DrawColor = m_BackgroundColor;
                render.DrawFilledRect(RenderBounds);
            }

            DoRender(m_Skin);

            DragAndDrop.RenderOverlay(this, m_Skin);

            Gwen.ToolTip.RenderToolTip(m_Skin);

            render.EndClip();

            render.End();

            ProcessDelayedDeletes();
        }

        // Internal. Do not call directly.
        protected override void Render(Skin.Base skin)
        {
            base.Render(skin);
            m_bNeedsRedraw = false;
        }

        internal override void onBoundsChanged(Rectangle oldBounds)
        {
            base.onBoundsChanged(oldBounds);
            InvalidateChildren(true);
        }

        // Call this whenever you want to process input. This
        // is usually once a frame..
        protected virtual void DoThink()
        {
            if (IsHidden)
                return;

            Animation.GlobalThink();

            // Reset tabbing
            {
                NextTab = null;
                FirstTab = null;
            }

            ProcessDelayedDeletes();
            // Check has focus etc..
            RecurseLayout(m_Skin);

            // If we didn't have a next tab, cycle to the start.
            if (NextTab == null)
                NextTab = FirstTab;

            Input.Input.onCanvasThink(this);
        }

        internal virtual void AddDelayedDelete(Base control)
        {
            if (!m_bAnyDelete || !m_DeleteSet.Contains(control))
            {
                m_bAnyDelete = true;
                m_DeleteSet.Add(control);
                m_DeleteList.Add(control);
            }
        }

        internal virtual void ProcessDelayedDeletes()
        {
            while (m_bAnyDelete)
            {
                m_bAnyDelete = false;

                m_DeleteList.Clear();
                m_DeleteSet.Clear();
                // no deletes in c# ;)
            }
        }

        public virtual void Release()
        {
            // memory management, no need
        }

        public virtual bool InputMouseMoved(int x, int y, int dx, int dy)
        {
            if (IsHidden)
                return false;

            // Todo: Handle scaling here..
            //float fScale = 1.0f / Scale();

            Input.Input.onMouseMoved(this, x, y, dx, dy);

            if (Global.HoveredControl == null) return false;
            if (Global.HoveredControl == this) return false;
            if (Global.HoveredControl.GetCanvas() != this) return false;

            Global.HoveredControl.onMouseMoved(x, y, dx, dy);
            Global.HoveredControl.UpdateCursor();

            DragAndDrop.onMouseMoved(Global.HoveredControl, x, y);
            return true;
        }

        public virtual bool InputMouseButton(int button, bool pressed)
        {
            if (IsHidden) return false;

            return Input.Input.onMouseClicked(this, button, pressed);
        }

        public virtual bool InputKey(Key key, bool pressed)
        {
            if (IsHidden) return false;
            if (key <= Key.Invalid) return false;
            if (key >= Key.Count) return false;

            return Input.Input.onKeyEvent(this, key, pressed);
        }

        public virtual bool InputCharacter(char chr)
        {
            if (IsHidden) return false;
            if (char.IsControl(chr)) return false;

            //Handle Accelerators
            if (Input.Input.HandleAccelerator(this, chr))
                return true;

            //Handle characters
            if (Global.KeyboardFocus == null) return false;
            if (Global.KeyboardFocus.GetCanvas() != this) return false;
            if (!Global.KeyboardFocus.IsVisible) return false;
            if (Input.Input.IsControlDown) return false;

            return Global.KeyboardFocus.onChar(chr);
        }

        public virtual bool InputMouseWheel(int val)
        {
            if (IsHidden) return false;
            if (Global.HoveredControl == null) return false;
            if (Global.HoveredControl == this) return false;
            if (Global.HoveredControl.GetCanvas() != this) return false;

            return Global.HoveredControl.onMouseWheeled(val);
        }
    }
}
