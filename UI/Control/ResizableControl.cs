using System.Drawing;
using FoxTrader.UI.ControlInternal;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control
{
    /// <summary>Base resizable control</summary>
    internal class ResizableControl : GameControl
    {
        private readonly Resizer[] m_resizer;

        /// <summary>Initializes a new instance of the <see cref="ResizableControl" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public ResizableControl(GameControl c_parentControl) : base(c_parentControl)
        {
            m_resizer = new Resizer[10];
            MinimumSize = new Point(5, 5);
            ClampMovement = false;

            m_resizer[2] = new Resizer(this);
            m_resizer[2].Dock = Pos.Bottom;
            m_resizer[2].ResizeDir = Pos.Bottom;
            m_resizer[2].Resized += OnResized;
            m_resizer[2].Target = this;

            m_resizer[1] = new Resizer(m_resizer[2]);
            m_resizer[1].Dock = Pos.Left;
            m_resizer[1].ResizeDir = Pos.Bottom | Pos.Left;
            m_resizer[1].Resized += OnResized;
            m_resizer[1].Target = this;

            m_resizer[3] = new Resizer(m_resizer[2]);
            m_resizer[3].Dock = Pos.Right;
            m_resizer[3].ResizeDir = Pos.Bottom | Pos.Right;
            m_resizer[3].Resized += OnResized;
            m_resizer[3].Target = this;

            m_resizer[8] = new Resizer(this);
            m_resizer[8].Dock = Pos.Top;
            m_resizer[8].ResizeDir = Pos.Top;
            m_resizer[8].Resized += OnResized;
            m_resizer[8].Target = this;

            m_resizer[7] = new Resizer(m_resizer[8]);
            m_resizer[7].Dock = Pos.Left;
            m_resizer[7].ResizeDir = Pos.Left | Pos.Top;
            m_resizer[7].Resized += OnResized;
            m_resizer[7].Target = this;

            m_resizer[9] = new Resizer(m_resizer[8]);
            m_resizer[9].Dock = Pos.Right;
            m_resizer[9].ResizeDir = Pos.Right | Pos.Top;
            m_resizer[9].Resized += OnResized;
            m_resizer[9].Target = this;

            m_resizer[4] = new Resizer(this);
            m_resizer[4].Dock = Pos.Left;
            m_resizer[4].ResizeDir = Pos.Left;
            m_resizer[4].Resized += OnResized;
            m_resizer[4].Target = this;

            m_resizer[6] = new Resizer(this);
            m_resizer[6].Dock = Pos.Right;
            m_resizer[6].ResizeDir = Pos.Right;
            m_resizer[6].Resized += OnResized;
            m_resizer[6].Target = this;
        }

        /// <summary>Determines whether control's position should be restricted to its parent bounds</summary>
        public bool ClampMovement
        {
            get;
            set;
        }

        /// <summary>Invoked when the control has been resized</summary>
        public event SizeEventHandler Resized;

        /// <summary>Handler for the resized event</summary>
        /// <param name="c_control">Event source</param>
        protected virtual void OnResized(GameControl c_control)
        {
            if (Resized != null)
            {
                Resized.Invoke(this);
            }
        }

        protected Resizer GetResizer(int c_i)
        {
            return m_resizer[c_i];
        }

        /// <summary>Disables resizing</summary>
        public void DisableResizing()
        {
            for (var a_i = 0; a_i < 10; a_i++)
            {
                if (m_resizer[a_i] == null)
                {
                    continue;
                }
                m_resizer[a_i].MouseInputEnabled = false;
                m_resizer[a_i].IsHidden = true;
                Padding = new Padding(m_resizer[a_i].Width, m_resizer[a_i].Width, m_resizer[a_i].Width, m_resizer[a_i].Width);
            }
        }

        /// <summary>Enables resizing</summary>
        public void EnableResizing()
        {
            for (var a_i = 0; a_i < 10; a_i++)
            {
                if (m_resizer[a_i] == null)
                {
                    continue;
                }
                m_resizer[a_i].MouseInputEnabled = true;
                m_resizer[a_i].IsHidden = false;
                Padding = new Padding(0, 0, 0, 0); // TODO: check if ok
            }
        }

        /// <summary>Sets the control bounds</summary>
        /// <param name="c_x">X position</param>
        /// <param name="c_y">Y position</param>
        /// <param name="c_width">Width</param>
        /// <param name="c_height">Height</param>
        /// <returns>True if bounds changed</returns>
        public override bool SetBounds(int c_x, int c_y, int c_width, int c_height)
        {
            var a_minSize = MinimumSize;
            // Clamp Minimum Size
            if (c_width < a_minSize.X)
            {
                c_width = a_minSize.X;
            }
            if (c_height < a_minSize.Y)
            {
                c_height = a_minSize.Y;
            }

            // Clamp to parent's window
            var a_parent = Parent;
            if (a_parent != null && ClampMovement)
            {
                if (c_x + c_width > a_parent.Width)
                {
                    c_x = a_parent.Width - c_width;
                }
                if (c_x < 0)
                {
                    c_x = 0;
                }
                if (c_y + c_height > a_parent.Height)
                {
                    c_y = a_parent.Height - c_height;
                }
                if (c_y < 0)
                {
                    c_y = 0;
                }
            }

            return base.SetBounds(c_x, c_y, c_width, c_height);
        }
    }
}