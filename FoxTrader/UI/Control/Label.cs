using System.Drawing;
using FoxTrader.UI.ControlInternal;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control
{
    /// <summary>Static text label</summary>
    internal class Label : GameControl
    {
        private readonly Text m_text;
        private Pos m_align;
        private bool m_autoSizeToContents;
        private Padding m_textPadding;

        /// <summary>Initializes a new instance of the <see cref="Label" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public Label(GameControl c_parentControl) : base(c_parentControl)
        {
            m_text = new Text(this);

            MouseInputEnabled = false;
            Alignment = Pos.Left | Pos.Top;
            AutoSizeToContents = false;
        }

        /// <summary>Text alignment</summary>
        public Pos Alignment
        {
            get
            {
                return m_align;
            }
            set
            {
                m_align = value;
                Invalidate();
            }
        }

        /// <summary>Text</summary>
        public string Text
        {
            get
            {
                return m_text.String;
            }
            set
            {
                SetText(value);
            }
        }

        /// <summary>Font</summary>
        public GameFont Font
        {
            get
            {
                return m_text.Font;
            }
            set
            {
                m_text.Font = value;

                if (m_autoSizeToContents)
                {
                    SizeToContents();
                }

                Invalidate();
            }
        }

        /// <summary>Text color</summary>
        public Color TextColor
        {
            get
            {
                return m_text.TextColor;
            }
            set
            {
                m_text.TextColor = value;
            }
        }

        /// <summary>Override text color (used by tooltips)</summary>
        public Color TextColorOverride
        {
            get
            {
                return m_text.TextColorOverride;
            }
            set
            {
                m_text.TextColorOverride = value;
            }
        }

        /// <summary>Text override - used to display different string</summary>
        public string TextOverride
        {
            get
            {
                return m_text.TextOverride;
            }
            set
            {
                m_text.TextOverride = value;
            }
        }

        /// <summary>Width of the text (in pixels)</summary>
        public int TextWidth => m_text.Width;

        /// <summary>Height of the text (in pixels)</summary>
        public int TextHeight => m_text.Height;

        public int TextX => m_text.X;
        public int TextY => m_text.Y;

        /// <summary>Text length (in characters)</summary>
        public int TextLength => m_text.Length;

        public int TextRight => m_text.Right;

        /// <summary>Determines if the control should autosize to its text</summary>
        public bool AutoSizeToContents
        {
            get
            {
                return m_autoSizeToContents;
            }
            set
            {
                m_autoSizeToContents = value;
                Invalidate();
                InvalidateParent();
            }
        }

        /// <summary>Text padding</summary>
        public Padding TextPadding
        {
            get
            {
                return m_textPadding;
            }
            set
            {
                m_textPadding = value;
                Invalidate();
                InvalidateParent();
            }
        }

        /// <summary>Returns index of the character closest to specified point (in canvas coordinates)</summary>
        /// <param name="c_x"></param>
        /// <param name="c_y"></param>
        /// <returns></returns>
        protected int GetClosestCharacter(Point c_point)
        {
            return m_text.GetClosestCharacter(m_text.CanvasPosToLocal(c_point));
        }

        /// <summary>Sets the position of the internal text control</summary>
        /// <param name="c_x"></param>
        /// <param name="c_y"></param>
        protected void SetTextPosition(int c_x, int c_y)
        {
            m_text.SetPosition(c_x, c_y);
        }

        /// <summary>Handler for text changed event</summary>
        protected virtual void OnTextChanged()
        {
        }

        /// <summary>Lays out the control's interior according to alignment, padding, dock etc</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void OnLayout(Skin c_skin)
        {
            base.OnLayout(c_skin);

            var a_align = m_align;

            if (m_autoSizeToContents)
            {
                SizeToContents();
            }

            var a_x = m_textPadding.m_left + Padding.m_left;
            var a_y = m_textPadding.m_top + Padding.m_top;

            if (0 != (a_align & Pos.Right))
            {
                a_x = Width - m_text.Width - m_textPadding.m_right - Padding.m_right;
            }
            if (0 != (a_align & Pos.CenterH))
            {
                a_x = (int)((m_textPadding.m_left + Padding.m_left) + ((Width - m_text.Width - m_textPadding.m_left - Padding.m_left - m_textPadding.m_right - Padding.m_right) * 0.5f));
            }

            if (0 != (a_align & Pos.CenterV))
            {
                a_y = (int)((m_textPadding.m_top + Padding.m_top) + ((Height - m_text.Height) * 0.5f) - m_textPadding.m_bottom - Padding.m_bottom);
            }
            if (0 != (a_align & Pos.Bottom))
            {
                a_y = Height - m_text.Height - m_textPadding.m_bottom - Padding.m_bottom;
            }

            m_text.SetPosition(a_x, a_y);
        }

        /// <summary>Sets the label text</summary>
        /// <param name="c_str">Text to set</param>
        /// <param name="c_doEvents">Determines whether to invoke "text changed" event</param>
        public virtual void SetText(string c_str, bool c_doEvents = true)
        {
            if (Text == c_str)
            {
                return;
            }

            m_text.String = c_str;

            if (m_autoSizeToContents)
            {
                SizeToContents();
            }

            Invalidate();
            InvalidateParent();

            if (c_doEvents)
            {
                OnTextChanged();
            }
        }

        public virtual void SizeToContents()
        {
            m_text.SetPosition(m_textPadding.m_left + Padding.m_left, m_textPadding.m_top + Padding.m_top);
            m_text.SizeToContents();

            SetSize(m_text.Width + Padding.m_left + Padding.m_right + m_textPadding.m_left + m_textPadding.m_right, m_text.Height + Padding.m_top + Padding.m_bottom + m_textPadding.m_top + m_textPadding.m_bottom);
            InvalidateParent();
        }

        /// <summary>Gets the coordinates of specified character</summary>
        /// <param name="c_index">Character index</param>
        /// <returns>Character coordinates (local)</returns>
        public virtual Point GetCharacterPosition(int c_index)
        {
            var a_p = m_text.GetCharacterPosition(c_index);
            return new Point(a_p.X + m_text.X, a_p.Y + m_text.Y);
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(Skin c_skin)
        {
        }
    }
}