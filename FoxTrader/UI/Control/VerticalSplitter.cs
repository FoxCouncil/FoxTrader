using System.Windows.Forms;
using FoxTrader.UI.ControlInternal;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control
{
    internal class VerticalSplitter : GameControl
    {
        private readonly SplitterBar m_hSplitter;
        private readonly GameControl[] m_sections;

        private float m_hVal; // 0-1
        private int m_zoomedSection; // 0-3

        /// <summary>Initializes a new instance of the <see cref="CrossSplitter" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public VerticalSplitter(GameControl c_parentControl) : base(c_parentControl)
        {
            m_sections = new GameControl[2];

            m_hSplitter = new SplitterBar(this);
            m_hSplitter.SetPosition(128, 0);
            m_hSplitter.Dragged += OnHorizontalMoved;
            m_hSplitter.Cursor = Cursors.SizeWE;

            m_hVal = 0.5f;

            SetPanel(0, null);
            SetPanel(1, null);

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
                return m_hSplitter.ShouldDrawBackground;
            }
            set
            {
                m_hSplitter.ShouldDrawBackground = value;
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
            m_hVal = 0.5f;
            Invalidate();
        }

        public void SetHValue(float c_value)
        {
            if (c_value <= 1f || c_value >= 0)
            {
                m_hVal = c_value;
            }
        }

        private void UpdateHSplitter()
        {
            m_hSplitter.MoveTo((Width - m_hSplitter.Width) * (m_hVal), m_hSplitter.Y);
        }

        protected void OnHorizontalMoved(GameControl c_control)
        {
            m_hVal = CalculateValueHorizontal();
            Invalidate();
        }

        private float CalculateValueHorizontal()
        {
            return m_hSplitter.X / (float)(Width - m_hSplitter.Width);
        }

        /// <summary>Lays out the control's interior according to alignment, padding, dock etc</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void OnLayout(Skin c_skin)
        {
            m_hSplitter.SetSize(SplitterSize, Height);

            UpdateHSplitter();

            if (m_zoomedSection == -1)
            {
                if (m_sections[0] != null)
                {
                    m_sections[0].SetBounds(0, 0, m_hSplitter.X, Height);
                }

                if (m_sections[1] != null)
                {
                    m_sections[1].SetBounds(m_hSplitter.X + SplitterSize, 0, Width - (m_hSplitter.X + SplitterSize), Height);
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
                for (var a_i = 0; a_i < 2; a_i++)
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

            for (var a_i = 0; a_i < 2; a_i++)
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