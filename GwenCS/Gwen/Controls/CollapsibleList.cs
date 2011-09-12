using System;

namespace Gwen.Controls
{
    /// <summary>
    /// CollapsibleList control. Groups CollapsibleCategory controls.
    /// </summary>
    public class CollapsibleList : ScrollControl
    {
        /// <summary>
        /// Invoked when an entry is selected.
        /// </summary>
        public event ControlCallback OnSelection;

        /// <summary>
        /// Invoked when the category collapsed state changes (header button is pressed).
        /// </summary>
        public event ControlCallback OnCollapsed;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollapsibleList"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public CollapsibleList(Base parent) : base(parent)
        {
            EnableScroll(false, true);
            AutoHideBars = true;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        public override void Dispose()
        {
            foreach (Base child in InnerChildren)
            {
                CollapsibleCategory cat = child as CollapsibleCategory;
                if (cat == null)
                    continue;

                cat.Dispose();
            }
            base.Dispose();
        }

        // todo: iterator, make this as function? check if works
        /// <summary>
        /// Selected entry.
        /// </summary>
        public Button Selected
        {
            get
            {
                foreach (Base child in InnerChildren)
                {
                    CollapsibleCategory cat = child as CollapsibleCategory;
                    if (cat == null)
                        continue;

                    Button button = cat.Selected;

                    if (button != null)
                        return button;
                }

                return null;
            }
        }

        /// <summary>
        /// Adds a category to the list.
        /// </summary>
        /// <param name="category">Category control to add.</param>
        protected virtual void Add(CollapsibleCategory category)
        {
            category.Parent = this;
            category.Dock = Pos.Top;
            category.Margin = new Margin(1, 0, 1, 1);
            category.OnSelection += onCategorySelect;
            category.OnCollapsed += onCategoryCollapse;
            // this relies on fact that category.m_List is set to its parent
        }

        /// <summary>
        /// Adds a new category to the list.
        /// </summary>
        /// <param name="categoryName">Name of the category.</param>
        /// <returns>Newly created control.</returns>
        public virtual CollapsibleCategory Add(String categoryName)
        {
            CollapsibleCategory cat = new CollapsibleCategory(this);
            cat.Text = categoryName;
            Add(cat);
            return cat;
        }

        /// <summary>
        /// Renders the control using specified skin.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Render(Skin.Base skin)
        {
            Skin.DrawCategoryHolder(this);
        }

        /// <summary>
        /// Unselects all entries.
        /// </summary>
        public virtual void UnselectAll()
        {
            foreach (Base child in InnerChildren)
            {
                CollapsibleCategory cat = child as CollapsibleCategory;
                if (cat == null)
                    continue;

                cat.UnselectAll();
            }
        }

        /// <summary>
        /// Handler for OnSelection event.
        /// </summary>
        /// <param name="control">Event source: <see cref="CollapsibleList"/>.</param>
        protected virtual void onCategorySelect(Base control)
        {
            CollapsibleCategory cat = control as CollapsibleCategory;
            if (cat == null) return;

            if (OnSelection != null)
                OnSelection.Invoke(this);
        }

        /// <summary>
        /// Handler for category collapsed event.
        /// </summary>
        /// <param name="control">Event source: <see cref="CollapsibleCategory"/>.</param>
        protected virtual void onCategoryCollapse(Base control)
        {
            CollapsibleCategory cat = control as CollapsibleCategory;
            if (cat == null) return;

            if (OnCollapsed != null)
                OnCollapsed.Invoke(control);
        }
    }
}
