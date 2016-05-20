using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FoxTrader.UI.Font
{
    public class GameFont : IEnumerable<GameFontCharacter>
    {
        public const int kNoMaxWidth = -1;

        private static readonly Dictionary<string, GameFont> m_loadedFonts = new Dictionary<string, GameFont>();
        private readonly string m_fontName;
        private readonly Bitmap m_fontAtlasBitmap;
        private int m_fontTotalCharacters;
        private int m_fontTotalKernings;

        private GameFont(string c_fontName)
        {
            Characters = new Dictionary<char, GameFontCharacter>();
            Kernings = new Dictionary<Tuple<char, char>, GameFontKerning>();
            m_fontName = c_fontName;

            var a_fontByteArray = (byte[])I18N.GetObject(string.Format(CultureInfo.InvariantCulture, "fnt_{0}", m_fontName));

            if (a_fontByteArray == null)
            {
                throw new ApplicationException($"The font {c_fontName} doesn't exist!");
            }

            m_fontAtlasBitmap = (Bitmap)I18N.GetObject(string.Format(CultureInfo.InvariantCulture, "png_gamefont_{0}", m_fontName));

            var a_fontStringLinesArray = Encoding.UTF8.GetString(a_fontByteArray).Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            foreach (var a_fontDataLine in a_fontStringLinesArray)
            {
                if (a_fontDataLine == string.Empty)
                {
                    continue;
                }

                var a_fontDataLineParsed = Regex.Split(a_fontDataLine, @"\s+");

                switch (a_fontDataLineParsed[0])
                {
                    case "info":
                    {
                        ProcessInfo(a_fontDataLineParsed);
                    }
                    break;

                    case "common":
                    {
                        ProcessCommon(a_fontDataLineParsed);
                    }
                    break;

                    case "page":
                    {
                        // We do not support pages yet...if ever
                    }
                    break;

                    case "chars":
                    {
                        var a_infoLineParsed = a_fontDataLineParsed[1].Split('=');

                        if (a_infoLineParsed.Length < 2 || a_infoLineParsed[0] != "count")
                        {
                            throw new ApplicationException($"Font {m_fontName} is corrupt...");
                        }

                        m_fontTotalCharacters = int.Parse(a_infoLineParsed[1]);
                    }
                    break;

                    case "char":
                    {
                        var a_char = new GameFontCharacter(m_fontName, a_fontDataLineParsed, m_fontAtlasBitmap);
                        Characters.Add(a_char.Char, a_char);
                    }
                    break;

                    case "kernings":
                    {
                        var a_infoLineParsed = a_fontDataLineParsed[1].Split('=');

                        if (a_infoLineParsed.Length < 2 || a_infoLineParsed[0] != "count")
                        {
                            throw new ApplicationException($"Font {m_fontName} is corrupt...");
                        }

                        m_fontTotalKernings = int.Parse(a_infoLineParsed[1]);
                    }
                    break;

                    case "kerning":
                    {
                        var a_kerning = new GameFontKerning(m_fontName, a_fontDataLineParsed);
                        var a_kerningKey = Tuple.Create(a_kerning.SecondCharacter, a_kerning.FirstCharacter);
                        Kernings.Add(a_kerningKey, a_kerning);
                    }
                    break;

                    default:
                    {
                        throw new ApplicationException($"Font {c_fontName} is corrupt...");
                    }
                }
            }
        }

        public int AlphaChannel
        {
            get;
            set;
        }

        public bool AntiAliased
        {
            get;
            set;
        }

        public int BaseHeight
        {
            get;
            set;
        }

        public int BlueChannel
        {
            get;
            set;
        }

        public bool Bold
        {
            get;
            set;
        }

        public IDictionary<char, GameFontCharacter> Characters
        {
            get;
            set;
        }

        public string Charset
        {
            get;
            set;
        }

        public string FamilyName
        {
            get;
            set;
        }

        public int FontSize
        {
            get;
            set;
        }

        public int GreenChannel
        {
            get;
            set;
        }

        public bool Italic
        {
            get;
            set;
        }

        public IDictionary<Tuple<char, char>, GameFontKerning> Kernings
        {
            get;
            set;
        }

        public int LineHeight
        {
            get;
            set;
        }

        public int OutlineSize
        {
            get;
            set;
        }

        public bool Packed
        {
            get;
            set;
        }

        public Padding Padding
        {
            get;
            set;
        }

        public Page[] Pages
        {
            get;
            set;
        }

        public int RedChannel
        {
            get;
            set;
        }

        public bool Smoothed
        {
            get;
            set;
        }

        public Point Spacing
        {
            get;
            set;
        }

        public int StretchedHeight
        {
            get;
            set;
        }

        public int SuperSampling
        {
            get;
            set;
        }

        public Size TextureSize
        {
            get;
            set;
        }

        public GameFontCharacter this[char c_character]
        {
            get
            {
                return Characters[c_character];
            }
        }

        public bool Unicode
        {
            get;
            set;
        }

        public IEnumerator<GameFontCharacter> GetEnumerator()
        {
            foreach (var a_pair in Characters)
            {
                yield return a_pair.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int GetKerning(char c_previous, char c_current)
        {
            var a_tupleKey = Tuple.Create(c_previous, c_current);

            if (!Kernings.ContainsKey(a_tupleKey))
            {
                return 0;
            }

            return Kernings[a_tupleKey].Amount;
        }

        public Size MeasureFont(string c_text)
        {
            return MeasureFont(c_text, kNoMaxWidth);
        }

        public Size MeasureFont(string c_text, double c_maxWidth)
        {
            var a_previousCharacter = ' ';
            var a_normalizedText = NormalizeLineBreaks(c_text);
            var a_currentLineWidth = 0;
            var a_currentLineHeight = LineHeight;
            var a_blockWidth = 0;
            var a_lineHeights = new List<int>();

            if (a_normalizedText == string.Empty)
            {
                a_normalizedText = " "; // used to still get text width
            }

            foreach (var a_character in a_normalizedText)
            {
                switch (a_character)
                {
                    case '\n':
                    {
                        a_lineHeights.Add(a_currentLineHeight);
                        a_blockWidth = Math.Max(a_blockWidth, a_currentLineWidth);
                        a_currentLineWidth = 0;
                        a_currentLineHeight = LineHeight;
                    }
                    break;

                    default:
                    {
                        var a_data = this[a_character];
                        var a_width = a_data.XAdvance + GetKerning(a_previousCharacter, a_character);

                        if (c_maxWidth != kNoMaxWidth && a_currentLineWidth + a_width >= c_maxWidth)
                        {
                            a_lineHeights.Add(a_currentLineHeight);
                            a_blockWidth = Math.Max(a_blockWidth, a_currentLineWidth);
                            a_currentLineWidth = 0;
                            a_currentLineHeight = LineHeight;
                        }

                        a_currentLineWidth += a_width;
                        a_currentLineHeight = Math.Max(a_currentLineHeight, a_data.Bounds.Height + a_data.Offset.Y);
                        a_previousCharacter = a_character;
                    }
                    break;
                }
            }

            // finish off the current line if required
            if (a_currentLineHeight != 0)
            {
                a_lineHeights.Add(a_currentLineHeight);
            }

            // reduce any lines other than the last back to the base
            for (var a_i = 0; a_i < a_lineHeights.Count - 1; a_i++)
            {
                a_lineHeights[a_i] = LineHeight;
            }

            // calculate the final block height
            var a_blockHeight = a_lineHeights.Sum();

            return new Size(Math.Max(a_currentLineWidth, a_blockWidth), a_blockHeight);
        }

        public string NormalizeLineBreaks(string c_s)
        {
            return c_s.Replace("\r\n", "\n").Replace("\r", "\n");
        }

        public static GameFont LoadFont(string c_fontName)
        {
            if (m_loadedFonts.ContainsKey(c_fontName))
            {
                return m_loadedFonts[c_fontName];
            }

            var a_newFont = new GameFont(c_fontName);

            m_loadedFonts.Add(c_fontName, a_newFont);

            return a_newFont;
        }

        private void ProcessInfo(IEnumerable<string> c_fontDataLineParsed)
        {
            foreach (var a_infoLine in c_fontDataLineParsed.Skip(1))
            {
                var a_infoLineParsed = a_infoLine.Split('=');

                if (a_infoLineParsed.Length < 2)
                {
                    throw new ApplicationException($"Font {m_fontName} is corrupt...");
                }

                var a_infoKey = a_infoLineParsed[0];
                var a_infoValue = a_infoLineParsed[1].Replace("\"", string.Empty);

                switch (a_infoKey)
                {
                    case "face":
                    {
                        FamilyName = a_infoValue;
                    }
                    break;

                    case "size":
                    {
                        FontSize = int.Parse(a_infoValue);
                    }
                    break;

                    case "bold":
                    {
                        Bold = Convert.ToBoolean(int.Parse(a_infoValue));
                    }
                    break;

                    case "italic":
                    {
                        Italic = Convert.ToBoolean(int.Parse(a_infoValue));
                    }
                    break;

                    case "charset":
                    {
                        Charset = a_infoValue;
                    }
                    break;

                    case "unicode":
                    {
                        Unicode = Convert.ToBoolean(int.Parse(a_infoValue));
                    }
                    break;

                    case "stretchH":
                    {
                        StretchedHeight = int.Parse(a_infoValue);
                    }
                    break;

                    case "smooth":
                    {
                        Smoothed = Convert.ToBoolean(int.Parse(a_infoValue));
                    }
                    break;

                    case "aa":
                    {
                        AntiAliased = Convert.ToBoolean(int.Parse(a_infoValue));
                    }
                    break;

                    case "padding":
                    {
                        var a_pad = a_infoValue.Split(',');
                        Padding = new Padding(int.Parse(a_pad[0]), int.Parse(a_pad[1]), int.Parse(a_pad[2]), int.Parse(a_pad[3]));
                    }
                    break;

                    case "spacing":
                    {
                        var a_spacing = a_infoValue.Split(',');
                        Spacing = new Point(int.Parse(a_spacing[0]), int.Parse(a_spacing[1]));
                    }
                    break;

                    case "outline":
                    {
                        OutlineSize = int.Parse(a_infoValue);
                    }
                    break;

                    default:
                    {
                        throw new ApplicationException($"Font {m_fontName} is corrupt...");
                    }
                }
            }
        }

        private void ProcessCommon(IEnumerable<string> c_fontDataLineParsed)
        {
            foreach (var a_infoLine in c_fontDataLineParsed.Skip(1))
            {
                var a_infoLineParsed = a_infoLine.Split('=');

                if (a_infoLineParsed.Length < 2)
                {
                    throw new ApplicationException($"Font {m_fontName} is corrupt...");
                }

                var a_infoKey = a_infoLineParsed[0];
                var a_infoValue = a_infoLineParsed[1].Replace("\"", string.Empty);

                switch (a_infoKey)
                {
                    case "lineHeight":
                    {
                        LineHeight = int.Parse(a_infoValue);
                    }
                    break;

                    case "base":
                    {
                        BaseHeight = int.Parse(a_infoValue);
                    }
                    break;

                    case "scaleW":
                    {
                        TextureSize = new Size(int.Parse(a_infoValue), TextureSize.Height);
                    }
                    break;

                    case "scaleH":
                    {
                        TextureSize = new Size(TextureSize.Width, int.Parse(a_infoValue));
                    }
                    break;

                    case "pages":
                    {
                        var a_numberOfTexturePages = int.Parse(a_infoValue);

                        if (a_numberOfTexturePages != 1)
                        {
                            throw new ApplicationException($"Font {m_fontName} is corrupt...");
                        }
                    }
                    break;

                    case "packed":
                    {
                        Packed = Convert.ToBoolean(int.Parse(a_infoValue));
                    }
                    break;

                    case "alphaChnl":
                    {
                        AlphaChannel = int.Parse(a_infoValue);
                    }
                    break;

                    case "redChnl":
                    {
                        RedChannel = int.Parse(a_infoValue);
                    }
                    break;

                    case "greenChnl":
                    {
                        GreenChannel = int.Parse(a_infoValue);
                    }
                    break;

                    case "blueChnl":
                    {
                        BlueChannel = int.Parse(a_infoValue);
                    }
                    break;
                }
            }
        }

        internal IList<GameFontCharacter> GetCharacterList(string c_string)
        {
            return c_string.Select(a_textCharacter => Characters[a_textCharacter]).ToList();
        }
    }
}