//   !!  // FoxTrader - Label.cs
// *.-". // Created: 25-04-2016 [8:41 PM]
//  | |  // ʇɟǝʃʎdoƆ 2016 FoxCouncil 

#region Usings

using System;
using System.Drawing;
using FoxTrader.UI.Font;
using static FoxTrader.Constants;

#endregion

namespace FoxTrader.UI.Control
{
    /// <summary>Static text label</summary>
    internal class Label : GameControl
    {
        private Pos m_align;
        private bool m_autoSizeToContents;
        private GameFont m_font;
        private string m_text = string.Empty;
        private Padding m_textPadding;

        /// <summary>Initializes a new instance of the <see cref="Label" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public Label(GameControl c_parentControl) : base(c_parentControl)
        {
            MouseInputEnabled = false;
            Alignment = Pos.Left | Pos.Top;
            AutoSizeToContents = false;
            TextOffset = Point.Empty;
            TextPadding = Padding.kZero;
            TextColor = Color.Black;
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
                return TextOverride != string.Empty ? TextOverride : m_text;
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
                return m_font ?? Skin.DefaultFont;
            }
            set
            {
                m_font = value;

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
            get;
            set;
        }

        /// <summary>Override text color (used by tooltips)</summary>
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
        } = string.Empty;

        public Point TextOffset
        {
            get;
            set;
        }

        public Size TextSize => Font.MeasureFont(Text);

        /// <summary>Text length (in characters)</summary>
        public int TextLength => m_text.Length;

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

        protected void RenderText(Renderer c_renderer)
        {
            var a_charList = Font.GetCharacterList(Text);

            var a_xCoordinate = TextOffset.X;
            var a_yCoordinate = TextOffset.Y;
            var a_previousChar = ' ';

            foreach (var a_char in a_charList)
            {
                var a_charTexture = a_char.CharTexture;

                var a_kerning = Font.GetKerning(a_previousChar, a_char.Char);

                c_renderer.DrawColor = TextColorOverride != Color.Empty ? TextColorOverride : TextColor;
                c_renderer.DrawTexturedRect(a_charTexture, new Rectangle(a_xCoordinate + a_char.Offset.X + a_kerning, a_yCoordinate + a_char.Offset.Y, a_charTexture.Width, a_charTexture.Height));

                a_xCoordinate += a_char.XAdvance + a_kerning;

                a_previousChar = a_char.Char;
            }
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

            CalculateTextOffset();
        }

        protected void CalculateTextOffset()
        {
            var a_align = m_align;

            if (m_autoSizeToContents)
            {
                SizeToContents();
            }

            var a_x = m_textPadding.Left + Padding.Left;
            var a_y = m_textPadding.Top + Padding.Top;

            if (0 != (a_align & Pos.Right))
            {
                a_x = Width - TextSize.Width - m_textPadding.Right - Padding.Right;
            }
            if (0 != (a_align & Pos.CenterH))
            {
                a_x = (int)((m_textPadding.Left + Padding.Left) + ((Width - TextSize.Width - m_textPadding.Left - Padding.Left - m_textPadding.Right - Padding.Right) * 0.5f));
            }

            if (0 != (a_align & Pos.CenterV))
            {
                a_y = (int)((m_textPadding.Top + Padding.Top) + ((Height - TextSize.Height) * 0.5f) - m_textPadding.Bottom - Padding.Bottom);
            }
            if (0 != (a_align & Pos.Bottom))
            {
                a_y = Height - TextSize.Height - m_textPadding.Bottom - Padding.Bottom;
            }

            TextOffset = new Point(a_x, a_y);
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

            m_text = c_str;

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
            SetSize(TextSize.Width + Padding.Left + Padding.Right + TextPadding.Left + TextPadding.Right, TextSize.Height + Padding.Top + Padding.Bottom + TextPadding.Top + TextPadding.Bottom);
            InvalidateParent();
        }

        /// <summary>Gets the coordinates of specified character in the text</summary>
        /// <param name="c_idx">Character index</param>
        /// <returns>Character position in local coordinates</returns>
        public Point GetCharacterPosition(int c_idx)
        {
            if (Text.Length == 0 || c_idx == 0)
            {
                return new Point(0, 0);
            }

            if (c_idx >= Text.Length)
            {
            }

            var a_sub = Text.Substring(0, c_idx);
            var a_p = new Point(Skin.Renderer.MeasureText(Font, a_sub));

            return a_p;
        }

        /// <summary>Searches for a character closest to given point</summary>
        /// <param name="c_point">Point</param>
        /// <returns>Character index</returns>
        public int GetClosestCharacter(Point c_point)
        {
            var a_distance = kMaxUIControlSize;
            var a_c = 0;

            for (var a_i = 0; a_i < Text.Length + 1; a_i++)
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

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(Skin c_skin)
        {
            RenderText(c_skin.Renderer);
        }
    }
}