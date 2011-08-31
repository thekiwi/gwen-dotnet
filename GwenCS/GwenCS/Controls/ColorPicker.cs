using System;
using System.Drawing;
using Gwen.ControlsInternal;

namespace Gwen.Controls
{
    public class ColorPicker : Base, IColorPicker
    {
        protected Color m_Color;

        public Color Color { get { return m_Color; } set { m_Color = value; UpdateControls(); } }
        public int R { get { return m_Color.R; } set { m_Color = Color.FromArgb(m_Color.A, value, m_Color.G, m_Color.B); } }
        public int G { get { return m_Color.G; } set { m_Color = Color.FromArgb(m_Color.A, m_Color.R, value, m_Color.B); } }
        public int B { get { return m_Color.B; } set { m_Color = Color.FromArgb(m_Color.A, m_Color.R, m_Color.G, value); } }
        public int A { get { return m_Color.A; } set { m_Color = Color.FromArgb(value, m_Color.R, m_Color.G, m_Color.B); } }

        public event ControlCallback OnColorChanged;

        public ColorPicker(Base parent) : base(parent)
        {
            MouseInputEnabled = true;

            SetSize(256, 150);
            CreateControls();
            Color = Color.FromArgb(255, 50, 60, 70);
        }

        protected virtual void CreateColorControl(String name, int y)
        {
            int colorSize = 12;

            GroupBox colorGroup = new GroupBox(this);
            colorGroup.SetPos(10, y);
            colorGroup.SetText(name);
            colorGroup.SetSize(160, 35);
            colorGroup.Name = name + "groupbox";

            ColorDisplay disp = new ColorDisplay(colorGroup);
            disp.Name=name;
            disp.SetBounds(0, 10, colorSize, colorSize);

            TextBoxNumeric numeric = new TextBoxNumeric(colorGroup);
            numeric.Name=name + "Box";
            numeric.SetPos(105, 7);
            numeric.SetSize(26, 16);
            numeric.SelectAllOnFocus=true;
            numeric.OnTextChanged += NumericTyped;

            HorizontalSlider slider = new HorizontalSlider(colorGroup);
            slider.SetPos(colorSize + 5, 10);
            slider.SetRange(0, 255);
            slider.SetSize(80, colorSize);
            slider.Name = name + "Slider";
            slider.OnValueChanged += SlidersMoved;
        }

        protected virtual void NumericTyped(Base control)
        {
            TextBoxNumeric box = control as TextBoxNumeric;
            if (null==box)
                return;

            if (box.Text == string.Empty)
                return;

            int textValue = (int) box.Value;
            if (textValue < 0) textValue = 0;
            if (textValue > 255) textValue = 255;

            if (box.Name.Contains("Red"))
                R = textValue;

            if (box.Name.Contains("Green"))
                G = textValue;

            if (box.Name.Contains("Blue"))
                B = textValue;

            if (box.Name.Contains("Alpha"))
                A = textValue;

            UpdateControls();
        }

        protected virtual void CreateControls()
        {
            int startY = 5;
            int height = 35;

            CreateColorControl("Red", startY);
            CreateColorControl("Green", startY + height);
            CreateColorControl("Blue", startY + height*2);
            CreateColorControl("Alpha", startY + height*3);

            GroupBox finalGroup = new GroupBox(this);
            finalGroup.SetPos(180, 40);
            finalGroup.SetSize(60, 60);
            finalGroup.SetText("Result");
            finalGroup.Name = "ResultGroupBox";

            ColorDisplay disp = new ColorDisplay(finalGroup);
            disp.Name = "Result";
            disp.SetBounds(0, 10, 32, 32);
            disp.DrawCheckers = true;

            //UpdateControls();
        }

        protected virtual void UpdateColorControls(String name, Color col, int sliderVal)
        {
            ColorDisplay disp = FindChildByName(name, true) as ColorDisplay;
            disp.Color = col;

            HorizontalSlider slider = FindChildByName(name + "Slider", true) as HorizontalSlider;
            slider.Value = sliderVal;

            TextBoxNumeric box = FindChildByName(name + "Box", true) as TextBoxNumeric;
            box.Value = sliderVal;
        }

        protected virtual void UpdateControls()
        {	//This is a little weird, but whatever for now
            UpdateColorControls("Red", Color.FromArgb(255, Color.R, 0, 0), Color.R);
            UpdateColorControls("Green", Color.FromArgb(255, 0, Color.G, 0), Color.G);
            UpdateColorControls("Blue", Color.FromArgb(255, 0, 0, Color.B), Color.B);
            UpdateColorControls("Alpha", Color.FromArgb(Color.A, 255, 255, 255), Color.A);

            ColorDisplay disp = FindChildByName("Result", true) as ColorDisplay;
            disp.Color = Color;

            if (OnColorChanged != null)
                OnColorChanged.Invoke(this);
        }

        protected virtual void SlidersMoved(Base control)
        {
            /*
            HorizontalSlider* redSlider		= gwen_cast<HorizontalSlider>(	FindChildByName( "RedSlider",   true ) );
            HorizontalSlider* greenSlider	= gwen_cast<HorizontalSlider>(	FindChildByName( "GreenSlider", true ) );
            HorizontalSlider* blueSlider	= gwen_cast<HorizontalSlider>(	FindChildByName( "BlueSlider",  true ) );
            HorizontalSlider* alphaSlider	= gwen_cast<HorizontalSlider>(	FindChildByName( "AlphaSlider", true ) );
            */

            HorizontalSlider slider = control as HorizontalSlider;
            if (slider != null)
                SetColorByName(GetColorFromName(slider.Name), (int)slider.Value);

            UpdateControls();
            //SetColor( Gwen::Color( redSlider->GetValue(), greenSlider->GetValue(), blueSlider->GetValue(), alphaSlider->GetValue() ) );
        }

        protected override void Layout(Skin.Base skin)
        {
            base.Layout(skin);

            SizeToChildren(false, true);
            SetSize(Width, Height + 5);

            GroupBox groupBox = FindChildByName("ResultGroupBox", true) as GroupBox;
            if (groupBox != null)
                groupBox.SetPos(groupBox.X, Height * 0.5f - groupBox.Height * 0.5f);

            //UpdateControls(); // this spams events continuously every tick
        }

        protected int GetColorByName(String colorName)
        {
            if (colorName == "Red")
                return Color.R;
            if (colorName == "Green")
                return Color.G;
            if (colorName == "Blue")
                return Color.B;
            if (colorName == "Alpha")
                return Color.A;
            return 0;
        }

        protected String GetColorFromName(String name)
        {
            if (name.Contains("Red"))
                return "Red";
            if (name.Contains("Green"))
                return "Green";
            if (name.Contains("Blue"))
                return "Blue";
            if (name.Contains("Alpha"))
                return "Alpha";
            return String.Empty;
        }

        protected void SetColorByName(String colorName, int colorValue)
        {
            if (colorName == "Red")
                R = colorValue;
            else if (colorName == "Green")
                G = colorValue;
            else if (colorName == "Blue")
                B = colorValue;
            else if (colorName == "Alpha")
                A = colorValue;
        }

        public void SetAlphaVisible(bool value)
        {
            GroupBox gb = FindChildByName("Alphagroupbox", true) as GroupBox;
            gb.IsHidden = !value;
            Invalidate();
        }
    }
}
