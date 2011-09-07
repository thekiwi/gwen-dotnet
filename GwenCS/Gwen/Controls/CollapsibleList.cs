using System;

namespace Gwen.Controls
{
    public class CollapsibleList : ScrollControl
    {
        public event ControlCallback OnSelection;

        public CollapsibleList(Base parent) : base(parent)
        {
            SetScroll(false, true);
            AutoHideBars = true;
        }

        // todo: iterator, make this as function? check if works
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

        public virtual void Add(CollapsibleCategory category)
        {
            category.Parent = this;
            category.Dock = Pos.Top;
            category.Margin = new Margin(1, 0, 1, 1);
            category.List = this;
            category.OnSelection += onSelectionEvent;
        }

        public virtual CollapsibleCategory Add(String categoryName)
        {
            CollapsibleCategory cat = new CollapsibleCategory(this);
            cat.Text = categoryName;
            Add(cat);
            return cat;
        }

        protected override void Render(Skin.Base skin)
        {
            Skin.DrawCategoryHolder(this);
        }

        public virtual void UnselectAll()
        {
            foreach (Base child in InnerChildren) // we are ScrollControl, content is in inner panel
            {
                CollapsibleCategory cat = child as CollapsibleCategory;
                if (cat == null)
                    continue;

                cat.UnselectAll();
            }
        }

        protected virtual void onSelection(CollapsibleCategory control, Button selected)
        {
            if (OnSelection != null)
                OnSelection.Invoke(this);
        }

        protected virtual void onSelectionEvent(Base control)
        {
            CollapsibleCategory cat = control as CollapsibleCategory;
            if (cat == null) return;

            onSelection(cat, cat.Selected);
        }

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
    }
}
