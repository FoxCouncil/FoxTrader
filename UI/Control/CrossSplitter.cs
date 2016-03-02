using System.Windows.Forms;
using FoxTrader.UI.ControlInternal;
using FoxTrader.UI.Skin;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control
{
    /// <summary>Splitter control</summary>
    internal class CrossSplitter : GameControl
    {
        private readonly SplitterBar m_centerSplitter;
        private readonly SplitterBar m_horizontalSplitter;

        private readonly GameControl[] m_sections;
        private readonly SplitterBar m_verticalSplitter;

        private float m_horizontalValue; // 0-1
        private float m_verticalValue; // 0-1

        private int m_zoomedSection; // 0-3

        /// <summary>Initializes a new instance of the <see cref="CrossSplitter" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public CrossSplitter(GameControl c_parentControl) : base(c_parentControl)
        {
            m_sections = new GameControl[4];

            m_verticalSplitter = new SplitterBar(this);
            m_verticalSplitter.SetPosition(0, 128);
            m_verticalSplitter.Dragged += OnVerticalMoved;
            m_verticalSplitter.Cursor = Cursors.SizeNS;

            m_horizontalSplitter = new SplitterBar(this);
            m_horizontalSplitter.SetPosition(128, 0);
            m_horizontalSplitter.Dragged += OnHorizontalMoved;
            m_horizontalSplitter.Cursor = Cursors.SizeWE;

            m_centerSplitter = new SplitterBar(this);
            m_centerSplitter.SetPosition(128, 128);
            m_centerSplitter.Dragged += OnCenterMoved;
            m_centerSplitter.Cursor = Cursors.SizeAll;

            m_horizontalValue = 0.5f;
            m_verticalValue = 0.5f;

            SetPanel(0, null);
            SetPanel(1, null);
            SetPanel(2, null);
            SetPanel(3, null);

            SplitterSize = 5;
            SplittersVisible = false;

            m_zoomedSection = -1;
        }

        /// <summary>Indicates whether any of the panels is zoomed</summary>
        public bool IsZoomed => m_zoomedSection != -1;

        /// <summary>Gets or sets a value indicating whether splitters should be visible</summary>
        public bool SplittersVisible
        {
            get
            {
                return m_centerSplitter.ShouldDrawBackground;
            }
            set
            {
                m_centerSplitter.ShouldDrawBackground = value;
                m_verticalSplitter.ShouldDrawBackground = value;
                m_horizontalSplitter.ShouldDrawBackground = value;
            }
        }

        /// <summary>Gets or sets the size of the splitter</summary>
        public int SplitterSize
        {
            get;
            set;
        }

        /// <summary>Invoked when one of the panels has been zoomed (maximized)</summary>
        public event ZoomEventHandler PanelZoomed;

        /// <summary>Invoked when one of the panels has been unzoomed (restored)</summary>
        public event ZoomEventHandler PanelUnZoomed;

        /// <summary>Invoked when the zoomed panel has been changed</summary>
        public event ZoomEventHandler ZoomChanged;

        /// <summary>Centers the panels so that they take even amount of space</summary>
        public void CenterPanels()
        {
            m_horizontalValue = 0.5f;
            m_verticalValue = 0.5f;
            Invalidate();
        }

        private void UpdateVSplitter()
        {
            m_verticalSplitter.MoveTo(m_verticalSplitter.X, (Height - m_verticalSplitter.Height) * (m_verticalValue));
        }

        private void UpdateHSplitter()
        {
            m_horizontalSplitter.MoveTo((Width - m_horizontalSplitter.Width) * (m_horizontalValue), m_horizontalSplitter.Y);
        }

        private void UpdateCSplitter()
        {
            m_centerSplitter.MoveTo((Width - m_centerSplitter.Width) * (m_horizontalValue), (Height - m_centerSplitter.Height) * (m_verticalValue));
        }

        protected void OnCenterMoved(GameControl c_control)
        {
            CalculateValueCenter();
            Invalidate();
        }

        protected void OnVerticalMoved(GameControl c_control)
        {
            m_verticalValue = CalculateValueVertical();
            Invalidate();
        }

        protected void OnHorizontalMoved(GameControl c_control)
        {
            m_horizontalValue = CalculateValueHorizontal();
            Invalidate();
        }

        private void CalculateValueCenter()
        {
            m_horizontalValue = m_centerSplitter.X / (float)(Width - m_centerSplitter.Width);
            m_verticalValue = m_centerSplitter.Y / (float)(Height - m_centerSplitter.Height);
        }

        private float CalculateValueVertical()
        {
            return m_verticalSplitter.Y / (float)(Height - m_verticalSplitter.Height);
        }

        private float CalculateValueHorizontal()
        {
            return m_horizontalSplitter.X / (float)(Width - m_horizontalSplitter.Width);
        }

        /// <summary>Lays out the control's interior according to alignment, padding, dock etc</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Layout(SkinBase c_skin)
        {
            m_verticalSplitter.SetSize(Width, SplitterSize);
            m_horizontalSplitter.SetSize(SplitterSize, Height);
            m_centerSplitter.SetSize(SplitterSize, SplitterSize);

            UpdateVSplitter();
            UpdateHSplitter();
            UpdateCSplitter();

            if (m_zoomedSection == -1)
            {
                if (m_sections[0] != null)
                {
                    m_sections[0].SetBounds(0, 0, m_horizontalSplitter.X, m_verticalSplitter.Y);
                }

                if (m_sections[1] != null)
                {
                    m_sections[1].SetBounds(m_horizontalSplitter.X + SplitterSize, 0, Width - (m_horizontalSplitter.X + SplitterSize), m_verticalSplitter.Y);
                }

                if (m_sections[2] != null)
                {
                    m_sections[2].SetBounds(0, m_verticalSplitter.Y + SplitterSize, m_horizontalSplitter.X, Height - (m_verticalSplitter.Y + SplitterSize));
                }

                if (m_sections[3] != null)
                {
                    m_sections[3].SetBounds(m_horizontalSplitter.X + SplitterSize, m_verticalSplitter.Y + SplitterSize, Width - (m_horizontalSplitter.X + SplitterSize), Height - (m_verticalSplitter.Y + SplitterSize));
                }
            }
            else
            {
                //This should probably use Fill docking instead
                m_sections[m_zoomedSection].SetBounds(0, 0, Width, Height);
            }
        }

        /// <summary>Assigns a control to the specific inner section</summary>
        /// <param name="c_index">Section index (0-3)</param>
        /// <param name="c_panel">Control to assign</param>
        public void SetPanel(int c_index, GameControl c_panel)
        {
            m_sections[c_index] = c_panel;

            if (c_panel != null)
            {
                c_panel.Dock = Pos.None;
                c_panel.Parent = this;
            }

            Invalidate();
        }

        /// <summary>Gets the specific inner section</summary>
        /// <param name="c_index">Section index (0-3)</param>
        /// <returns>Specified section</returns>
        internal GameControl GetPanel(int c_index)
        {
            return m_sections[c_index];
        }

        /// <summary>Internal handler for the zoom changed event</summary>
        protected void OnZoomChanged()
        {
            if (ZoomChanged != null)
            {
                ZoomChanged.Invoke(this);
            }

            if (m_zoomedSection == -1)
            {
                if (PanelUnZoomed != null)
                {
                    PanelUnZoomed.Invoke(this);
                }
            }
            else
            {
                if (PanelZoomed != null)
                {
                    PanelZoomed.Invoke(this);
                }
            }
        }

        /// <summary>Maximizes the specified panel so it fills the entire control</summary>
        /// <param name="c_section">Panel index (0-3)</param>
        public void Zoom(int c_section)
        {
            UnZoom();

            if (m_sections[c_section] != null)
            {
                for (var a_i = 0; a_i < 4; a_i++)
                {
                    if (a_i != c_section && m_sections[a_i] != null)
                    {
                        m_sections[a_i].IsHidden = true;
                    }
                }

                m_zoomedSection = c_section;

                Invalidate();
            }

            OnZoomChanged();
        }

        /// <summary>Restores the control so all panels are visible</summary>
        public void UnZoom()
        {
            m_zoomedSection = -1;

            for (var a_i = 0; a_i < 4; a_i++)
            {
                if (m_sections[a_i] != null)
                {
                    m_sections[a_i].IsHidden = false;
                }
            }

            Invalidate();
            OnZoomChanged();
        }
    }
}