using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Gwen.ControlsInternal;

namespace Gwen.Controls
{
    public class Label : Base
    {
        protected Text m_Text;
        protected Pos m_iAlign;
        protected Padding m_rTextPadding;

        public virtual Pos Alignment { get { return m_iAlign; } set { m_iAlign = value; } }
        public virtual String Text { get { return m_Text.String; } }
        public virtual Font Font { get { return m_Text.Font; } set { m_Text.Font = value; } }
        public virtual Color TextColor { get { return m_Text.TextColor; } set { m_Text.TextColor = value; } }

        public virtual int TextWidth { get { return m_Text.Width; } }
        public virtual int TextHeight { get { return m_Text.Height; } }
        public virtual int TextX { get { return m_Text.X; } }
        public virtual int TextY { get { return m_Text.Y; } }
        public virtual int TextLength { get { return m_Text.Length; } }
        public virtual int TextRight { get { return m_Text.Right; } }

        public virtual bool AutoSizeToContents { get { return m_Text.AutoSizeToContents; } set { m_Text.AutoSizeToContents = value; } }

        public virtual Padding TextPadding { get { return m_rTextPadding; } set { m_rTextPadding = value; Invalidate(); InvalidateParent(); } }

        public Label(Base parent) : base(parent)
        {
            m_Text = new Text(this);
            m_Text.Font = Skin.DefaultFont;

            MouseInputEnabled = false;
            SetBounds(0, 0, 100, 10);
            Alignment = Pos.Left | Pos.Top;
        }

        protected virtual void OnTextChanged()
        {}

        protected override void Layout(Skin.Base skin)
        {
            Pos iAlign = m_iAlign;

            int x = m_rTextPadding.Left + m_Padding.Left;
            int y = m_rTextPadding.Top + m_Padding.Top;

            if (iAlign.HasFlag(Pos.Right)) 
                x = Width - m_Text.Width - m_rTextPadding.Right - m_Padding.Right;
            if (iAlign.HasFlag(Pos.CenterH))
                x = Global.Trunc((m_rTextPadding.Left + m_Padding.Left) + ((Width - m_Text.Width)*0.5) - m_rTextPadding.Right - m_Padding.Right);

            if (iAlign.HasFlag(Pos.CenterV))
                y = Global.Trunc((m_rTextPadding.Top + m_Padding.Top) + ((Height - m_Text.Height) * 0.5) - m_rTextPadding.Bottom - m_Padding.Bottom);
            if (iAlign.HasFlag(Pos.Bottom)) 
                y = Height - m_Text.Height - m_rTextPadding.Bottom - m_Padding.Bottom;

            m_Text.SetPos(x, y);
        }

        public virtual void SetText(String str, bool doEvents = true)
        {
            if (Text == str)
                return;

            m_Text.String = str;
            if (AutoSizeToContents)
                SizeToContents();
            Redraw();

            if (doEvents)
                OnTextChanged();
        }

        public virtual void SizeToContents()
        {
            m_Text.SetPos(m_rTextPadding.Left + m_Padding.Left, m_rTextPadding.Top + m_Padding.Top);
            m_Text.RefreshSize();

            SetSize(m_Text.Width + m_Padding.Left + m_Padding.Right + m_rTextPadding.Left + m_rTextPadding.Right, 
                m_Text.Height + m_Padding.Top + m_Padding.Bottom + m_rTextPadding.Top + m_rTextPadding.Bottom);
        }

        public virtual Point GetCharacterPosition(int index)
        {
            Point p = m_Text.GetCharacterPosition(index);
            return new Point(p.X + m_Text.X, p.Y + m_Text.Y);
        }
    }
}
