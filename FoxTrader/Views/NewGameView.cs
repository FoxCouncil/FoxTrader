﻿using System;
using FoxTrader.Game;
using FoxTrader.UI;
using FoxTrader.UI.Control;
using static FoxTrader.Constants;

namespace FoxTrader.Views
{
    // ReSharper disable once UnusedMember.Global
    class NewGameView : BaseGameView
    {
        private DateTime m_startTime;

        public NewGameView(GameControl c_controlParent) : base(c_controlParent)
        {
            Dock = Pos.Fill;

            m_startTime = DateTime.Now;
        }

        internal override void DoUpdate()
        {
#if !DEBUG
            var a_timeDifference = DateTime.Now - m_startTime;

            if (a_timeDifference.Seconds >= 5)
            {
#endif
                GameContext.Instance.MarkStateComplete(ContextState.NewGame);
#if !DEBUG
            }
#endif
        }
    }
}
