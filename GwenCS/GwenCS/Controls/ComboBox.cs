using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gwen.Controls
{
    public class ComboBoxButton : Button
    {
        public ComboBoxButton(Base parent) : base(parent)
        {}

        protected override void Render(Skin.Base skin)
        {
            skin.DrawComboBoxButton(this, m_bDepressed);
        }
    }

    public class ComboBox : Base
    {
        protected Menu m_Menu;
        protected MenuItem m_MenuItem;
        protected ComboBoxButton m_OpenButton;
        protected Label m_SelectedText;
    }
}
