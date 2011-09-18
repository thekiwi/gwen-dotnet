using System;
using System.Drawing;
using Gwen.Anim;
using Gwen.DragDrop;

namespace Gwen.Control
{
    /// <summary>
    /// Canvas control. It should be the root parent for all other controls.
    /// </summary>
    public class Canvas : Base
    {
        private bool m_NeedsRedraw;
        private float m_Scale;

        private Color m_BackgroundColor;

        // [omeg] these are not created by us, so no disposing
        internal Base FirstTab;
        internal Base NextTab;

        /// <summary>
        /// Scale for rendering.
        /// </summary>
        public float Scale
        {
            get { return m_Scale; }
            set
            {
                if (m_Scale == value)
                    return;

                m_Scale = value;

                if (Skin != null && Skin.Renderer != null)
                    Skin.Renderer.Scale = m_Scale;

                OnScaleChanged();
                Redraw();
            }
        }

        /// <summary>
        /// Background color.
        /// </summary>
        public Color BackgroundColor { get { return m_BackgroundColor; } set { m_BackgroundColor = value; } }

        /// <summary>
        /// In most situations you will be rendering the canvas every frame. 
        /// But in some situations you will only want to render when there have been changes. 
        /// You can do this by checking NeedsRedraw.
        /// </summary>
        public bool NeedsRedraw { get { return m_NeedsRedraw; } set { m_NeedsRedraw = value; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="Canvas"/> class.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        public Canvas(Skin.Base skin)
        {
            SetBounds(0, 0, 10000, 10000);
            SetSkin(skin);
            Scale = 1.0f;
            BackgroundColor = Color.White;
            ShouldDrawBackground = false;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            // kill everything since we're the top-level control
            foreach (Base child in Children)
            {
                child.Dispose();
            }
            base.Dispose(); // inner panel if any
        }

        /// <summary>
        /// Re-renders the control, invalidates cached texture.
        /// </summary>
        public override void Redraw()
        {
            NeedsRedraw = true;
            base.Redraw();
        }
        
        // Children call parent.GetCanvas() until they get to 
        // this top level function.
        public override Canvas GetCanvas()
        {
            return this;
        }

        /// <summary>
        /// Additional initialization (which is sometimes not appropriate in the constructor)
        /// </summary>
        protected void Initialize()
        {

        }

        /// <summary>
        /// Renders the canvas. Call in your rendering loop.
        /// </summary>
        public void RenderCanvas()
        {
            DoThink();

            Renderer.Base render = Skin.Renderer;

            render.Begin();

            RecurseLayout(Skin);

            render.ClipRegion = Bounds;
            render.RenderOffset = Point.Empty;
            render.Scale = Scale;

            if (ShouldDrawBackground)
            {
                render.DrawColor = m_BackgroundColor;
                render.DrawFilledRect(RenderBounds);
            }

            DoRender(Skin);

            DragAndDrop.RenderOverlay(this, Skin);

            Gwen.ToolTip.RenderToolTip(Skin);

            render.EndClip();

            render.End();
        }

        /// <summary>
        /// Renders the control using specified skin.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Render(Skin.Base skin)
        {
            //skin.Renderer.rnd = new Random(1);
            base.Render(skin);
            m_NeedsRedraw = false;
        }

        /// <summary>
        /// Handler invoked when control's bounds change.
        /// </summary>
        /// <param name="oldBounds">Old bounds.</param>
        protected override void OnBoundsChanged(Rectangle oldBounds)
        {
            base.OnBoundsChanged(oldBounds);
            InvalidateChildren(true);
        }

        /// <summary>
        /// Processes input and layout.
        /// </summary>
        private void DoThink()
        {
            if (IsHidden)
                return;

            Animation.GlobalThink();

            // Reset tabbing
            {
                NextTab = null;
                FirstTab = null;
            }

            // Check has focus etc..
            RecurseLayout(Skin);

            // If we didn't have a next tab, cycle to the start.
            if (NextTab == null)
                NextTab = FirstTab;

            Input.Input.OnCanvasThink(this);
        }

        /// <summary>
        /// Handles mouse movement events. Called from Input subsystems.
        /// </summary>
        /// <returns>True if handled.</returns>
        public bool Input_MouseMoved(int x, int y, int dx, int dy)
        {
            if (IsHidden)
                return false;

            // Todo: Handle scaling here..
            //float fScale = 1.0f / Scale();

            Input.Input.OnMouseMoved(this, x, y, dx, dy);

            if (Global.HoveredControl == null) return false;
            if (Global.HoveredControl == this) return false;
            if (Global.HoveredControl.GetCanvas() != this) return false;

            Global.HoveredControl.InputMouseMoved(x, y, dx, dy);
            Global.HoveredControl.UpdateCursor();

            DragAndDrop.OnMouseMoved(Global.HoveredControl, x, y);
            return true;
        }

        /// <summary>
        /// Handles mouse button events. Called from Input subsystems.
        /// </summary>
        /// <returns>True if handled.</returns>
        public bool Input_MouseButton(int button, bool down)
        {
            if (IsHidden) return false;

            return Input.Input.OnMouseClicked(this, button, down);
        }

        /// <summary>
        /// Handles keyboard events. Called from Input subsystems.
        /// </summary>
        /// <returns>True if handled.</returns>
        public bool Input_Key(Key key, bool down)
        {
            if (IsHidden) return false;
            if (key <= Key.Invalid) return false;
            if (key >= Key.Count) return false;

            return Input.Input.OnKeyEvent(this, key, down);
        }

        /// <summary>
        /// Handles keyboard events. Called from Input subsystems.
        /// </summary>
        /// <returns>True if handled.</returns>
        public bool Input_Character(char chr)
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

            return Global.KeyboardFocus.InputChar(chr);
        }

        /// <summary>
        /// Handles the mouse wheel events. Called from Input subsystems.
        /// </summary>
        /// <returns>True if handled.</returns>
        public bool Input_MouseWheel(int val)
        {
            if (IsHidden) return false;
            if (Global.HoveredControl == null) return false;
            if (Global.HoveredControl == this) return false;
            if (Global.HoveredControl.GetCanvas() != this) return false;

            return Global.HoveredControl.InputMouseWheeled(val);
        }
    }
}
