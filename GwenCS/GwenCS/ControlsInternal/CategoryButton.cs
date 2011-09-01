using System;
using Gwen.Controls;

namespace Gwen.ControlsInternal
{
    public class CategoryButton : Button
    {
        internal bool m_bAlt;

        internal CategoryButton(Base parent) : base(parent)
        {
            Alignment = Pos.Left | Pos.CenterV;
            m_bAlt = false;
        }

        protected override void Render(Skin.Base skin)
        {
            if (m_bAlt)
            {
                if (IsDepressed | ToggleState)
                    Skin.Renderer.DrawColor = skin.Colors.Category.LineAlt.Button_Selected;
                else if (IsHovered)
                    Skin.Renderer.DrawColor = skin.Colors.Category.LineAlt.Button_Hover;
                else
                    Skin.Renderer.DrawColor = skin.Colors.Category.LineAlt.Button;
            }
            else
            {
                if (IsDepressed | ToggleState)
                    Skin.Renderer.DrawColor = skin.Colors.Category.Line.Button_Selected;
                else if (IsHovered)
                    Skin.Renderer.DrawColor = skin.Colors.Category.Line.Button_Hover;
                else
                    Skin.Renderer.DrawColor = skin.Colors.Category.Line.Button;
            }

            skin.Renderer.DrawFilledRect(RenderBounds);
        }

        public override void UpdateColors()
        {
            if (m_bAlt)
            {
                if (IsDepressed || ToggleState)
                {
                    TextColor = Skin.Colors.Category.LineAlt.Text_Selected;
                    return;
                }
                if (IsHovered)
                {
                    TextColor = Skin.Colors.Category.LineAlt.Text_Hover;
                    return;
                }
                TextColor = Skin.Colors.Category.LineAlt.Text;
            }
            if (IsDepressed || ToggleState)
            {
                TextColor = Skin.Colors.Category.Line.Text_Selected;
                return;
            }
            if (IsHovered)
            {
                TextColor = Skin.Colors.Category.Line.Text_Hover;
                return;
            }
            TextColor = Skin.Colors.Category.Line.Text;
        }
    }
}
