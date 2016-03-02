using System.Drawing;
using FoxTrader.UI.Skin;

namespace FoxTrader.UI.Control
{
    /// <summary>Horizontal slider</summary>
    internal class HorizontalSlider : Slider
    {
        /// <summary>Initializes a new instance of the <see cref="HorizontalSlider" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public HorizontalSlider(GameControl c_parentControl) : base(c_parentControl)
        {
            m_sliderBar.IsHorizontal = true;
        }

        protected override float CalculateValue()
        {
            return (float)m_sliderBar.X / (Width - m_sliderBar.Width);
        }

        protected override void UpdateBarFromValue()
        {
            m_sliderBar.MoveTo((int)((Width - m_sliderBar.Width) * (m_value)), m_sliderBar.Y);
        }

        /// <summary>Handler invoked on mouse click (left) event</summary>
        /// <param name="c_x">X coordinate</param>
        /// <param name="c_y">Y coordinate</param>
        /// <param name="c_down">If set to <c>true</c> mouse button is down</param>
        protected override void OnMouseClickedLeft(int c_x, int c_y, bool c_down)
        {
            m_sliderBar.MoveTo((int)(CanvasPosToLocal(new Point(c_x, c_y)).X - m_sliderBar.Width * 0.5), m_sliderBar.Y);
            m_sliderBar.InputMouseClickedLeft(c_x, c_y, c_down);
            OnMoved(m_sliderBar);
        }

        /// <summary>Lays out the control's interior according to alignment, padding, dock etc</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Layout(SkinBase c_skin)
        {
            m_sliderBar.SetSize(15, Height);
            UpdateBarFromValue();
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(SkinBase c_skin)
        {
            c_skin.DrawSlider(this, true, m_snapToNotches ? m_notchCount : 0, m_sliderBar.Width);
        }
    }
}