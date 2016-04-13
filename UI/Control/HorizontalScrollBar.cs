using FoxTrader.UI.Skin;
using OpenTK.Input;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control
{
    /// <summary>Horizontal scrollbar</summary>
    internal class HorizontalScrollBar : ScrollBar
    {
        /// <summary>Initializes a new instance of the <see cref="HorizontalScrollBar" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        internal HorizontalScrollBar(GameControl c_parentControl) : base(c_parentControl)
        {
            m_bar.IsHorizontal = true;

            m_scrollButton[0].SetDirectionLeft();
            m_scrollButton[0].Clicked += NudgeLeft;

            m_scrollButton[1].SetDirectionRight();
            m_scrollButton[1].Clicked += NudgeRight;

            m_bar.Dragged += OnBarMoved;
        }

        /// <summary>Bar size (in pixels)</summary>
        public override int BarSize
        {
            get
            {
                return m_bar.Width;
            }
            set
            {
                m_bar.Width = value;
            }
        }

        /// <summary>Bar position (in pixels)</summary>
        public override int BarPos => m_bar.X - Height;

        /// <summary>Indicates whether the bar is horizontal</summary>
        public override bool IsHorizontal => true;

        /// <summary>Button size (in pixels)</summary>
        public override int ButtonSize => Height;

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

            m_scrollButton[0].Width = Height;
            m_scrollButton[0].Dock = Pos.Left;

            m_scrollButton[1].Width = Height;
            m_scrollButton[1].Dock = Pos.Right;

            m_bar.Height = ButtonSize;
            m_bar.Padding = new Padding(ButtonSize, 0, ButtonSize, 0);

            var a_barWidth = (m_viewableContentSize / m_contentSize) * (Width - (ButtonSize * 2));

            if (a_barWidth < ButtonSize * 0.5f)
            {
                a_barWidth = (int)(ButtonSize * 0.5f);
            }

            m_bar.Width = (int)(a_barWidth);
            m_bar.IsHidden = Width - (ButtonSize * 2) <= a_barWidth;

            //Based on our last scroll amount, produce a position for the bar
            if (!m_bar.IsHeld)
            {
                SetScrollAmount(ScrollAmount, true);
            }
        }

        public void NudgeLeft(GameControl c_control, MouseButtonEventArgs c_args)
        {
            if (!IsDisabled)
            {
                SetScrollAmount(ScrollAmount - NudgeAmount, true);
            }
        }

        public void NudgeRight(GameControl c_control, MouseButtonEventArgs c_args)
        {
            if (!IsDisabled)
            {
                SetScrollAmount(ScrollAmount + NudgeAmount, true);
            }
        }

        public override void ScrollToLeft()
        {
            SetScrollAmount(0, true);
        }

        public override void ScrollToRight()
        {
            SetScrollAmount(1, true);
        }

        /// <summary>Handler invoked on mouse click (left) event</summary>
        /// <param name="c_x">X coordinate</param>
        /// <param name="c_y">Y coordinate</param>
        /// <param name="c_down">If set to <c>true</c> mouse button is down</param>
        public override void OnMouseDown(MouseButtonEventArgs c_mouseButtonEventArgs)
        {
            m_isDepressed = true;
            GetCanvas().MouseFocus = this;
        }

        public override void OnMouseUp(MouseButtonEventArgs c_mouseButtonEventArgs)
        {
            var a_clickPos = CanvasPosToLocal(c_mouseButtonEventArgs.Position);

            if (a_clickPos.X < m_bar.X)
            {
                NudgeLeft(this, c_mouseButtonEventArgs);
            }
            else
            {
                if (a_clickPos.X > m_bar.X + m_bar.Width)
                {
                    NudgeRight(this, c_mouseButtonEventArgs);
                }
            }

            m_isDepressed = false;
            GetCanvas().MouseFocus = null;
        }

        protected override float CalculateScrolledAmount()
        {
            return (float)(m_bar.X - ButtonSize) / (Width - m_bar.Width - (ButtonSize * 2));
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
                var a_newX = (int)(ButtonSize + (c_value * ((Width - m_bar.Width) - (ButtonSize * 2))));

                m_bar.MoveTo(a_newX, m_bar.Y);
            }

            return true;
        }

        /// <summary>Handler for the BarMoved event</summary>
        /// <param name="c_control">Event source</param>
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