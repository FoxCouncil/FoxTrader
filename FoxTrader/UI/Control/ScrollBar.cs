using FoxTrader.UI.ControlInternal;

namespace FoxTrader.UI.Control
{
    /// <summary>Base class for scrollbars</summary>
    internal class ScrollBar : GameControl
    {
        protected readonly ScrollBarBar m_bar;
        protected readonly ScrollBarButton[] m_scrollButton;
        protected float m_contentSize;

        protected bool m_isDepressed;
        protected float m_nudgeAmount;
        protected float m_scrollAmount;
        protected float m_viewableContentSize;

        /// <summary>Initializes a new instance of the <see cref="ScrollBar" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        protected ScrollBar(GameControl c_parentControl) : base(c_parentControl)
        {
            m_scrollButton = new ScrollBarButton[2];
            m_scrollButton[0] = new ScrollBarButton(this);
            m_scrollButton[1] = new ScrollBarButton(this);

            m_bar = new ScrollBarBar(this);

            SetBounds(0, 0, 15, 15);
            m_isDepressed = false;

            m_scrollAmount = 0;
            m_contentSize = 0;
            m_viewableContentSize = 0;

            NudgeAmount = 20;
        }

        /// <summary>Bar size (in pixels)</summary>
        public virtual int BarSize
        {
            get;
            set;
        }

        /// <summary>Bar position (in pixels)</summary>
        public virtual int BarPos => 0;

        /// <summary>Button size (in pixels)</summary>
        public virtual int ButtonSize => 0;

        public virtual float NudgeAmount
        {
            get
            {
                return m_nudgeAmount / m_contentSize;
            }
            set
            {
                m_nudgeAmount = value;
            }
        }

        public float ScrollAmount => m_scrollAmount;

        public float ContentSize
        {
            get
            {
                return m_contentSize;
            }
            set
            {
                if (m_contentSize != value)
                {
                    Invalidate();
                }
                m_contentSize = value;
            }
        }

        public float ViewableContentSize
        {
            get
            {
                return m_viewableContentSize;
            }
            set
            {
                if (m_viewableContentSize != value)
                {
                    Invalidate();
                }
                m_viewableContentSize = value;
            }
        }

        /// <summary>Indicates whether the bar is horizontal</summary>
        public virtual bool IsHorizontal => false;

        /// <summary>Invoked when the bar is moved</summary>
        public event ScrollEventHandler BarMoved;

        /// <summary>Sets the scroll amount (0-1)</summary>
        /// <param name="c_value">Scroll amount</param>
        /// <param name="c_forceUpdate">Determines whether the control should be updated</param>
        /// <returns>True if control state changed</returns>
        public virtual bool SetScrollAmount(float c_value, bool c_forceUpdate = false)
        {
            if (m_scrollAmount == c_value && !c_forceUpdate)
            {
                return false;
            }
            m_scrollAmount = c_value;
            Invalidate();
            OnBarMoved(this);
            return true;
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(Skin c_skin)
        {
            c_skin.DrawScrollBar(this, IsHorizontal, m_isDepressed);
        }

        /// <summary>Handler for the BarMoved event</summary>
        /// <param name="c_control">The control</param>
        protected virtual void OnBarMoved(GameControl c_control)
        {
            BarMoved?.Invoke(this);
        }

        protected virtual float CalculateScrolledAmount()
        {
            return 0;
        }

        protected virtual int CalculateBarSize()
        {
            return 0;
        }

        public virtual void ScrollToLeft()
        {
        }

        public virtual void ScrollToRight()
        {
        }

        public virtual void ScrollToTop()
        {
        }

        public virtual void ScrollToBottom()
        {
        }
    }
}