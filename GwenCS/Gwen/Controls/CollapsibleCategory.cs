using System;
using Gwen.ControlsInternal;

namespace Gwen.Controls
{
    /// <summary>
    /// CollapsibleCategory control. Used in CollapsibleList.
    /// </summary>
    public class CollapsibleCategory : Base
    {
        protected readonly Button m_Button;
        protected readonly CollapsibleList m_List;

        /// <summary>
        /// Header text.
        /// </summary>
        public String Text { get { return m_Button.Text; } set { m_Button.Text = value; } }

        /// <summary>
        /// Invoked when an entry is selected.
        /// </summary>
        public event ControlCallback OnSelection;

        // todo: iterator, make this as function?
        /// <summary>
        /// Selected entry.
        /// </summary>
        public Button Selected
        {
            get
            {
                foreach (Base child in Children)
                {
                    CategoryButton button = child as CategoryButton;
                    if (button == null)
                        continue;

                    if (button.ToggleState)
                        return button;
                }

                return null;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollapsibleCategory"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public CollapsibleCategory(CollapsibleList parent) : base(parent)
        {
            m_Button = new CategoryHeaderButton(this);
            m_Button.Text = "Category Title"; // [omeg] todo: i18n
            m_Button.Dock = Pos.Top;
            m_Button.Height = 20;

            m_List = parent;

            Padding = new Padding(1, 0, 1, 5);
            SetSize(512, 512);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            m_Button.Dispose();
            base.Dispose();
        }

        /// <summary>
        /// Handler for OnSelection event.
        /// </summary>
        /// <param name="control">Event source.</param>
        protected virtual void onSelection(Base control)
        {
            CategoryButton child = control as CategoryButton;
            if (child == null) return;

            if (m_List != null)
            {
                m_List.UnselectAll();
            }
            else
            {
                UnselectAll();
            }

            child.ToggleState = true;

            if (OnSelection != null)
                OnSelection.Invoke(this);
        }

        /// <summary>
        /// Adds a new entry.
        /// </summary>
        /// <param name="name">Entry name (displayed).</param>
        /// <returns>Newly created control.</returns>
        public Button Add(String name)
        {
            CategoryButton button = new CategoryButton(this);
            button.Text = name;
            button.Dock = Pos.Top;
            button.SizeToContents();
            button.SetSize(button.Width + 4, button.Height + 4);
            button.Padding = new Padding(5, 2, 2, 2);
            button.OnPress += onSelection;

            return button;
        }

        /// <summary>
        /// Renders the control using specified skin.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Render(Skin.Base skin)
        {
            skin.DrawCategoryInner(this, m_Button.ToggleState);
        }

        /// <summary>
        /// Unselects all entries.
        /// </summary>
        public void UnselectAll()
        {
            foreach (Base child in Children)
            {
                CategoryButton button = child as CategoryButton;
                if (button == null)
                    continue;

                button.ToggleState = false;
            }
        }

        /// <summary>
        /// Function invoked after layout.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void PostLayout(Skin.Base skin)
        {
            if (m_Button.ToggleState)
            {
                Height = m_Button.Height;
            }
            else
            {
                SizeToChildren(false, true);
            }

            bool b = true;
            foreach (Base child in Children)
            {
                CategoryButton button = child as CategoryButton;
                if (button == null)
                    continue;

                button.m_Alt = b;
                button.UpdateColors();
                b = !b;
            }
        }
    }
}
