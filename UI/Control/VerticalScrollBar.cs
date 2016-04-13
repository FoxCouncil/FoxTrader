using FoxTrader.UI.Skin;
using OpenTK.Input;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control
{
    /// <summary>Vertical scrollbar</summary>
    internal class VerticalScrollBar : ScrollBar
    {
        /// <summary>Initializes a new instance of the <see cref="VerticalScrollBar" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public VerticalScrollBar(GameControl c_parentControl) : base(c_parentControl)
        {
            m_bar.IsVertical = true;

            m_scrollButton[0].SetDirectionUp();
            m_scrollButton[0].Clicked += NudgeUp;

            m_scrollButton[1].SetDirectionDown();
            m_scrollButton[1].Clicked += NudgeDown;

            m_bar.Dragged += OnBarMoved;
        }

        /// <summary>Bar size (in pixels)</summary>
        public override int BarSize
        {
            get
            {
                return m_bar.Height;
            }
            set
            {
                m_bar.Height = value;
            }
        }

        /// <summary>Bar position (in pixels)</summary>
        public override int BarPos => m_bar.Y - Width;

        /// <summary>Button size (in pixels)</summary>
        public override int ButtonSize => Width;

        public override float NudgeAmount
        {
            get
            {
                if (m_isDepressed)
                {
                    return m_viewableContentSize / m_contentSize;
                }
                return base.NudgeAmount;
            }
            set
            {
                base.NudgeAmount = value;
            }
        }

        /// <summary>Lays out the control's interior according to alignment, padding, dock etc</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void OnLayout(SkinBase c_skin)
        {
            base.OnLayout(c_skin);

            m_scrollButton[0].Height = Width;
            m_scrollButton[0].Dock = Pos.Top;

            m_scrollButton[1].Height = Width;
            m_scrollButton[1].Dock = Pos.Bottom;

            m_bar.Width = ButtonSize;
            m_bar.Padding = new Padding(0, ButtonSize, 0, ButtonSize);

            var a_barHeight = 0.0f;

            if (m_contentSize > 0.0f)
            {
                a_barHeight = (m_viewableContentSize / m_contentSize) * (Height - (ButtonSize * 2));
            }

            if (a_barHeight < ButtonSize * 0.5f)
            {
                a_barHeight = (int)(ButtonSize * 0.5f);
            }

            m_bar.Height = (int)a_barHeight;
            m_bar.IsHidden = Height - (ButtonSize * 2) <= a_barHeight;

            //Based on our last scroll amount, produce a position for the bar
            if (!m_bar.IsHeld)
            {
                SetScrollAmount(ScrollAmount, true);
            }
        }

        public virtual void NudgeUp(GameControl c_control, MouseButtonEventArgs c_args)
        {
            if (!IsDisabled)
            {
                SetScrollAmount(ScrollAmount - NudgeAmount, true);
            }
        }

        public virtual void NudgeDown(GameControl c_control, MouseButtonEventArgs c_args)
        {
            if (!IsDisabled)
            {
                SetScrollAmount(ScrollAmount + NudgeAmount, true);
            }
        }

        public override void ScrollToTop()
        {
            SetScrollAmount(0, true);
        }

        public override void ScrollToBottom()
        {
            SetScrollAmount(1, true);
        }

        public override void OnMouseDown(MouseButtonEventArgs c_mouseButtonEventArgs)
        {
            m_isDepressed = true;
            GetCanvas().MouseFocus = this;
        }

        public override void OnMouseUp(MouseButtonEventArgs c_mouseButtonEventArgs)
        {
            base.OnMouseUp(c_mouseButtonEventArgs);

            var a_clickPosition = CanvasPosToLocal(c_mouseButtonEventArgs.Position);

            if (a_clickPosition.Y < m_bar.Y)
            {
                NudgeUp(this, c_mouseButtonEventArgs);
            }
            else if (a_clickPosition.Y > m_bar.Y + m_bar.Height)
            {
                NudgeDown(this, c_mouseButtonEventArgs);
            }

            m_isDepressed = false;

            GetCanvas().MouseFocus = null;
        }

        protected override float CalculateScrolledAmount()
        {
            return (float)(m_bar.Y - ButtonSize) / (Height - m_bar.Height - (ButtonSize * 2));
        }

        /// <summary>Sets the scroll amount (0-1)</summary>
        /// <param name="c_value">Scroll amount</param>
        /// <param name="c_forceUpdate">Determines whether the control should be updated</param>
        /// <returns>True if control state changed</returns>
        public override bool SetScrollAmount(float c_value, bool c_forceUpdate = false)
        {
            c_value = Util.Clamp(c_value, 0, 1);

            if (!base.SetScrollAmount(c_value, c_forceUpdate))
            {
                return false;
            }

            if (c_forceUpdate)
            {
                var a_newY = (int)(ButtonSize + (c_value * ((Height - m_bar.Height) - (ButtonSize * 2))));
                m_bar.MoveTo(m_bar.X, a_newY);
            }

            return true;
        }

        /// <summary>Handler for the BarMoved event</summary>
        /// <param name="c_control">The control</param>
        protected override void OnBarMoved(GameControl c_control)
        {
            if (m_bar.IsHeld)
            {
                SetScrollAmount(CalculateScrolledAmount(), false);
                base.OnBarMoved(c_control);
            }
            else
            {
                InvalidateParent();
            }
        }
    }
}