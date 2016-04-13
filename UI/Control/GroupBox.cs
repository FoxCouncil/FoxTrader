using FoxTrader.UI.Skin;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control
{
    /// <summary>Group box (container)</summary>
    /// <remarks>Don't use autosize with docking</remarks>
    internal class GroupBox : Label
    {
        /// <summary>Initializes a new instance of the <see cref="GroupBox" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public GroupBox(GameControl c_parentControl) : base(c_parentControl)
        {
            // Set to true, because it's likely that our  
            // children will want mouse input, and they
            // can't get it without us..
            MouseInputEnabled = true;
            KeyboardInputEnabled = true;

            TextPadding = new Padding(10, 0, 10, 0);
            Alignment = Pos.Top | Pos.Left;
            Invalidate();

            m_innerControl = new GameControl(this);
            m_innerControl.Dock = Pos.Fill;
            m_innerControl.Margin = new Margin(5, TextHeight + 5, 5, 5);
            //Margin = new Margin(5, 5, 5, 5);
        }

        /// <summary>Lays out the control's interior according to alignment, padding, dock etc</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void OnLayout(SkinBase c_skin)
        {
            base.OnLayout(c_skin);
            if (AutoSizeToContents)
            {
                DoSizeToContents();
            }
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(SkinBase c_skin)
        {
            c_skin.DrawGroupBox(this, TextX, TextHeight, TextWidth);
        }

        /// <summary>Sizes to contents</summary>
        public override void SizeToContents()
        {
            // we inherit from Label and shouldn't use its method.
            DoSizeToContents();
        }

        protected virtual void DoSizeToContents()
        {
            m_innerControl.SizeToChildren();
            SizeToChildren();
            if (Width < TextWidth + TextPadding.m_right + TextPadding.m_left)
            {
                Width = TextWidth + TextPadding.m_right + TextPadding.m_left;
            }
        }
    }
}