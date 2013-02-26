using Gwen.Drawing;

namespace Gwen.Controls.Internal
{
    /// <summary>
    /// Slider bar.
    /// </summary>
    public class SliderBar : Dragger
    {
        /// <summary>
        /// Indicates whether the bar is horizontal.
        /// </summary>
        public bool IsHorizontal { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SliderBar"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public SliderBar(Control parent)
            : base(parent)
        {
            Target = this;
            RestrictToParent = true;
        }

        /// <summary>
        /// Renders the control using specified skin.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Render(Skin skin)
        {
            skin.DrawSliderButton(this, IsHeld, IsHorizontal);
        }
    }
}
