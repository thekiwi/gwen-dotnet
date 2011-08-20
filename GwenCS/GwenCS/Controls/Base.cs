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
        // [omeg] C# delegates/events instead of Gwen::Event
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

        protected bool m_bRestrictToParent;
        protected bool m_bDisabled;
        protected bool m_bHidden;
        protected bool m_bMouseInputEnabled;
        protected bool m_bKeyboardInputEnabled;
        protected bool m_bDrawBackground;

        protected Pos m_iDock;

        protected Cursor m_Cursor;

        protected bool m_Tabable;

        protected bool m_bNeedsLayout;
        protected bool m_bCacheTextureDirty;
        protected bool m_bCacheToTexture;

        protected Package m_DragAndDrop_Package;

        private object m_pUserData;

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
            get { return m_iDock; }
            set
            {
                if (m_iDock == value)
                    return;

                m_iDock = value;

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

        public bool IsOnTop { get { return this == Parent.Children.First(); } }
        public object UserData { get { return m_pUserData; } set { m_pUserData = value; } }
        public bool IsHovered { get { return Global.HoveredControl == this; } }
        public bool ShouldDrawHover { get { return Global.MouseFocus == this || Global.MouseFocus == null; } }
        public bool HasFocus { get { return Global.KeyboardFocus == this; } }
        public bool IsDisabled { get { return m_bDisabled; } set { m_bDisabled = value; } }
        public bool IsHidden { get { return m_bHidden; } set { if (value == m_bHidden) return; m_bHidden = value; Invalidate(); } }
        public bool RestrictToParent { get { return m_bRestrictToParent; } set { m_bRestrictToParent = value; } }
        public bool MouseInputEnabled { get { return m_bMouseInputEnabled; } set { m_bMouseInputEnabled = value; } }
        public bool KeyboardInputEnabled { get { return m_bKeyboardInputEnabled; } set { m_bKeyboardInputEnabled = value; } }
        public Cursor Cursor { get { return m_Cursor; } set { m_Cursor = value; } }
        public bool IsTabable { get { return m_Tabable; } set { m_Tabable = value; } }
        public bool ShouldDrawBackground { get { return m_bDrawBackground; } set { m_bDrawBackground = value; } }
        public bool ShouldCacheToTexture { get { return m_bCacheToTexture; } set { m_bCacheToTexture = value; /*Children.ForEach(x => x.ShouldCacheToTexture=value);*/ } }
        public String Name { get { return m_Name; } set { m_Name = value; } }
        public Rectangle Bounds { get { return m_Bounds; } }
        public bool NeedsLayout { get { return m_bNeedsLayout; } }
        public Rectangle RenderBounds { get { return m_RenderBounds; } }
        public Rectangle InnerBounds { get { return m_InnerBounds; } }
        public virtual bool AccelOnlyFocus { get { return false; } }
        public virtual bool NeedsInputChars { get { return false; } }
        public Point MinimumSize { get { return m_MinimumSize; } }
        public Point MaximumSize { get { return m_MaximumSize; } }

        private static readonly Point m_MinimumSize = new Point(1, 1);
        private static readonly Point m_MaximumSize = new Point(Global.MaxCoord, Global.MaxCoord);

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
        public int Bottom { get { return m_Bounds.Bottom + m_Margin.bottom; } }
        public int Right { get { return m_Bounds.Right + m_Margin.right; } }

        public Base(Base parent = null)
        {
            Children = new List<Base>();
            m_Accelerators = new Dictionary<string, ControlCallback>();

            Parent = parent;

            m_bHidden = false;
            m_Bounds = new Rectangle(0, 0, 10, 10);
            m_Padding = new Padding(0, 0, 0, 0);
            m_Margin = new Margin(0, 0, 0, 0);

            RestrictToParent = false;

            MouseInputEnabled = true;
            KeyboardInputEnabled = false;

            Invalidate();
            Cursor = Cursors.Default;
            ToolTip = null;
            IsTabable = false;
            ShouldDrawBackground = true;
            m_bDisabled = false;
            m_bCacheTextureDirty = true;
            m_bCacheToTexture = false;
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
            m_bNeedsLayout = true;
            m_bCacheTextureDirty = true;
        }

        public virtual void SendToBack()
        {
            if (m_Parent == null)
                return;
            if (m_Parent.Children.First() == this)
                return;

            m_Parent.Children.Remove(this);
            m_Parent.Children.Insert(0, this);

            InvalidateParent();
        }

        public virtual void BringToFront()
        {
            if (m_Parent == null)
                return;
            if (m_Parent.Children.Last() == this)
                return;

            m_Parent.Children.Remove(this);
            m_Parent.Children.Add(this);
            InvalidateParent();
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
            if (null == m_Parent)
                return;

            m_Parent.Children.Remove(this);

            // todo: validate
            int idx = m_Parent.Children.IndexOf(child);
            if (idx == m_Parent.Children.Count - 1)
            {
                BringToFront();
                return;
            }

            if (behind)
            {
                ++idx;

                if (idx == m_Parent.Children.Count - 1)
                {
                    BringToFront();
                    return;
                }
            }

            m_Parent.Children.Insert(idx, this);
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
            child.Dispose();
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
            if (m_bRestrictToParent && (Parent != null))
            {
                Base parent = Parent;
                if (x - Padding.Left < parent.Margin.left)
                    x = parent.Margin.left + Padding.Left;
                if (y - Padding.Top < parent.Margin.top)
                    y = parent.Margin.top + Padding.Top;
                if (x + Width + Padding.Right > parent.Width - parent.Margin.right)
                    x = parent.Width - parent.Margin.right - Width - Padding.Right;
                if (y + Height + Padding.Bottom > parent.Height - parent.Margin.bottom)
                    y = parent.Height - parent.Margin.bottom - Height - Padding.Bottom;
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
            return SetBounds((int) x, (int) y, (int) w, (int) h);
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

            Point pOldRenderOffset = render.RenderOffset;
            Rectangle rOldRegion = render.ClipRegion;

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

            if (m_bCacheTextureDirty && render.ClipRegionVisible)
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
                    m_bCacheTextureDirty = false;
                }
            }

            render.ClipRegion = rOldRegion;
            render.StartClip();
            render.RenderOffset = pOldRenderOffset;

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

            if (render.CTT!=null && ShouldCacheToTexture)
            {
                DoCacheRender(skin, this);
                return;
            }

            Point pOldRenderOffset = render.RenderOffset;

            render.AddRenderOffset(Bounds);

            RenderUnder(skin);

            Rectangle rOldRegion = render.ClipRegion;
            render.AddClipRegion(Bounds);

            if (render.ClipRegionVisible)
            {
                render.StartClip();

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

                render.ClipRegion = rOldRegion;
                render.StartClip();

                RenderOver(skin);
            }
            else
            {
                render.ClipRegion = rOldRegion;
            }

            RenderFocus(skin);

            render.RenderOffset = pOldRenderOffset;
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

        internal virtual bool onMouseWheeled(int iDelta)
        {
            if (m_ActualParent != null)
                return m_ActualParent.onMouseWheeled(iDelta);

            return false;
        }

        internal virtual void onMouseMoved(int x, int y, int dx, int dy)
        {

        }

        internal virtual void onMouseClickLeft(int x, int y, bool pressed)
        {

        }

        internal virtual void onMouseClickRight(int x, int y, bool pressed)
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
        }

        internal virtual void onMouseLeave()
        {
            if (OnHoverLeave != null)
                OnHoverLeave.Invoke(this);

            if (ToolTip != null)
                global::Gwen.ToolTip.Disable(this);
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
                m_bNeedsLayout = false;
                Layout(skin);
            }

            Rectangle rBounds = RenderBounds;

            // Adjust bounds for padding
            rBounds.X += m_Padding.Left;
            rBounds.Width -= m_Padding.Left + m_Padding.Right;
            rBounds.Y += m_Padding.Top;
            rBounds.Height -= m_Padding.Top + m_Padding.Bottom;

            foreach (Base child in Children)
            {
                if (child.IsHidden)
                    continue;

                Pos iDock = child.Dock;

                if (iDock.HasFlag(Pos.Fill))
                    continue;

                if (iDock.HasFlag(Pos.Top))
                {
                    Margin margin = child.Margin;

                    child.SetBounds(rBounds.X + margin.left, rBounds.Y + margin.top,
                                    rBounds.Width - margin.left - margin.right, child.Height);

                    int iHeight = margin.top + margin.bottom + child.Height;
                    rBounds.Y += iHeight;
                    rBounds.Height -= iHeight;
                }

                if (iDock.HasFlag(Pos.Left))
                {
                    Margin margin = child.Margin;

                    child.SetBounds(rBounds.X + margin.left, rBounds.Y + margin.top, child.Width,
                                      rBounds.Height - margin.top - margin.bottom);

                    int iWidth = margin.left + margin.right + child.Width;
                    rBounds.X += iWidth;
                    rBounds.Width -= iWidth;
                }

                if (iDock.HasFlag(Pos.Right))
                {
                    // TODO: THIS MARGIN CODE MIGHT NOT BE FULLY FUNCTIONAL
                    Margin margin = child.Margin;

                    child.SetBounds((rBounds.X + rBounds.Width) - child.Width - margin.right, rBounds.Y + margin.top,
                                      child.Width, rBounds.Height - margin.top - margin.bottom);

                    int iWidth = margin.left + margin.right + child.Width;
                    rBounds.Width -= iWidth;
                }

                if (iDock.HasFlag(Pos.Bottom))
                {
                    // TODO: THIS MARGIN CODE MIGHT NOT BE FULLY FUNCTIONAL
                    Margin margin = child.Margin;

                    child.SetBounds(rBounds.X + margin.left,
                                      (rBounds.Y + rBounds.Height) - child.Height - margin.bottom,
                                      rBounds.Width - margin.left - margin.right, child.Height);
                    rBounds.Height -= child.Height + margin.bottom + margin.top;
                }

                child.RecurseLayout(skin);
            }

            m_InnerBounds = rBounds;

            //
            // Fill uses the left over space, so do that now.
            //
            foreach (Base child in Children)
            {
                Pos iDock = child.Dock;

                if (!(iDock.HasFlag(Pos.Fill)))
                    continue;

                Margin margin = child.Margin;

                child.SetBounds(rBounds.X + margin.left, rBounds.Y + margin.top,
                                  rBounds.Width - margin.left - margin.right, rBounds.Height - margin.top - margin.bottom);
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
            m_bCacheTextureDirty = true; 
            if (m_Parent != null) 
                m_Parent.Redraw();
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
                case Key.Tab:       handled = onKeyTab(pressed); break;
                case Key.Space:     handled = onKeySpace(pressed); break;
                case Key.Home:      handled = onKeyHome(pressed); break;
                case Key.End:       handled = onKeyEnd(pressed); break;
                case Key.Return:    handled = onKeyReturn(pressed); break;
                case Key.Backspace: handled = onKeyBackspace(pressed); break;
                case Key.Delete:    handled = onKeyDelete(pressed); break;
                case Key.Right:     handled = onKeyRight(pressed); break;
                case Key.Left:      handled = onKeyLeft(pressed); break;
                case Key.Up:        handled = onKeyUp(pressed); break;
                case Key.Down:      handled = onKeyDown(pressed); break;
                case Key.Escape:    handled = onKeyEscape(pressed); break;
                default: break;
            }

            if (!handled && Parent!=null)
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

        internal virtual bool onKeySpace(bool bDown) { return false; }
        internal virtual bool onKeyReturn(bool bDown) { return false; }
        internal virtual bool onKeyBackspace(bool bDown) { return false; }
        internal virtual bool onKeyDelete(bool bDown) { return false; }
        internal virtual bool onKeyRight(bool bDown) { return false; }
        internal virtual bool onKeyLeft(bool bDown) { return false; }
        internal virtual bool onKeyHome(bool bDown) { return false; }
        internal virtual bool onKeyEnd(bool bDown) { return false; }
        internal virtual bool onKeyUp(bool bDown) { return false; }
        internal virtual bool onKeyDown(bool bDown) { return false; }
        internal virtual bool onKeyEscape(bool bDown) { return false; }

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

        public virtual void Anim_WidthIn(float fLength, float fDelay = 0.0f, float fEase = 1.0f)
        {
            Animation.Add(this, new Anim.Size.Width(0, Width, fLength, false, fDelay, fEase));
            Width = 0;
        }

        public virtual void Anim_HeightIn( float fLength, float fDelay, float fEase )
        {
            Animation.Add(this, new Anim.Size.Height(0, Height, fLength, false, fDelay, fEase));
            Height = 0;
        }

        public virtual void Anim_WidthOut( float fLength, bool bHide, float fDelay, float fEase )
        {
            Animation.Add(this, new Anim.Size.Width(Width, 0, fLength, bHide, fDelay, fEase));
        }

        public virtual void Anim_HeightOut( float fLength, bool bHide, float fDelay, float fEase )
        {
            Animation.Add(this, new Anim.Size.Height(Height, 0, fLength, bHide, fDelay, fEase));
        }

    }
}
