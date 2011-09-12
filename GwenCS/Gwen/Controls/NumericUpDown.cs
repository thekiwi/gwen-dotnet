using System;
using Gwen.Controls.Layout;

namespace Gwen.Controls
{
    /// <summary>
    /// Numeric up arrow.
    /// </summary>
    public class NumericUpDownButton_Up : Button
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumericUpDownButton_Up"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public NumericUpDownButton_Up(Base parent)
            : base(parent)
        {
            SetSize(7, 7);
        }

        /// <summary>
        /// Renders the control using specified skin.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Render(Skin.Base skin)
        {
            skin.DrawNumericUpDownButton(this, IsDepressed, true);
        }
    }

    /// <summary>
    /// Numeric down arrow.
    /// </summary>
    internal class NumericUpDownButton_Down : Button
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumericUpDownButton_Down"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public NumericUpDownButton_Down(Base parent)
            : base(parent)
        {
            SetSize(7, 7);
        }

        /// <summary>
        /// Renders the control using specified skin.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Render(Skin.Base skin)
        {
            skin.DrawNumericUpDownButton(this, IsDepressed, false);
        }
    }

    /// <summary>
    /// Numeric up/down.
    /// </summary>
    public class NumericUpDown : TextBoxNumeric
    {
        protected int m_Max;
        protected int m_Min;

        private readonly Splitter m_Splitter;
        private readonly NumericUpDownButton_Up m_Up;
        private readonly NumericUpDownButton_Down m_Down;

        /// <summary>
        /// Minimum value.
        /// </summary>
        public int Min { get { return m_Min; } set { m_Min = value; } }

        /// <summary>
        /// Maximum value.
        /// </summary>
        public int Max { get { return m_Max; } set { m_Max = value; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericUpDown"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public NumericUpDown(Base parent)
            : base(parent)
        {
            SetSize(100, 20);

            m_Splitter = new Splitter(this);
            m_Splitter.Dock = Pos.Right;
            m_Splitter.SetSize(13, 13);

            m_Up = new NumericUpDownButton_Up(m_Splitter);
            m_Up.OnPress += onButtonUp;
            m_Up.IsTabable = false;
            m_Splitter.SetPanel(0, m_Up, false);

            m_Down = new NumericUpDownButton_Down(m_Splitter);
            m_Down.OnPress += onButtonDown;
            m_Down.IsTabable = false;
            m_Down.Padding = new Padding(0, 1, 1, 0);
            m_Splitter.SetPanel(1, m_Down, false);

            m_Max = 100;
            m_Min = 0;
            m_Value = 0f;
            Text = "0";
        }

        /// <summary>
        /// Invoked when the value has changed.
        /// </summary>
        public event ControlCallback OnValueChanged;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            m_Splitter.Dispose();
            m_Up.Dispose();
            m_Down.Dispose();
            base.Dispose();
        }

        /// <summary>
        /// Handler for Up Arrow keyboard event.
        /// </summary>
        /// <param name="down">Indicates whether the key was pressed or released.</param>
        /// <returns>
        /// True if handled.
        /// </returns>
        protected override bool onKeyUp(bool down)
        {
            if (down) onButtonUp(null);
            return true;
        }

        /// <summary>
        /// Handler for Down Arrow keyboard event.
        /// </summary>
        /// <param name="down">Indicates whether the key was pressed or released.</param>
        /// <returns>
        /// True if handled.
        /// </returns>
        protected override bool onKeyDown(bool down)
        {
            if (down) onButtonDown(null);
            return true;
        }

        /// <summary>
        /// Handler for the button up event.
        /// </summary>
        /// <param name="control">Event source.</param>
        protected virtual void onButtonUp(Base control)
        {
            Value = m_Value + 1;
        }

        /// <summary>
        /// Handler for the button down event.
        /// </summary>
        /// <param name="control">Event source.</param>
        protected virtual void onButtonDown(Base control)
        {
            Value = m_Value - 1;
        }

        /// <summary>
        /// Determines whether the text can be assighed to the control.
        /// </summary>
        /// <param name="str">Text to evaluate.</param>
        /// <returns>True if the text is allowed.</returns>
        protected override bool IsTextAllowed(string str)
        {
            float d;
            if (!float.TryParse(str, out d))
                return false;
            if (d < m_Min) return false;
            if (d > m_Max) return false;
            return true;
        }

        /// <summary>
        /// Numeric value of the control.
        /// </summary>
        public override float Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                if (value < m_Min) value = m_Min;
                if (value > m_Max) value = m_Max;
                if (value == m_Value) return;

                base.Value = value;
            }
        }

        /// <summary>
        /// Handler for the text changed event.
        /// </summary>
        protected override void onTextChanged()
        {
            base.onTextChanged();
            if (OnValueChanged != null)
                OnValueChanged.Invoke(this);
        }
    }
}
