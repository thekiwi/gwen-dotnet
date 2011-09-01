using System;

namespace Gwen.Controls.Layout
{
    public class Position : Base
    {
        protected Pos m_Pos;

        public Pos Pos { get { return m_Pos; } set { m_Pos = value; } }

        public Position(Base parent) : base(parent)
        {
            Pos = Pos.Left | Pos.Top;
        }

        protected override void PostLayout(Skin.Base skin)
        {
            foreach (Base child in Children)
            {
                child.Position(m_Pos);
            }
        }
    }

    public class Center : Position
    {
        public Center(Base parent) : base(parent)
        {
            Pos = Pos.Center;
        }
    }
}
