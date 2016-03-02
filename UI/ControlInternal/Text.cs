using System;
using System.Drawing;
using FoxTrader.UI.Control;
using FoxTrader.UI.Skin;

namespace FoxTrader.UI.ControlInternal
{
    /// <summary>Displays text. Always sized to contents</summary>
    internal class Text : GameControl
    {
        private GameFont m_baseFont;
        private string m_baseString;

        /// <summary>Initializes a new instance of the <see cref="Text" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public Text(GameControl c_parentControl) : base(c_parentControl)
        {
            m_baseFont = Skin.DefaultFont;
            m_baseString = string.Empty;
            TextColor = Skin.m_colors.m_label.m_default;
            MouseInputEnabled = false;
            TextColorOverride = Color.FromArgb(0, 255, 255, 255); // A==0, override disabled
        }

        /// <summary>Font used to display the text</summary>
        /// <remarks>The font is not being disposed by this class</remarks>
        public GameFont Font
        {
            get
            {
                return m_baseFont;
            }
            set
            {
                m_baseFont = value;
                SizeToContents();
            }
        }

        /// <summary>Text to display</summary>
        public string String
        {
            get
            {
                return m_baseString;
            }
            set
            {
                m_baseString = value;
                SizeToContents();
            }
        }

        /// <summary>Text color</summary>
        public Color TextColor
        {
            get;
            set;
        }

        /// <summary>Text length in characters</summary>
        public int Length
        {
            get
            {
                if (m_baseString == null)
                {
                    return 0;
                }

                return m_baseString.Length;
            }
        }

        /// <summary>Text color override - used by tooltips</summary>
        public Color TextColorOverride
        {
            get;
            set;
        }

        /// <summary>Text override - used to display different string</summary>
        public string TextOverride
        {
            get;
            set;
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(SkinBase c_skin)
        {
            if (Length == 0 || Font == null)
            {
                return;
            }

            if (TextColorOverride.A == 0)
            {
                c_skin.Renderer.DrawColor = TextColor;
            }
            else
            {
                c_skin.Renderer.DrawColor = TextColorOverride;
            }

            c_skin.Renderer.RenderText(Font, Point.Empty, TextOverride ?? String);

#if DEBUG_TEXT_MEASURE
            {
                Point lastPos = Point.Empty;

                for (int i = 0; i < m_String.Length + 1; i++)
                {
                    String sub = (TextOverride ?? String).Substring(0, i);
                    Point p = Skin.Renderer.MeasureText(Font, sub);

                    Rectangle rect = new Rectangle();
                    rect.Location = lastPos;
                    rect.Size = new Size(p.X - lastPos.X, p.Y);
                    skin.Renderer.DrawColor = Color.FromArgb(64, 0, 0, 0);
                    skin.Renderer.DrawLinedRect(rect);

                    lastPos = new Point(rect.Right, 0);
                }
            }
#endif
        }

        /// <summary>Lays out the control's interior according to alignment, padding, dock etc</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Layout(SkinBase c_skin)
        {
            SizeToContents();
            base.Layout(c_skin);
        }

        /// <summary>Handler invoked when control's scale changes</summary>
        protected override void OnScaleChanged()
        {
            Invalidate();
        }

        /// <summary>Sizes the control to its contents</summary>
        public void SizeToContents()
        {
            if (String == null)
            {
                return;
            }

            if (Font == null)
            {
                throw new InvalidOperationException("Text.SizeToContents() - No Font!!\n");
            }

            var a_p = new Point(1, Font.Size);

            if (Length > 0)
            {
                a_p = Skin.Renderer.MeasureText(Font, TextOverride ?? String);
            }

            if (a_p.X == Width && a_p.Y == Height)
            {
                return;
            }

            SetSize(a_p.X, a_p.Y);
            Invalidate();
            InvalidateParent();
        }

        /// <summary>Gets the coordinates of specified character in the text</summary>
        /// <param name="c_idx">Character index</param>
        /// <returns>Character position in local coordinates</returns>
        public Point GetCharacterPosition(int c_idx)
        {
            if (Length == 0 || c_idx == 0)
            {
                return new Point(0, 0);
            }

            var a_sub = (TextOverride ?? String).Substring(0, c_idx);
            var a_p = Skin.Renderer.MeasureText(Font, a_sub);

            return a_p;
        }

        /// <summary>Searches for a character closest to given point</summary>
        /// <param name="c_point">Point</param>
        /// <returns>Character index</returns>
        public int GetClosestCharacter(Point c_point)
        {
            var a_distance = Constants.kMaxUIControlSize;
            var a_c = 0;

            for (var a_i = 0; a_i < String.Length + 1; a_i++)
            {
                var a_cp = GetCharacterPosition(a_i);
                var a_dist = Math.Abs(a_cp.X - c_point.X); // TODO: handle multiline

                if (a_dist > a_distance)
                {
                    continue;
                }

                a_distance = a_dist;
                a_c = a_i;
            }

            return a_c;
        }
    }
}