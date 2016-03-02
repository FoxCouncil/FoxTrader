using System;
using System.Collections.Generic;
using FoxTrader.UI.Control;

namespace FoxTrader.UI.Anim
{
    internal class Animation
    {
        //private static List<Animation> g_AnimationsListed = new List<Animation>(); // unused
        private static readonly Dictionary<GameControl, List<Animation>> m_animations = new Dictionary<GameControl, List<Animation>>();
        protected GameControl m_control;

        public virtual bool Finished
        {
            get
            {
                throw new InvalidOperationException("Pure virtual function call");
            }
        }

        protected virtual void Think()
        {
        }

        internal static void Add(GameControl c_control, Animation c_animation)
        {
            c_animation.m_control = c_control;
            if (!m_animations.ContainsKey(c_control))
            {
                m_animations[c_control] = new List<Animation>();
            }
            m_animations[c_control].Add(c_animation);
        }

        internal static void Cancel(GameControl c_control)
        {
            if (!m_animations.ContainsKey(c_control))
            {
                return;
            }

            m_animations[c_control].Clear();
            m_animations.Remove(c_control);
        }

        internal static void GlobalThink()
        {
            foreach (var a_pair in m_animations)
            {
                var a_valCopy = a_pair.Value.FindAll(c_x => true); // list copy so foreach won't break when we remove elements
                foreach (var a_animation in a_valCopy)
                {
                    a_animation.Think();
                    if (a_animation.Finished)
                    {
                        a_pair.Value.Remove(a_animation);
                    }
                }
            }
        }
    }
}