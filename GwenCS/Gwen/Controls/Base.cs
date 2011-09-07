using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using Gwen.Anim;
using Gwen.DragDrop;

namespace Gwen.Controls
{
    public class Base : IDisposable
    {
        // [omeg] C# delegates/events instead of Gwen.Event
        public delegate void ControlCallback(Base control);

        // The logical parent
        // It's usually what you expect, the control you've parented it to.
        protected Base m_Parent;

        // This is the panel's actual parent - most likely the logical 
        //  parent's InnerPanel (if it has one). You should rarely need this.
        protected Base m_ActualParent;

        // If the innerpanel exists our children will automatically
        //  become children of that instead of us - allowing us to move
        //  them all around by moving that panel (useful for scrolling etc)
        protected Base m_InnerPanel;

        protected Base m_ToolTip;

        protected Skin.Base m_Skin;

        protected Rectangle m_Bounds;
        protected Rectangle m_RenderBounds;
        protected Rectangle m_InnerBounds;
        protected Padding m_Padding;
        protected Margin m_Margin;

        protected String m_Name;

        protected bool m_RestrictToParent;
        protected bool m_Disabled;
        protected bool m_Hidden;
        protected bool m_MouseInputEnabled;
        protected bool m_KeyboardInputEnabled;
        protected bool m_DrawBackground;

        protected Pos m_Dock;

        protected Cursor m_Cursor;

        protected bool m_Tabable;

        protected bool m_NeedsLayout;
        protected bool m_CacheTextureDirty;
        protected bool m_CacheToTexture;

        protected Package m_DragAndDrop_Package;

        private object m_UserData;

        // Childrens List
        internal List<Base> Children;

        // Default Events
        public event ControlCallback OnHoverEnter;
        public event ControlCallback OnHoverLeave;

        // accelerator map
        protected Dictionary<String, ControlCallback> m_Accelerators;

        public List<Base> InnerChildren
        {
            get
            {
                if (m_InnerPanel != null)
                    return m_InnerPanel.InnerChildren;
                return Children;
            }
        }

        public Base Parent
        {
            get { return m_Parent; }
            set
            {
                if (m_Parent == value)
                    return;

                if (m_Parent != null)
                {
                    m_Parent.RemoveChild(this);
                }

                m_Parent = value;
                m_ActualParent = null;

                if (m_Parent != null)
                {
                    m_Parent.AddChild(this);
                }
            }
        }

        public Pos Dock
        {
            get { return m_Dock; }
            set
            {
                if (m_Dock == value)
                    return;

                m_Dock = value;

                Invalidate();
                InvalidateParent();
            }
        }

        public int ChildrenCount { get { return Children.Count; } }

        public Skin.Base Skin
        {
            get
            {
                if (m_Skin != null)
                    return m_Skin;
                if (m_Parent != null)
                    return m_Parent.Skin;

                throw new Exception("GetSkin: null");
                return null;
            }
        }

        public Base ToolTip
        {
            get { return m_ToolTip; }
            set
            {
                m_ToolTip = value;
                if (m_ToolTip != null)
                {
                    m_ToolTip.Parent = this;
                    m_ToolTip.IsHidden = true;
                }
            }
        }

        public virtual bool IsMenuComponent
        {
            get
            {
                if (m_Parent == null)
                    return false;
                return m_Parent.IsMenuComponent;
            }
        }

        public virtual bool ShouldClip { get { return true; } }

        public Padding Padding
        {
            get { return m_Padding; }
            set
            {
                if (m_Padding == value)
                    return;

                m_Padding = value;
                Invalidate();
                InvalidateParent();
            }
        }

        public Margin Margin
        {
            get { return m_Margin; }
            set
            {
                if (m_Margin == value)
                    return;

                m_Margin = value;
                Invalidate();
                InvalidateParent();
            }
        }

        public virtual bool IsOnTop { get { return this == Parent.Children.First(); } }
        public object UserData { get { return m_UserData; } set { m_UserData = value; } }
        public bool IsHovered { get { return Global.HoveredControl == this; } }
        public bool ShouldDrawHover { get { return Global.MouseFocus == this || Global.MouseFocus == null; } }
        public bool HasFocus { get { return Global.KeyboardFocus == this; } }
        public bool IsDisabled { get { return m_Disabled; } set { m_Disabled = value; } }
        public virtual bool IsHidden { get { return m_Hidden; } set { if (value == m_Hidden) return; m_Hidden = value; Invalidate(); } }
        public bool RestrictToParent { get { return m_RestrictToParent; } set { m_RestrictToParent = value; } }
        public bool MouseInputEnabled { get { return m_MouseInputEnabled; } set { m_MouseInputEnabled = value; } }
        public bool KeyboardInputEnabled { get { return m_KeyboardInputEnabled; } set { m_KeyboardInputEnabled = value; } }
        public Cursor Cursor { get { return m_Cursor; } set { m_Cursor = value; } }
        public bool IsTabable { get { return m_Tabable; } set { m_Tabable = value; } }
        public bool ShouldDrawBackground { get { return m_DrawBackground; } set { m_DrawBackground = value; } }
        public bool ShouldCacheToTexture { get { return m_CacheToTexture; } set { m_CacheToTexture = value; /*Children.ForEach(x => x.ShouldCacheToTexture=value);*/ } }
        public String Name { get { return m_Name; } set { m_Name = value; } }
        public Rectangle Bounds { get { return m_Bounds; } }
        public bool NeedsLayout { get { return m_NeedsLayout; } }
        public Rectangle RenderBounds { get { return m_RenderBounds; } }
        public Rectangle InnerBounds { get { return m_InnerBounds; } }
        public virtual bool AccelOnlyFocus { get { return false; } }
        public virtual bool NeedsInputChars { get { return false; } }
        public Point MinimumSize { get { return m_MinimumSize; } set { m_MinimumSize = value; } }
        public Point MaximumSize { get { return m_MaximumSize; } set { m_MaximumSize = value; } }

        private Point m_MinimumSize = new Point(1, 1);
        private Point m_MaximumSize = new Point(Global.MaxCoord, Global.MaxCoord);

        // Returns false if this control or its parents are hidden
        public bool IsVisible
        {
            get
            {
                if (IsHidden)
                    return false;

                if (Parent != null)
                    return Parent.IsVisible;

                return true;
            }
        }

        public int X { get { return m_Bounds.X; } set { SetPos(value, Y); } }
        public int Y { get { return m_Bounds.Y; } set { SetPos(X, value); } }
        public int Width { get { return m_Bounds.Width; } set { SetSize(value, Height); } }
        public int Height { get { return m_Bounds.Height; } set { SetSize(Width, value); } }
        public int Bottom { get { return m_Bounds.Bottom + m_Margin.Bottom; } }
        public int Right { get { return m_Bounds.Right + m_Margin.Right; } }

        public Base(Base parent = null)
        {
            Children = new List<Base>();
            m_Accelerators = new Dictionary<string, ControlCallback>();

            Parent = parent;

            m_Hidden = false;
            m_Bounds = new Rectangle(0, 0, 10, 10);
            m_Padding = new Padding(0, 0, 0, 0);
            m_Margin = new Margin(0, 0, 0, 0);

            RestrictToParent = false;

            MouseInputEnabled = true;
            KeyboardInputEnabled = false;

            Invalidate();
            Cursor = Cursors.Default;
            //ToolTip = null;
            IsTabable = false;
            ShouldDrawBackground = true;
            m_Disabled = false;
            m_CacheTextureDirty = true;
            m_CacheToTexture = false;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public virtual void Dispose()
        {
            if (Global.HoveredControl == this)
                Global.HoveredControl = null;
            if (Global.KeyboardFocus == this)
                Global.KeyboardFocus = null;
            if (Global.MouseFocus == this)
                Global.MouseFocus = null;

            DragAndDrop.ControlDeleted(this);
            Gwen.ToolTip.ControlDeleted(this);
            Animation.Cancel(this);

            if (m_InnerPanel != null)
                m_InnerPanel.Dispose();

            if (m_ToolTip != null)
                m_ToolTip.Dispose();
        }

        public void DefaultAccel(Base control)
        {
            AcceleratePressed();
        }

        protected virtual void AcceleratePressed()
        {

        }

        public virtual void Hide()
        {
            IsHidden = true;
        }

        public virtual void Show()
        {
            IsHidden = false;
        }

        public virtual void SetToolTipText(String text)
        {
            if (ToolTip != null)
                ToolTip.Dispose();
            Label tooltip = new Label(this);
            tooltip.SetText(text);
            tooltip.TextColorOverride = Skin.Colors.TooltipText;
            tooltip.Padding = new Padding(5, 3, 5, 3);
            tooltip.SizeToContents();

            ToolTip = tooltip;
        }

        public void InvalidateChildren(bool recursive = false)
        {
            foreach (Base child in Children)
            {
                child.Invalidate();
                if (recursive)
                    child.InvalidateChildren(true);
            }

            if (m_InnerPanel != null)
            {
                foreach (Base child in m_InnerPanel.Children)
                {
                    child.Invalidate();
                    if (recursive)
                        child.InvalidateChildren(true);
                }
            }
        }

        public virtual void Invalidate()
        {
            m_NeedsLayout = true;
            m_CacheTextureDirty = true;
        }

        public virtual void SendToBack()
        {
            if (m_ActualParent == null)
                return;
            if (m_ActualParent.Children.First() == this)
                return;

            m_ActualParent.Children.Remove(this);
            m_ActualParent.Children.Insert(0, this);

            InvalidateParent();
        }

        public virtual void BringToFront()
        {
            if (m_ActualParent == null)
                return;
            if (m_ActualParent.Children.Last() == this)
                return;

            m_ActualParent.Children.Remove(this);
            m_ActualParent.Children.Add(this);
            InvalidateParent();
            Redraw();
        }

        public virtual Canvas GetCanvas()
        {
            Base canvas = m_Parent;
            if (canvas == null)
                return null;

            return canvas.GetCanvas();
        }

        public virtual void BringNextToControl(Base child, bool behind)
        {
            if (null == m_ActualParent)
                return;

            m_ActualParent.Children.Remove(this);

            // todo: validate
            int idx = m_ActualParent.Children.IndexOf(child);
            if (idx == m_ActualParent.Children.Count - 1)
            {
                BringToFront();
                return;
            }

            if (behind)
            {
                ++idx;

                if (idx == m_ActualParent.Children.Count - 1)
                {
                    BringToFront();
                    return;
                }
            }

            m_ActualParent.Children.Insert(idx, this);
            InvalidateParent();
        }

        public virtual Base FindChildByName(String name, bool recursive = false)
        {
            Base b = Children.Find(x => x.m_Name == name);
            if (b != null)
                return b;

            if (recursive)
            {
                foreach (Base child in Children)
                {
                    b = child.FindChildByName(name, true);
                    if (b != null)
                        return b;
                }
            }
            return null;
        }

        protected virtual void AddChild(Base child)
        {
            if (m_InnerPanel != null)
            {
                m_InnerPanel.AddChild(child);
                return;
            }

            Children.Add(child);
            onChildAdded(child);

            child.m_ActualParent = this;
        }

        protected virtual void RemoveChild(Base child)
        {
            // If we removed our innerpanel
            // remove our pointer to it
            if (m_InnerPanel == child)
            {
                m_InnerPanel = null;
            }

            if (m_InnerPanel != null)
            {
                m_InnerPanel.RemoveChild(child);
            }

            Children.Remove(child);
            onChildRemoved(child);
        }

        public virtual void RemoveAllChildren()
        {
            // todo: probably shouldn't invalidate after each removal
            while (Children.Count > 0)
                RemoveChild(Children[0]);
        }

        internal virtual void onChildAdded(Base child)
        {
            Invalidate();
        }

        internal virtual void onChildRemoved(Base child)
        {
            Invalidate();
        }

        public virtual void MoveBy(int x, int y)
        {
            SetBounds(X + x, Y + y, Width, Height);
        }

        public virtual void MoveTo(float x, float y)
        {
            MoveTo((int)x, (int)y);
        }

        public virtual void MoveTo(int x, int y)
        {
            if (m_RestrictToParent && (Parent != null))
            {
                Base parent = Parent;
                if (x - Padding.Left < parent.Margin.Left)
                    x = parent.Margin.Left + Padding.Left;
                if (y - Padding.Top < parent.Margin.Top)
                    y = parent.Margin.Top + Padding.Top;
                if (x + Width + Padding.Right > parent.Width - parent.Margin.Right)
                    x = parent.Width - parent.Margin.Right - Width - Padding.Right;
                if (y + Height + Padding.Bottom > parent.Height - parent.Margin.Bottom)
                    y = parent.Height - parent.Margin.Bottom - Height - Padding.Bottom;
            }

            SetBounds(x, y, Width, Height);
        }

        public virtual void SetPos(float x, float y)
        {
            SetPos((int)x, (int)y);
        }

        public virtual void SetPos(int x, int y)
        {
            SetBounds(x, y, Width, Height);
        }

        public virtual bool SetSize(int w, int h)
        {
            return SetBounds(X, Y, w, h);
        }

        public virtual bool SetBounds(Rectangle bounds)
        {
            return SetBounds(bounds.X, bounds.Y, bounds.Width, bounds.Height);
        }

        public virtual bool SetBounds(float x, float y, float w, float h)
        {
            return SetBounds((int)x, (int)y, (int)w, (int)h);
        }

        public virtual bool SetBounds(int x, int y, int w, int h)
        {
            if (m_Bounds.X == x &&
                 m_Bounds.Y == y &&
                 m_Bounds.Width == w &&
                 m_Bounds.Height == h)
                return false;

            Rectangle oldBounds = Bounds;

            m_Bounds.X = x;
            m_Bounds.Y = y;

            m_Bounds.Width = w;
            m_Bounds.Height = h;

            onBoundsChanged(oldBounds);

            return true;
        }

        public virtual void Position(Pos pos, int xpadding = 0, int ypadding = 0)
        {
            int w = Parent.Width;
            int h = Parent.Height;
            Padding padding = Parent.Padding;

            int x = X;
            int y = Y;
            if (pos.HasFlag(Pos.Left)) x = padding.Left + xpadding;
            if (pos.HasFlag(Pos.Right)) x = w - Width - padding.Right - xpadding;
            if (pos.HasFlag(Pos.CenterH))
                x = (int)(padding.Left + xpadding + (w - Width - padding.Left - padding.Right) * 0.5f);

            if (pos.HasFlag(Pos.Top)) y = padding.Top + ypadding;
            if (pos.HasFlag(Pos.Bottom)) y = h - Height - padding.Bottom - ypadding;
            if (pos.HasFlag(Pos.CenterV))
                y = (int)(padding.Top + ypadding + (h - Height - padding.Bottom - padding.Top) * 0.5f);

            SetPos(x, y);
        }

        internal virtual void onBoundsChanged(Rectangle oldBounds)
        {
            //Anything that needs to update on size changes
            //Iterate my children and tell them I've changed
            //
            if (Parent != null)
                Parent.onChildBoundsChanged(oldBounds, this);


            if (m_Bounds.Width != oldBounds.Width || m_Bounds.Height != oldBounds.Height)
            {
                Invalidate();
            }

            Redraw();
            UpdateRenderBounds();
        }

        internal virtual void onScaleChanged()
        {
            foreach (Base child in Children)
            {
                child.onScaleChanged();
            }
        }

        internal virtual void onChildBoundsChanged(Rectangle oldChildBounds, Base child)
        {

        }

        protected virtual void Render(Skin.Base skin)
        {

        }

        public virtual void DoCacheRender(Skin.Base skin, Base master)
        {
            Renderer.Base render = skin.Renderer;
            Renderer.ICacheToTexture cache = render.CTT;

            if (cache == null)
                return;

            Point oldRenderOffset = render.RenderOffset;
            Rectangle oldRegion = render.ClipRegion;

            if (this != master)
            {
                render.AddRenderOffset(Bounds);
                render.AddClipRegion(Bounds);
            }
            else
            {
                render.RenderOffset = Point.Empty;
                render.ClipRegion = new Rectangle(0, 0, Width, Height);
            }

            if (m_CacheTextureDirty && render.ClipRegionVisible)
            {
                render.StartClip();

                if (ShouldCacheToTexture)
                    cache.SetupCacheTexture(this);

                //Render myself first
                //var old = render.ClipRegion;
                //render.ClipRegion = Bounds;
                //var old = render.RenderOffset;
                //render.RenderOffset = new Point(Bounds.X, Bounds.Y);
                Render(skin);
                //render.RenderOffset = old;
                //render.ClipRegion = old;

                if (Children.Count > 0)
                {
                    //Now render my kids
                    foreach (Base child in Children)
                    {
                        if (child.IsHidden)
                            continue;
                        child.DoCacheRender(skin, master);
                    }
                }

                if (ShouldCacheToTexture)
                {
                    cache.FinishCacheTexture(this);
                    m_CacheTextureDirty = false;
                }
            }

            render.ClipRegion = oldRegion;
            render.StartClip();
            render.RenderOffset = oldRenderOffset;

            if (ShouldCacheToTexture)
                cache.DrawCachedControlTexture(this);
        }

        internal virtual void DoRender(Skin.Base skin)
        {
            // If this control has a different skin, 
            // then so does its children.
            if (m_Skin != null)
                skin = m_Skin;

            // Do think
            Think();

            Renderer.Base render = skin.Renderer;

            if (render.CTT != null && ShouldCacheToTexture)
            {
                DoCacheRender(skin, this);
                return;
            }

            RenderRecursive(skin, Bounds);
        }

        protected virtual void RenderRecursive(Skin.Base skin, Rectangle clipRect)
        {
            Renderer.Base render = skin.Renderer;
            Point oldRenderOffset = render.RenderOffset;

            render.AddRenderOffset(clipRect);

            RenderUnder(skin);

            Rectangle oldRegion = render.ClipRegion;

            if (ShouldClip)
            {
                render.AddClipRegion(clipRect);

                if (!render.ClipRegionVisible)
                {
                    render.RenderOffset = oldRenderOffset;
                    render.ClipRegion = oldRegion;
                    return;
                }

                render.StartClip();
            }

            //Render myself first
            Render(skin);

            if (Children.Count > 0)
            {
                //Now render my kids
                foreach (Base child in Children)
                {
                    if (child.IsHidden)
                        continue;
                    child.DoRender(skin);
                }
            }

            render.ClipRegion = oldRegion;
            render.StartClip();
            RenderOver(skin);

            RenderFocus(skin);

            render.RenderOffset = oldRenderOffset;
        }

        public virtual void SetSkin(Skin.Base skin, bool doChildren = false)
        {
            if (m_Skin == skin)
                return;
            m_Skin = skin;
            Invalidate();
            Redraw();
            onSkinChanged(skin);

            if (doChildren)
            {
                foreach (Base child in Children)
                {
                    child.SetSkin(skin, true);
                }
            }
        }

        internal virtual void onSkinChanged(Skin.Base newSkin)
        {

        }

        internal virtual bool onMouseWheeled(int delta)
        {
            if (m_ActualParent != null)
                return m_ActualParent.onMouseWheeled(delta);

            return false;
        }

        internal virtual void onMouseMoved(int x, int y, int dx, int dy)
        {

        }

        internal virtual void onMouseClickLeft(int x, int y, bool down)
        {

        }

        internal virtual void onMouseClickRight(int x, int y, bool down)
        {

        }

        internal virtual void onMouseDoubleClickLeft(int x, int y)
        {
            onMouseClickLeft(x, y, true); // [omeg] should this be called?
        }

        internal virtual void onMouseDoubleClickRight(int x, int y)
        {
            onMouseClickRight(x, y, true); // [omeg] should this be called?
        }

        internal virtual void onMouseEnter()
        {
            if (OnHoverEnter != null)
                OnHoverEnter.Invoke(this);

            if (ToolTip != null)
                Gwen.ToolTip.Enable(this);
            else if (Parent != null && Parent.ToolTip != null)
                Gwen.ToolTip.Enable(Parent);

            Redraw();
        }

        internal virtual void onMouseLeave()
        {
            if (OnHoverLeave != null)
                OnHoverLeave.Invoke(this);

            if (ToolTip != null)
                Gwen.ToolTip.Disable(this);

            Redraw();
        }

        public virtual void Focus()
        {
            if (Global.KeyboardFocus == this)
                return;

            if (Global.KeyboardFocus != null)
                Global.KeyboardFocus.onLostKeyboardFocus();

            Global.KeyboardFocus = this;
            onKeyboardFocus();
            Redraw();
        }

        public virtual void Blur()
        {
            if (Global.KeyboardFocus != this)
                return;

            Global.KeyboardFocus = null;
            onLostKeyboardFocus();
            Redraw();
        }

        public virtual void Touch()
        {
            if (Parent != null)
                Parent.onChildRemoved(this);
        }

        internal virtual void onChildTouched(Base control)
        {
            Touch();
        }

        public virtual Base GetControlAt(int x, int y)
        {
            if (IsHidden)
                return null;

            if (x < 0 || y < 0 || x >= Width || y >= Height)
                return null;

            // todo: convert to linq FindLast
            var rev = ((IList<Base>)Children).Reverse(); // IList.Reverse creates new list, List.Reverse works in place.. go figure
            foreach (Base child in rev)
            {
                Base found = child.GetControlAt(x - child.X, y - child.Y);
                if (found != null)
                    return found;
            }

            if (!MouseInputEnabled)
                return null;

            return this;
        }

        protected virtual void Layout(Skin.Base skin)
        {
            if (skin.Renderer.CTT != null && ShouldCacheToTexture)
                skin.Renderer.CTT.CreateControlCacheTexture(this);
        }

        protected virtual void RecurseLayout(Skin.Base skin)
        {
            if (m_Skin != null)
                skin = m_Skin;
            if (IsHidden)
                return;

            if (NeedsLayout)
            {
                m_NeedsLayout = false;
                Layout(skin);
            }

            Rectangle bounds = RenderBounds;

            // Adjust bounds for padding
            bounds.X += m_Padding.Left;
            bounds.Width -= m_Padding.Left + m_Padding.Right;
            bounds.Y += m_Padding.Top;
            bounds.Height -= m_Padding.Top + m_Padding.Bottom;

            foreach (Base child in Children)
            {
                if (child.IsHidden)
                    continue;

                Pos dock = child.Dock;

                if (dock.HasFlag(Pos.Fill))
                    continue;

                if (dock.HasFlag(Pos.Top))
                {
                    Margin margin = child.Margin;

                    child.SetBounds(bounds.X + margin.Left, bounds.Y + margin.Top,
                                    bounds.Width - margin.Left - margin.Right, child.Height);

                    int height = margin.Top + margin.Bottom + child.Height;
                    bounds.Y += height;
                    bounds.Height -= height;
                }

                if (dock.HasFlag(Pos.Left))
                {
                    Margin margin = child.Margin;

                    child.SetBounds(bounds.X + margin.Left, bounds.Y + margin.Top, child.Width,
                                      bounds.Height - margin.Top - margin.Bottom);

                    int width = margin.Left + margin.Right + child.Width;
                    bounds.X += width;
                    bounds.Width -= width;
                }

                if (dock.HasFlag(Pos.Right))
                {
                    // TODO: THIS MARGIN CODE MIGHT NOT BE FULLY FUNCTIONAL
                    Margin margin = child.Margin;

                    child.SetBounds((bounds.X + bounds.Width) - child.Width - margin.Right, bounds.Y + margin.Top,
                                      child.Width, bounds.Height - margin.Top - margin.Bottom);

                    int width = margin.Left + margin.Right + child.Width;
                    bounds.Width -= width;
                }

                if (dock.HasFlag(Pos.Bottom))
                {
                    // TODO: THIS MARGIN CODE MIGHT NOT BE FULLY FUNCTIONAL
                    Margin margin = child.Margin;

                    child.SetBounds(bounds.X + margin.Left,
                                      (bounds.Y + bounds.Height) - child.Height - margin.Bottom,
                                      bounds.Width - margin.Left - margin.Right, child.Height);
                    bounds.Height -= child.Height + margin.Bottom + margin.Top;
                }

                child.RecurseLayout(skin);
            }

            m_InnerBounds = bounds;

            //
            // Fill uses the left over space, so do that now.
            //
            foreach (Base child in Children)
            {
                Pos dock = child.Dock;

                if (!(dock.HasFlag(Pos.Fill)))
                    continue;

                Margin margin = child.Margin;

                child.SetBounds(bounds.X + margin.Left, bounds.Y + margin.Top,
                                  bounds.Width - margin.Left - margin.Right, bounds.Height - margin.Top - margin.Bottom);
                child.RecurseLayout(skin);
            }

            PostLayout(skin);

            if (IsTabable)
            {
                if (GetCanvas().FirstTab == null)
                    GetCanvas().FirstTab = this;
                if (GetCanvas().NextTab == null)
                    GetCanvas().NextTab = this;
            }

            if (Global.KeyboardFocus == this)
            {
                GetCanvas().NextTab = null;
            }
        }

        public virtual bool IsChild(Base child)
        {
            return Children.Contains(child);
        }

        public virtual Point LocalPosToCanvas(Point pnt)
        {
            if (m_Parent != null)
            {
                int x = pnt.X + X;
                int y = pnt.Y + Y;

                // If our parent has an innerpanel and we're a child of it
                // add its offset onto us.
                //
                if (m_Parent.m_InnerPanel != null && m_Parent.m_InnerPanel.IsChild(this))
                {
                    x += m_Parent.m_InnerPanel.X;
                    y += m_Parent.m_InnerPanel.Y;
                }

                return m_Parent.LocalPosToCanvas(new Point(x, y));
            }

            return pnt;
        }

        public virtual Point CanvasPosToLocal(Point pnt)
        {
            if (m_Parent != null)
            {
                int x = pnt.X - X;
                int y = pnt.Y - Y;

                // If our parent has an innerpanel and we're a child of it
                // add its offset onto us.
                //
                if (m_Parent.m_InnerPanel != null && m_Parent.m_InnerPanel.IsChild(this))
                {
                    x -= m_Parent.m_InnerPanel.X;
                    y -= m_Parent.m_InnerPanel.Y;
                }


                return m_Parent.CanvasPosToLocal(new Point(x, y));
            }

            return pnt;
        }

        public virtual void CloseMenus()
        {
            foreach (Base child in Children)
            {
                child.CloseMenus();
            }
        }

        protected virtual void UpdateRenderBounds()
        {
            m_RenderBounds.X = 0;
            m_RenderBounds.Y = 0;

            m_RenderBounds.Width = m_Bounds.Width;
            m_RenderBounds.Height = m_Bounds.Height;
        }

        public virtual void UpdateCursor()
        {
            Platform.Windows.SetCursor(m_Cursor);
        }

        // giver
        public virtual Package DragAndDrop_GetPackage(int x, int y)
        {
            return m_DragAndDrop_Package;
        }

        // giver
        public virtual bool DragAndDrop_Draggable()
        {
            if (m_DragAndDrop_Package == null)
                return false;

            return m_DragAndDrop_Package.IsDraggable;
        }

        // giver
        public virtual void DragAndDrop_SetPackage(bool draggable, String name = "", object userData = null)
        {
            if (m_DragAndDrop_Package == null)
            {
                m_DragAndDrop_Package = new Package();
                m_DragAndDrop_Package.IsDraggable = draggable;
                m_DragAndDrop_Package.Name = name;
                m_DragAndDrop_Package.UserData = userData;
            }
        }

        // giver
        public virtual bool DragAndDrop_ShouldStartDrag()
        {
            return true;
        }

        // giver
        public virtual void DragAndDrop_StartDragging(Package package, int x, int y)
        {
            package.HoldOffset = CanvasPosToLocal(new Point(x, y));
            package.DrawControl = this;
        }

        // giver
        public virtual void DragAndDrop_EndDragging(bool success, int x, int y)
        {
        }

        // receiver
        public virtual bool DragAndDrop_HandleDrop(Package p, int x, int y)
        {
            DragAndDrop.SourceControl.Parent = this;
            return true;
        }

        // receiver
        public virtual void DragAndDrop_HoverEnter(Package p, int x, int y)
        {

        }

        // receiver
        public virtual void DragAndDrop_HoverLeave(Package p)
        {

        }

        // receiver
        public virtual void DragAndDrop_Hover(Package p, int x, int y)
        {

        }

        // receiver
        public virtual bool DragAndDrop_CanAcceptPackage(Package p)
        {
            return false;
        }

        public virtual bool SizeToChildren(bool w = true, bool h = true)
        {
            Point size = ChildrenSize();
            size.X += Padding.Right;
            size.Y += Padding.Bottom;
            return SetSize(w ? size.X : Width, h ? size.Y : Height);
        }

        public virtual Point ChildrenSize()
        {
            Point size = Point.Empty;

            foreach (Base child in Children)
            {
                if (child.IsHidden)
                    continue;

                size.X = Math.Max(size.X, child.Right);
                size.Y = Math.Max(size.Y, child.Bottom);
            }

            return size;
        }

        internal virtual bool HandleAccelerator(String accelerator)
        {
            if (Global.KeyboardFocus == this || !AccelOnlyFocus)
            {
                if (m_Accelerators.ContainsKey(accelerator))
                {
                    m_Accelerators[accelerator].Invoke(this);
                    return true;
                }
            }

            return Children.Any(child => child.HandleAccelerator(accelerator));
        }

        public void AddAccelerator(String accelerator, ControlCallback handler)
        {
            m_Accelerators[accelerator] = handler;
        }

        public void AddAccelerator(String accelerator)
        {
            m_Accelerators[accelerator] = DefaultAccel;
        }

        protected virtual void PostLayout(Skin.Base skin)
        {

        }

        public virtual void Redraw()
        {
            UpdateColors();
            m_CacheTextureDirty = true;
            if (m_Parent != null)
                m_Parent.Redraw();
        }

        public virtual void UpdateColors()
        {
            
        }

        public void InvalidateParent()
        {
            if (m_Parent != null)
            {
                m_Parent.Invalidate();
            }
        }

        internal virtual bool onKeyPress(Key key, bool pressed = true)
        {
            bool handled = false;
            switch (key)
            {
                case Key.Tab: handled = onKeyTab(pressed); break;
                case Key.Space: handled = onKeySpace(pressed); break;
                case Key.Home: handled = onKeyHome(pressed); break;
                case Key.End: handled = onKeyEnd(pressed); break;
                case Key.Return: handled = onKeyReturn(pressed); break;
                case Key.Backspace: handled = onKeyBackspace(pressed); break;
                case Key.Delete: handled = onKeyDelete(pressed); break;
                case Key.Right: handled = onKeyRight(pressed); break;
                case Key.Left: handled = onKeyLeft(pressed); break;
                case Key.Up: handled = onKeyUp(pressed); break;
                case Key.Down: handled = onKeyDown(pressed); break;
                case Key.Escape: handled = onKeyEscape(pressed); break;
                default: break;
            }

            if (!handled && Parent != null)
                Parent.onKeyPress(key, pressed);

            return handled;
        }

        internal virtual bool onKeyRelease(Key key)
        {
            return onKeyPress(key, false);
        }

        internal virtual bool onKeyTab(bool pressed)
        {
            if (!pressed)
                return true;

            if (GetCanvas().NextTab != null)
            {
                GetCanvas().NextTab.Focus();
                Redraw();
            }

            return true;
        }

        internal virtual bool onKeySpace(bool down) { return false; }
        internal virtual bool onKeyReturn(bool down) { return false; }
        internal virtual bool onKeyBackspace(bool down) { return false; }
        internal virtual bool onKeyDelete(bool down) { return false; }
        internal virtual bool onKeyRight(bool down) { return false; }
        internal virtual bool onKeyLeft(bool down) { return false; }
        internal virtual bool onKeyHome(bool down) { return false; }
        internal virtual bool onKeyEnd(bool down) { return false; }
        internal virtual bool onKeyUp(bool down) { return false; }
        internal virtual bool onKeyDown(bool down) { return false; }
        internal virtual bool onKeyEscape(bool down) { return false; }

        internal virtual void onPaste(Base from)
        {
        }

        internal virtual void onCopy(Base from)
        {
        }

        internal virtual void onCut(Base from)
        {
        }

        internal virtual void onSelectAll(Base from)
        {
        }

        protected virtual void RenderFocus(Skin.Base skin)
        {
            if (Global.KeyboardFocus != this)
                return;
            if (!IsTabable)
                return;

            skin.DrawKeyboardHighlight(this, RenderBounds, 3);
        }

        protected virtual void RenderUnder(Skin.Base skin)
        {

        }

        protected virtual void RenderOver(Skin.Base skin)
        {

        }

        public virtual void Think()
        {

        }

        internal virtual void onKeyboardFocus()
        {

        }

        internal virtual void onLostKeyboardFocus()
        {

        }

        internal virtual bool onChar(Char chr)
        {
            return false;
        }

        public virtual void Anim_WidthIn(float length, float delay = 0.0f, float ease = 1.0f)
        {
            Animation.Add(this, new Anim.Size.Width(0, Width, length, false, delay, ease));
            Width = 0;
        }

        public virtual void Anim_HeightIn(float length, float delay, float ease)
        {
            Animation.Add(this, new Anim.Size.Height(0, Height, length, false, delay, ease));
            Height = 0;
        }

        public virtual void Anim_WidthOut(float length, bool hide, float delay, float ease)
        {
            Animation.Add(this, new Anim.Size.Width(Width, 0, length, hide, delay, ease));
        }

        public virtual void Anim_HeightOut(float length, bool hide, float delay, float ease)
        {
            Animation.Add(this, new Anim.Size.Height(Height, 0, length, hide, delay, ease));
        }
    }
}
