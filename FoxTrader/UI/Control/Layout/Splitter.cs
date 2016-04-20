using System;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control.Layout
{
    /// <summary>Base splitter class</summary>
    internal class Splitter : GameControl
    {
        private readonly GameControl[] m_panel;
        private readonly bool[] m_scale;

        /// <summary>Initializes a new instance of the <see cref="Splitter" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public Splitter(GameControl c_parentControl) : base(c_parentControl)
        {
            m_panel = new GameControl[2];
            m_scale = new bool[2];
            m_scale[0] = true;
            m_scale[1] = true;
        }

        /// <summary>Sets the contents of a splitter panel</summary>
        /// <param name="c_panelIndex">Panel index (0-1)</param>
        /// <param name="c_panel">Panel contents</param>
        /// <param name="c_noScale">Determines whether the content is to be scaled</param>
        public void SetPanel(int c_panelIndex, GameControl c_panel, bool c_noScale = false)
        {
            if (c_panelIndex < 0 || c_panelIndex > 1)
            {
                throw new ArgumentException("Invalid panel index", "c_panelIndex");
            }

            m_panel[c_panelIndex] = c_panel;
            m_scale[c_panelIndex] = !c_noScale;

            if (null != m_panel[c_panelIndex])
            {
                m_panel[c_panelIndex].Parent = this;
            }
        }

        /// <summary>Gets the contents of a secific panel</summary>
        /// <param name="c_panelIndex">Panel index (0-1)</param>
        /// <returns></returns>
        private GameControl GetPanel(int c_panelIndex)
        {
            if (c_panelIndex < 0 || c_panelIndex > 1)
            {
                throw new ArgumentException("Invalid panel index", "c_panelIndex");
            }
            return m_panel[c_panelIndex];
        }

        /// <summary>Lays out the control's interior according to alignment, padding, dock etc</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void OnLayout(Skin c_skin)
        {
            LayoutVertical(c_skin);
        }

        protected virtual void LayoutVertical(Skin c_skin)
        {
            var a_w = Width;
            var a_h = Height;

            if (m_panel[0] != null)
            {
                var a_m = m_panel[0].Margin;
                if (m_scale[0])
                {
                    m_panel[0].SetBounds(a_m.m_left, a_m.m_top, a_w - a_m.m_left - a_m.m_right, (a_h * 0.5f) - a_m.m_top - a_m.m_bottom);
                }
                else
                {
                    m_panel[0].SetRelativePosition(Pos.Center, 0, (int)(a_h * -0.25f));
                }
            }

            if (m_panel[1] != null)
            {
                var a_m = m_panel[1].Margin;
                if (m_scale[1])
                {
                    m_panel[1].SetBounds(a_m.m_left, a_m.m_top + (a_h * 0.5f), a_w - a_m.m_left - a_m.m_right, (a_h * 0.5f) - a_m.m_top - a_m.m_bottom);
                }
                else
                {
                    m_panel[1].SetRelativePosition(Pos.Center, 0, (int)(a_h * 0.25f));
                }
            }
        }

        protected virtual void LayoutHorizontal(Skin c_skin)
        {
            throw new NotImplementedException();
        }
    }
}