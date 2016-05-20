using System;
using System.Collections.Generic;
using System.Drawing;
using FoxTrader.UI.Font;

namespace FoxTrader.UI.Control
{
    /// <summary>Multiline label with text chunks having different color/font</summary>
    internal class RichLabel : GameControl
    {
        private readonly string[] m_newline;
        private readonly List<TextBlock> m_textBlocks;

        private bool m_needsRebuild;

        /// <summary>Initializes a new instance of the <see cref="RichLabel" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public RichLabel(GameControl c_parentControl) : base(c_parentControl)
        {
            m_newline = new[] { Environment.NewLine };
            m_textBlocks = new List<TextBlock>();
        }

        /// <summary>Adds a line break to the control</summary>
        public void AddLineBreak()
        {
            var a_block = new TextBlock { m_type = BlockType.NewLine };
            m_textBlocks.Add(a_block);
        }

        /// <summary>Adds text to the control</summary>
        /// <param name="c_text">Text to add</param>
        /// <param name="c_color">Text color</param>
        /// <param name="c_font">Font to use</param>
        public void AddText(string c_text, Color c_color, GameFont c_font = null)
        {
            if (string.IsNullOrEmpty(c_text))
            {
                return;
            }

            var a_lines = c_text.Split(m_newline, StringSplitOptions.None);
            for (var a_i = 0; a_i < a_lines.Length; a_i++)
            {
                if (a_i > 0)
                {
                    AddLineBreak();
                }

                var a_block = new TextBlock { m_type = BlockType.Text, m_text = a_lines[a_i], m_color = c_color, m_font = c_font };

                m_textBlocks.Add(a_block);
                m_needsRebuild = true;
                Invalidate();
            }
        }

        /// <summary>Resizes the control to fit its children</summary>
        /// <param name="c_width">Determines whether to change control's width</param>
        /// <param name="c_height">Determines whether to change control's height</param>
        /// <returns>True if bounds changed</returns>
        public override bool SizeToChildren(bool c_width = true, bool c_height = true)
        {
            Rebuild();
            return base.SizeToChildren(c_width, c_height);
        }

        protected void SplitLabel(string c_text, GameFont c_font, TextBlock c_block, ref int c_x, ref int c_y, ref int c_lineHeight)
        {
            var a_spaced = Util.SplitAndKeep(c_text, " ");
            if (a_spaced.Length == 0)
            {
                return;
            }

            var a_spaceLeft = Width - c_x;
            string a_leftOver;

            // Does the whole word fit in?
            var a_stringSize = Skin.Renderer.MeasureText(c_font, c_text);
            if (a_spaceLeft > a_stringSize.Width)
            {
                CreateLabel(c_text, c_block, ref c_x, ref c_y, ref c_lineHeight, true);
                return;
            }

            // If the first word is bigger than the line, just give up.
            var a_wordSize = Skin.Renderer.MeasureText(c_font, a_spaced[0]);
            if (a_wordSize.Width >= a_spaceLeft)
            {
                CreateLabel(a_spaced[0], c_block, ref c_x, ref c_y, ref c_lineHeight, true);
                if (a_spaced[0].Length >= c_text.Length)
                {
                    return;
                }

                a_leftOver = c_text.Substring(a_spaced[0].Length + 1);
                SplitLabel(a_leftOver, c_font, c_block, ref c_x, ref c_y, ref c_lineHeight);
                return;
            }

            var a_newString = string.Empty;
            for (var a_i = 0; a_i < a_spaced.Length; a_i++)
            {
                a_wordSize = Skin.Renderer.MeasureText(c_font, a_newString + a_spaced[a_i]);
                if (a_wordSize.Width > a_spaceLeft)
                {
                    CreateLabel(a_newString, c_block, ref c_x, ref c_y, ref c_lineHeight, true);
                    c_x = 0;
                    c_y += c_lineHeight;
                    break;
                }

                a_newString += a_spaced[a_i];
            }

            var a_newstrLen = a_newString.Length;
            if (a_newstrLen < c_text.Length)
            {
                a_leftOver = c_text.Substring(a_newstrLen + 1);
                SplitLabel(a_leftOver, c_font, c_block, ref c_x, ref c_y, ref c_lineHeight);
            }
        }

        protected void CreateLabel(string c_text, TextBlock c_block, ref int c_x, ref int c_y, ref int c_lineHeight, bool c_noSplit)
        {
            // Use default font or is one set?
            var a_font = Skin.DefaultFont;
            if (c_block.m_font != null)
            {
                a_font = c_block.m_font;
            }

            // This string is too long for us, split it up.
            var a_p = Skin.Renderer.MeasureText(a_font, c_text);

            if (c_lineHeight == -1)
            {
                c_lineHeight = a_p.Height;
            }

            if (!c_noSplit)
            {
                if (c_x + a_p.Width > Width)
                {
                    SplitLabel(c_text, a_font, c_block, ref c_x, ref c_y, ref c_lineHeight);
                    return;
                }
            }

            // Wrap
            if (c_x + a_p.Width >= Width)
            {
                CreateNewline(ref c_x, ref c_y, c_lineHeight);
            }

            var a_label = new Label(this);
            a_label.SetText(c_x == 0 ? c_text.TrimStart(' ') : c_text);
            a_label.TextColor = c_block.m_color;
            a_label.Font = a_font;
            a_label.SizeToContents();
            a_label.SetPosition(c_x, c_y);

            //lineheight = (lineheight + pLabel.Height()) / 2;			

            c_x += a_label.Width;

            if (c_x >= Width)
            {
                CreateNewline(ref c_x, ref c_y, c_lineHeight);
            }
        }

        protected void CreateNewline(ref int c_x, ref int c_y, int c_lineHeight)
        {
            c_x = 0;
            c_y += c_lineHeight;
        }

        protected void Rebuild()
        {
            DeleteAllChildren();

            var a_x = 0;
            var a_y = 0;
            var a_lineHeight = -1;

            foreach (var a_block in m_textBlocks)
            {
                if (a_block.m_type == BlockType.NewLine)
                {
                    CreateNewline(ref a_x, ref a_y, a_lineHeight);
                    continue;
                }

                if (a_block.m_type == BlockType.Text)
                {
                    CreateLabel(a_block.m_text, a_block, ref a_x, ref a_y, ref a_lineHeight, false);
                }
            }

            m_needsRebuild = false;
        }

        /// <summary>Handler invoked when control's bounds change</summary>
        /// <param name="c_oldBounds">Old bounds</param>
        protected override void OnBoundsChanged(Rectangle c_oldBounds)
        {
            base.OnBoundsChanged(c_oldBounds);
            Rebuild();
        }

        /// <summary>Lays out the control's interior according to alignment, padding, dock etc</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void OnLayout(Skin c_skin)
        {
            base.OnLayout(c_skin);
            if (m_needsRebuild)
            {
                Rebuild();
            }

            // align bottoms. this is still not ideal, need to take font metrics into account.
            GameControl a_prev = null;
            foreach (var a_child in Children)
            {
                if (a_prev != null && a_child.Y == a_prev.Y)
                {
                    Align.PlaceRightBottom(a_child, a_prev);
                }
                a_prev = a_child;
            }
        }

        protected struct TextBlock
        {
            public BlockType m_type;
            public string m_text;
            public Color m_color;
            public GameFont m_font;
        }

        protected enum BlockType
        {
            Text,
            NewLine
        }
    }
}