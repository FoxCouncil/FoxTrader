using System;
using FoxTrader.UI.Platform;

namespace FoxTrader.UI.Anim
{
    // Timed animation. Provides a useful base for animations.
    internal class TimedAnimation : Animation
    {
        private readonly float m_ease;
        private readonly float m_end;
        private bool m_finished;
        private readonly float m_start;
        private bool m_started;

        public TimedAnimation(float c_length, float c_delay = 0.0f, float c_ease = 1.0f)
        {
            m_start = Neutral.GetTimeInSeconds() + c_delay;
            m_end = m_start + c_length;
            m_ease = c_ease;
            m_started = false;
            m_finished = false;
        }

        public override bool Finished => m_finished;

        protected override void Think()
        {
            //base.Think();

            if (m_finished)
            {
                return;
            }

            var a_current = Neutral.GetTimeInSeconds();
            var a_secondsIn = a_current - m_start;
            if (a_secondsIn < 0.0)
            {
                return;
            }

            if (!m_started)
            {
                m_started = true;
                OnStart();
            }

            var a_delta = a_secondsIn / (m_end - m_start);
            if (a_delta < 0.0f)
            {
                a_delta = 0.0f;
            }
            if (a_delta > 1.0f)
            {
                a_delta = 1.0f;
            }

            Run((float)Math.Pow(a_delta, m_ease));

            if (a_delta == 1.0f)
            {
                m_finished = true;
                OnFinish();
            }
        }

        // These are the magic functions you should be overriding

        protected virtual void OnStart()
        {
        }

        protected virtual void Run(float c_delta)
        {
        }

        protected virtual void OnFinish()
        {
        }
    }
}