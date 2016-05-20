using System;
using System.Linq;
using OpenTK.Input;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control
{
    /// <summary>Base for controls whose interior can be scrolled</summary>
    internal class ScrollControl : GameControl
    {
        private readonly ScrollBar m_horizontalScrollBar;

        private readonly ScrollBar m_verticalScrollBar;

        /// <summary>Initializes a new instance of the <see cref="ScrollControl" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public ScrollControl(GameControl c_parentControl) : base(c_parentControl)
        {
            MouseInputEnabled = false;

            m_verticalScrollBar = new VerticalScrollBar(this);
            m_verticalScrollBar.Dock = Pos.Right;
            m_verticalScrollBar.BarMoved += VBarMoved;
            CanScrollV = true;
            m_verticalScrollBar.NudgeAmount = 30;

            m_horizontalScrollBar = new HorizontalScrollBar(this);
            m_horizontalScrollBar.Dock = Pos.Bottom;
            m_horizontalScrollBar.BarMoved += HBarMoved;
            CanScrollH = true;
            m_horizontalScrollBar.NudgeAmount = 30;

            m_innerControl = new GameControl(this);
            m_innerControl.SetPosition(0, 0);
            m_innerControl.Margin = Margin.kFive;
            m_innerControl.SendToBack();
            m_innerControl.MouseInputEnabled = false;

            AutoHideBars = false;
        }

        /// <summary>Indicates whether the control can be scrolled horizontally</summary>
        public bool CanScrollH
        {
            get;
            private set;
        }

        /// <summary>Indicates whether the control can be scrolled vertically</summary>
        public bool CanScrollV
        {
            get;
            private set;
        }

        /// <summary>Determines whether the scroll bars should be hidden if not needed</summary>
        public bool AutoHideBars
        {
            get;
            set;
        }

        protected bool HScrollRequired
        {
            set
            {
                if (value)
                {
                    m_horizontalScrollBar.SetScrollAmount(0, true);
                    m_horizontalScrollBar.IsDisabled = true;
                    if (AutoHideBars)
                    {
                        m_horizontalScrollBar.IsHidden = true;
                    }
                }
                else
                {
                    m_horizontalScrollBar.IsHidden = false;
                    m_horizontalScrollBar.IsDisabled = false;
                }
            }
        }

        protected bool VScrollRequired
        {
            set
            {
                if (value)
                {
                    m_verticalScrollBar.SetScrollAmount(0, true);
                    m_verticalScrollBar.IsDisabled = true;
                    if (AutoHideBars)
                    {
                        m_verticalScrollBar.IsHidden = true;
                    }
                }
                else
                {
                    m_verticalScrollBar.IsHidden = false;
                    m_verticalScrollBar.IsDisabled = false;
                }
            }
        }

        /// <summary>Enables or disables inner scrollbars</summary>
        /// <param name="c_horizontal">Determines whether the horizontal scrollbar should be enabled</param>
        /// <param name="c_vertical">Determines whether the vertical scrollbar should be enabled</param>
        public virtual void EnableScroll(bool c_horizontal, bool c_vertical)
        {
            CanScrollV = c_vertical;
            CanScrollH = c_horizontal;
            m_verticalScrollBar.IsHidden = !CanScrollV;
            m_horizontalScrollBar.IsHidden = !CanScrollH;
        }

        public virtual void SetInnerSize(int c_width, int c_height)
        {
            m_innerControl.SetSize(c_width, c_height);
        }

        protected virtual void VBarMoved(GameControl c_control)
        {
            Invalidate();
        }

        protected virtual void HBarMoved(GameControl c_control)
        {
            Invalidate();
        }

        /// <summary>Lays out the control's interior according to alignment, padding, dock etc</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void OnLayout(Skin c_skin)
        {
            UpdateScrollBars();
            base.OnLayout(c_skin);
        }

        /// <summary>Handler invoked on mouse wheel event</summary>
        /// <param name="c_delta">Scroll delta</param>
        /// <returns></returns>
        public override void OnMouseWheel(MouseWheelEventArgs c_mouseWheelEventArgs)
        {
            if (CanScrollV && m_verticalScrollBar.IsVisible)
            {
                if (m_verticalScrollBar.SetScrollAmount(m_verticalScrollBar.ScrollAmount - m_verticalScrollBar.NudgeAmount * (c_mouseWheelEventArgs.Delta / 60.0f), true))
                {
                    return;
                }
            }

            if (CanScrollH && m_horizontalScrollBar.IsVisible)
            {
                if (m_horizontalScrollBar.SetScrollAmount(m_horizontalScrollBar.ScrollAmount - m_horizontalScrollBar.NudgeAmount * (c_mouseWheelEventArgs.Delta / 60.0f), true))
                {
                    return;
                }
            }
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(Skin c_skin)
        {
        }

        public virtual void UpdateScrollBars()
        {
            if (null == m_innerControl)
            {
                return;
            }

            //Get the max size of all our children together
            var a_childrenWidth = Children.Count > 0 ? Children.Max(c_x => c_x.Right) : 0;
            var a_childrenHeight = Children.Count > 0 ? Children.Max(c_x => c_x.Bottom) : 0;

            if (CanScrollH)
            {
                m_innerControl.SetSize(Math.Max(Width, a_childrenWidth), Math.Max(Height, a_childrenHeight));
            }
            else
            {
                m_innerControl.SetSize(Width - (m_verticalScrollBar.IsHidden ? 0 : m_verticalScrollBar.Width), Math.Max(Height, a_childrenHeight));
            }

            var a_wPercent = Width / (float)(a_childrenWidth + (m_verticalScrollBar.IsHidden ? 0 : m_verticalScrollBar.Width));
            var a_hPercent = Height / (float)(a_childrenHeight + (m_horizontalScrollBar.IsHidden ? 0 : m_horizontalScrollBar.Height));

            if (CanScrollV)
            {
                VScrollRequired = a_hPercent >= 1;
            }
            else
            {
                m_verticalScrollBar.IsHidden = true;
            }

            if (CanScrollH)
            {
                HScrollRequired = a_wPercent >= 1;
            }
            else
            {
                m_horizontalScrollBar.IsHidden = true;
            }

            m_verticalScrollBar.ContentSize = m_innerControl.Height;
            m_verticalScrollBar.ViewableContentSize = Height - (m_horizontalScrollBar.IsHidden ? 0 : m_horizontalScrollBar.Height);

            m_horizontalScrollBar.ContentSize = m_innerControl.Width;
            m_horizontalScrollBar.ViewableContentSize = Width - (m_verticalScrollBar.IsHidden ? 0 : m_verticalScrollBar.Width);

            var a_newInnerPanelPosX = 0;
            var a_newInnerPanelPosY = 0;

            if (CanScrollV && !m_verticalScrollBar.IsHidden)
            {
                a_newInnerPanelPosY = (int)(-((m_innerControl.Height) - Height + (m_horizontalScrollBar.IsHidden ? 0 : m_horizontalScrollBar.Height)) * m_verticalScrollBar.ScrollAmount);
            }
            if (CanScrollH && !m_horizontalScrollBar.IsHidden)
            {
                a_newInnerPanelPosX = (int)(-((m_innerControl.Width) - Width + (m_verticalScrollBar.IsHidden ? 0 : m_verticalScrollBar.Width)) * m_horizontalScrollBar.ScrollAmount);
            }

            m_innerControl.SetPosition(a_newInnerPanelPosX, a_newInnerPanelPosY);
        }

        public virtual void ScrollToBottom()
        {
            if (!CanScrollV)
            {
                return;
            }

            UpdateScrollBars();
            m_verticalScrollBar.ScrollToBottom();
        }

        public virtual void ScrollToTop()
        {
            if (CanScrollV)
            {
                UpdateScrollBars();
                m_verticalScrollBar.ScrollToTop();
            }
        }

        public virtual void ScrollToLeft()
        {
            if (CanScrollH)
            {
                UpdateScrollBars();
                m_verticalScrollBar.ScrollToLeft();
            }
        }

        public virtual void ScrollToRight()
        {
            if (CanScrollH)
            {
                UpdateScrollBars();
                m_verticalScrollBar.ScrollToRight();
            }
        }

        public virtual void DeleteAll()
        {
            m_innerControl.DeleteAllChildren();
        }
    }
}