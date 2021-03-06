﻿using System;
using System.Drawing;
using FoxTrader.Game;
using FoxTrader.UI;
using FoxTrader.UI.Control;
using FoxTrader.UI.Control.Layout;
using static FoxTrader.Constants;

namespace FoxTrader.Views
{
    // ReSharper disable once UnusedMember.Global
    class BumpersView : BaseGameView
    {
        private DateTime m_startTime;

        public BumpersView(GameControl c_controlParent) : base(c_controlParent)
        {
            Dock = Pos.Fill;

            var a_center = new Center(this);

            a_center.Dock = Pos.Fill;

            var a_label = new Label(a_center);
            a_label.AutoSizeToContents = true;
            a_label.Text = "A Fox Council Game";
            a_label.Dock = Pos.CenterH | Pos.CenterV;
            a_label.TextColor = Color.White;

            m_startTime = DateTime.Now;
        }

        internal override void DoUpdate()
        {
#if !DEBUG
            var a_timeDifference = DateTime.Now - m_startTime;

            if (a_timeDifference.Seconds >= 5)
            {
#endif
            GameContext.Instance.MarkStateComplete(ContextState.Bumpers);
#if !DEBUG
            }
#endif
        }
    }
}
