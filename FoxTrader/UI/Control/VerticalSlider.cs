using FoxTrader.UI.Skin;
using OpenTK.Input;

namespace FoxTrader.UI.Control
{
    /// <summary>Vertical slider</summary>
    internal class VerticalSlider : Slider
    {
        /// <summary>Initializes a new instance of the <see cref="VerticalSlider" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public VerticalSlider(GameControl c_parentControl) : base(c_parentControl)
        {
            m_sliderBar.IsHorizontal = false;
        }

        protected override float CalculateValue()
        {
            return 1 - m_sliderBar.Y / (float)(Height - m_sliderBar.Height);
        }

        protected override void UpdateBarFromValue()
        {
            m_sliderBar.MoveTo(m_sliderBar.X, (int)((Height - m_sliderBar.Height) * (1 - m_value)));
        }

        /// <summary>Handler invoked on mouse click (left) event</summary>
        /// <param name="c_x">X coordinate</param>
        /// <param name="c_y">Y coordinate</param>
        /// <param name="c_down">If set to <c>true</c> mouse button is down</param>
        public override void OnMouseDown(MouseButtonEventArgs c_mouseButtonEventArgs)
        {
            m_sliderBar.MoveTo(m_sliderBar.X, (int)(CanvasPosToLocal(c_mouseButtonEventArgs.Position).Y - m_sliderBar.Height * 0.5));
            m_sliderBar.OnMouseDown(c_mouseButtonEventArgs);
            OnMoved(m_sliderBar);
        }

        /// <summary>Lays out the control's interior according to alignment, padding, dock etc</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void OnLayout(SkinBase c_skin)
        {
            m_sliderBar.SetSize(Width, 15);
            UpdateBarFromValue();
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(SkinBase c_skin)
        {
            c_skin.DrawSlider(this, false, m_snapToNotches ? m_notchCount : 0, m_sliderBar.Height);
        }
    }
}