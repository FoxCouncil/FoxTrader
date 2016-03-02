using System.Drawing;
using System.Linq;
using FoxTrader.UI.ControlInternal;
using FoxTrader.UI.Skin;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control
{
    /// <summary>Movable window with title bar</summary>
    internal class WindowControl : ResizableControl
    {
        private readonly Label m_caption;
        private readonly CloseButton m_closeButton;
        private readonly Dragger m_titleBar;
        private Modal m_modal;

        /// <summary>Initializes a new instance of the <see cref="WindowControl" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        /// <param name="c_caption">Window caption</param>
        /// <param name="c_modal">Determines whether the window should be modal</param>
        public WindowControl(GameControl c_parentControl, string c_caption = "", bool c_modal = false) : base(c_parentControl)
        {
            m_titleBar = new Dragger(this);
            m_titleBar.Height = 24;
            m_titleBar.Padding = Padding.m_zero;
            m_titleBar.Margin = new Margin(0, 0, 0, 4);
            m_titleBar.Target = this;
            m_titleBar.Dock = Pos.Top;

            m_caption = new Label(m_titleBar);
            m_caption.Alignment = Pos.Left | Pos.CenterV;
            m_caption.Text = c_caption;
            m_caption.Dock = Pos.Fill;
            m_caption.Padding = new Padding(8, 0, 0, 0);
            m_caption.TextColor = Skin.m_colors.m_window.m_titleInactive;

            m_closeButton = new CloseButton(m_titleBar, this);
            //m_CloseButton.Text = String.Empty;
            m_closeButton.SetSize(24, 24);
            m_closeButton.Dock = Pos.Right;
            m_closeButton.Clicked += CloseButtonPressed;
            m_closeButton.IsTabable = false;
            m_closeButton.Name = "closeButton";

            //Create a blank content control, dock it to the top - Should this be a ScrollControl?
            m_innerControl = new GameControl(this);
            m_innerControl.Dock = Pos.Fill;
            GetResizer(8).Hide();
            BringToFront();
            IsTabable = false;
            Focus();
            MinimumSize = new Point(100, 40);
            ClampMovement = true;
            KeyboardInputEnabled = false;

            if (c_modal)
            {
                MakeModal();
            }
        }

        /// <summary>Window caption</summary>
        public string Caption
        {
            get
            {
                return m_caption.Text;
            }
            set
            {
                m_caption.Text = value;
            }
        }

        /// <summary>Determines whether the window has close button</summary>
        public bool IsClosable
        {
            get
            {
                return !m_closeButton.IsHidden;
            }
            set
            {
                m_closeButton.IsHidden = !value;
            }
        }

        /// <summary>Determines whether the control should be disposed on close</summary>
        public bool DeleteOnClose
        {
            get;
            set;
        }

        /// <summary>Indicates whether the control is hidden</summary>
        public override bool IsHidden
        {
            get
            {
                return base.IsHidden;
            }
            set
            {
                if (!value)
                {
                    BringToFront();
                }
                base.IsHidden = value;
            }
        }

        /// <summary>Indicates whether the control is on top of its parent's children</summary>
        public override bool IsOnTop
        {
            get
            {
                return Parent.Children.Where(c_x => c_x is WindowControl).Last() == this;
            }
        }

        protected virtual void CloseButtonPressed(GameControl c_control)
        {
            IsHidden = true;

            if (m_modal != null)
            {
                m_modal.DelayedDelete();
                m_modal = null;
            }

            if (DeleteOnClose)
            {
                Parent.RemoveChild(this, true);
            }
        }

        /// <summary>Makes the window modal: covers the whole canvas and gets all input</summary>
        /// <param name="c_dim">Determines whether all the background should be dimmed</param>
        public void MakeModal(bool c_dim = false)
        {
            if (m_modal != null)
            {
                return;
            }

            m_modal = new Modal(GetCanvas());
            Parent = m_modal;

            if (c_dim)
            {
                m_modal.ShouldDrawBackground = true;
            }
            else
            {
                m_modal.ShouldDrawBackground = false;
            }
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(SkinBase c_skin)
        {
            var a_hasFocus = IsOnTop;

            if (a_hasFocus)
            {
                m_caption.TextColor = Skin.m_colors.m_window.m_titleActive;
            }
            else
            {
                m_caption.TextColor = Skin.m_colors.m_window.m_titleInactive;
            }

            c_skin.DrawWindow(this, m_titleBar.Bottom, a_hasFocus);
        }

        /// <summary>Renders under the actual control (shadows etc)</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void RenderUnder(SkinBase c_skin)
        {
            base.RenderUnder(c_skin);
            c_skin.DrawShadow(this);
        }

        public override void Touch()
        {
            base.Touch();
            BringToFront();
        }

        /// <summary>Renders the focus overlay</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void RenderFocus(SkinBase c_skin)
        {
        }
    }
}