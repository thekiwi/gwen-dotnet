using System;
using System.Drawing;
using Gwen.ControlsInternal;

namespace Gwen.Controls
{
    public class HSVColorPicker : Base, IColorPicker
    {
        protected ColorLerpBox m_LerpBox;
        protected ColorSlider m_ColorSlider;
        protected ColorDisplay m_Before;
        protected ColorDisplay m_After;

        public event ControlCallback OnColorChanged;

        public Color DefaultColor { get { return m_Before.Color; } }
        public Color Color { get { return m_LerpBox.SelectedColor; } }

        public HSVColorPicker(Base parent)
            : base(parent)
        {
            MouseInputEnabled = true;
            SetSize(256, 128);
            //ShouldCacheToTexture = true;

            m_LerpBox = new ColorLerpBox(this);
            m_LerpBox.OnSelectionChanged += ColorBoxChanged;
            m_LerpBox.Dock = Pos.Left;

            m_ColorSlider = new ColorSlider(this);
            m_ColorSlider.SetPos(m_LerpBox.Width + 15, 5);
            m_ColorSlider.OnSelectionChanged += ColorSliderChanged;
            m_ColorSlider.Dock = Pos.Left;

            m_After = new ColorDisplay(this);
            m_After.SetSize(48, 24);
            m_After.SetPos(m_ColorSlider.X + m_ColorSlider.Width + 15, 5);

            m_Before = new ColorDisplay(this);
            m_Before.SetSize(48, 24);
            m_Before.SetPos(m_After.X, 28);

            int x = m_Before.X;
            int y = m_Before.Y + 30;

            {
                Label label = new Label(this);
                label.SetText("R:");
                label.SizeToContents();
                label.SetPos(x, y);

                TextBoxNumeric numeric = new TextBoxNumeric(this);
                numeric.Name = "RedBox";
                numeric.SetPos(x + 15, y - 1);
                numeric.SetSize(26, 16);
                numeric.SelectAllOnFocus = true;
                numeric.OnTextChanged += NumericTyped;
            }

            y += 20;

            {
                Label label = new Label(this);
                label.SetText("G:");
                label.SizeToContents();
                label.SetPos(x, y);

                TextBoxNumeric numeric = new TextBoxNumeric(this);
                numeric.Name = "GreenBox";
                numeric.SetPos(x + 15, y - 1);
                numeric.SetSize(26, 16);
                numeric.SelectAllOnFocus = true;
                numeric.OnTextChanged += NumericTyped;
            }

            y += 20;

            {
                Label label = new Label(this);
                label.SetText("B:");
                label.SizeToContents();
                label.SetPos(x, y);

                TextBoxNumeric numeric = new TextBoxNumeric(this);
                numeric.Name = "BlueBox";
                numeric.SetPos(x + 15, y - 1);
                numeric.SetSize(26, 16);
                numeric.SelectAllOnFocus = true;
                numeric.OnTextChanged += NumericTyped;
            }

            SetColor(DefaultColor);
        }

        protected void NumericTyped(Base control)
        {
            TextBoxNumeric box = control as TextBoxNumeric;
            if (null == box) return;

            if (box.Text == String.Empty) return;

            int textValue = (int)box.Value;
            if (textValue < 0) textValue = 0;
            if (textValue > 255) textValue = 255;

            Color newColor = Color;

            if (box.Name.Contains("Red"))
            {
                newColor = Color.FromArgb(Color.A, textValue, Color.G, Color.B);
            }
            else if (box.Name.Contains("Green"))
            {
                newColor = Color.FromArgb(Color.A, Color.R, textValue, Color.B);
            }
            else if (box.Name.Contains("Blue"))
            {
                newColor = Color.FromArgb(Color.A, Color.R, Color.G, textValue);
            }
            else if (box.Name.Contains("Alpha"))
            {
                newColor = Color.FromArgb(textValue, Color.R, Color.G, Color.B);
            }

            SetColor(newColor);
        }

        protected void UpdateControls(Color color)
        {
            // What in the FUCK

            TextBoxNumeric redBox = FindChildByName("RedBox", false) as TextBoxNumeric;
            if (redBox != null)
                redBox.SetText(color.R.ToString(), false);

            TextBoxNumeric greenBox = FindChildByName("GreenBox", false) as TextBoxNumeric;
            if (greenBox != null)
                greenBox.SetText(color.G.ToString(), false);

            TextBoxNumeric blueBox = FindChildByName("BlueBox", false) as TextBoxNumeric;
            if (blueBox != null)
                blueBox.SetText(color.B.ToString(), false);

            m_After.Color = color;

            if (OnColorChanged != null)
                OnColorChanged.Invoke(this);
        }

        public void SetColor(Color color, bool onlyHue = false, bool reset = false)
        {
            UpdateControls(color);

            if (reset)
                m_Before.Color = color;

            m_ColorSlider.SetColor(color);
            m_LerpBox.SetColor(color, onlyHue);
            m_After.Color = color;
        }

        protected void ColorBoxChanged(Base control)
        {
            UpdateControls(Color);
            Invalidate();
        }

        protected void ColorSliderChanged(Base control)
        {
            if (m_LerpBox != null)
                m_LerpBox.SetColor(m_ColorSlider.SelectedColor, true);
            Invalidate();
        }
    }
}
