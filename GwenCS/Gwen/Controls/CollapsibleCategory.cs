using System;
using Gwen.ControlsInternal;

namespace Gwen.Controls
{
    public class CollapsibleCategory : Base
    {
        protected Button m_Button;
        protected CollapsibleList m_List;

        public CollapsibleList List { get { return m_List; } set { m_List = value; } }
        public String Text { get { return m_Button.Text; } set { m_Button.Text = value; } }

        public event ControlCallback OnSelection;

        // todo: iterator, make this as function?
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

        public CollapsibleCategory(Base parent) : base(parent)
        {
            m_Button = new CategoryHeaderButton(this);
            m_Button.Text = "Category Title"; // [omeg] todo: i18n
            m_Button.Dock = Pos.Top;
            m_Button.Height = 20;

            Padding = new Padding(1, 0, 1, 5);
            SetSize(512, 512);
        }

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

        protected override void Render(Skin.Base skin)
        {
            skin.DrawCategoryInner(this, m_Button.ToggleState);
        }

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

        public override void Dispose()
        {
            m_Button.Dispose();
            base.Dispose();
        }
    }
}
