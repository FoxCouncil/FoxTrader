using System;
using FoxTrader.UI;
using FoxTrader.UI.Control;
using FoxTrader.UI.Control.Layout;
using static FoxTrader.Constants;

namespace FoxTrader.Views
{
    class BumpersView : GameView
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
            a_label.MakeColorBright();

            m_startTime = DateTime.Now;
        }

        internal override void DoUpdate()
        {
#if !DEBUG
            var a_timeDifference = DateTime.Now - m_startTime;

            if (a_timeDifference.Seconds >= 5)
            {
#endif
            FoxTraderGame.GameContextInstance.MarkStateComplete(ContextState.Bumpers);
#if !DEBUG
        }
#endif
        }
    }
}
