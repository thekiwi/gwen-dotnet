using System;
using Gwen.ControlInternal;
using Gwen.Input;

namespace Gwen.Control.Property
{
    /// <summary>
    /// Color property.
    /// </summary>
    public class Color : Text
    {
        protected readonly ColorButton m_Button;

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public Color(Control.Base parent) : base(parent)
        {
            m_Button = new ColorButton(m_TextBox);
            m_Button.Dock = Pos.Right;
            m_Button.Width = 20;
            m_Button.Margin = new Margin(1, 1, 1, 2);
            m_Button.Clicked += OnButtonPressed;
        }

        /// <summary>
        /// Color-select button press handler.
        /// </summary>
        /// <param name="control">Event source.</param>
        protected virtual void OnButtonPressed(Control.Base control)
        {
            Menu menu = new Menu(GetCanvas());
            menu.SetSize(256, 180);
            menu.DeleteOnClose = true;
            menu.IconMarginDisabled = true;

            HSVColorPicker picker = new HSVColorPicker(menu);
            picker.Dock = Pos.Fill;
            picker.SetSize(256, 128);

            String[] split = m_TextBox.Text.Split(' ');

            picker.SetColor(System.Drawing.Color.FromArgb(255, 
                Convert.ToInt32(split[0]), Convert.ToInt32(split[1]), Convert.ToInt32(split[2])), false, true);
            picker.ColorChanged += OnColorChanged;

            menu.Open(Pos.Right | Pos.Top);
        }

        /// <summary>
        /// Color changed handler.
        /// </summary>
        /// <param name="control">Event source.</param>
        protected virtual void OnColorChanged(Control.Base control)
        {
            HSVColorPicker picker = control as HSVColorPicker;
            System.Drawing.Color col = picker.SelectedColor;
            m_TextBox.Text = String.Format("{0} {1} {2}", col.R, col.G, col.B);
            DoChanged();
        }

        /// <summary>
        /// Property value.
        /// </summary>
        public override string Value
        {
            get { return m_TextBox.Text; }
            set { base.Value = value; }
        }

        /// <summary>
        /// Sets the property value.
        /// </summary>
        /// <param name="value">Value to set.</param>
        /// <param name="fireEvents">Determines whether to fire "value changed" event.</param>
        public override void SetValue(string value, bool fireEvents = false)
        {
            m_TextBox.SetText(value, fireEvents);
        }

        /// <summary>
        /// Indicates whether the property value is being edited.
        /// </summary>
        public override bool IsEditing
        {
            get { return m_TextBox == InputHandler.KeyboardFocus; }
        }

        protected override void DoChanged()
        {
            base.DoChanged();
            String[] split = m_TextBox.Text.Split(' ');
            m_Button.Color = System.Drawing.Color.FromArgb(255, 
                Convert.ToInt32(split[0]), Convert.ToInt32(split[1]), Convert.ToInt32(split[2]));
        }
    }
}
