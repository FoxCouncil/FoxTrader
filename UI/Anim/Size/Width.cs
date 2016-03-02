using System;

namespace FoxTrader.UI.Anim.Size
{
    internal class Width : TimedAnimation
    {
        private readonly int m_delta;
        private readonly bool m_hide;
        private readonly int m_startSize;

        public Width(int c_startSize, int c_endSize, float c_length, bool c_hide = false, float c_delay = 0.0f, float c_ease = 1.0f) : base(c_length, c_delay, c_ease)
        {
            m_startSize = c_startSize;
            m_delta = c_endSize - m_startSize;
            m_hide = c_hide;
        }

        protected override void OnStart()
        {
            base.OnStart();
            m_control.Width = m_startSize;
        }

        protected override void Run(float c_delta)
        {
            base.Run(c_delta);
            m_control.Width = (int)Math.Round(m_startSize + (m_delta * c_delta));
        }

        protected override void OnFinish()
        {
            base.OnFinish();
            m_control.Width = m_startSize + m_delta;
            m_control.IsHidden = m_hide;
        }
    }
}