using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace FoxTrader.UI.Font
{
    public class GameFontCharacter
    {
        private readonly string m_fontName;

        public GameFontCharacter(string c_fontName, IEnumerable<string> c_fontDataLineParsed, Bitmap c_fontAtlasBitmap)
        {
            m_fontName = c_fontName;

            var a_x = 0;
            var a_y = 0;
            var a_width = 0;
            var a_height = 0;
            var a_xOffset = 0;
            var a_yOffset = 0;

            foreach (var a_infoLine in c_fontDataLineParsed.Skip(1))
            {
                var a_charLineParsed = a_infoLine.Split('=');

                if (a_charLineParsed.Length < 2)
                {
                    throw new ApplicationException($"Font {m_fontName} is corrupt...");
                }

                var a_charKey = a_charLineParsed[0];
                var a_charValue = a_charLineParsed[1].Replace("\"", string.Empty);

                switch (a_charKey)
                {
                    case "id":
                    {
                        Char = (char)int.Parse(a_charValue);
                    }
                    break;

                    case "x":
                    {
                        a_x = int.Parse(a_charValue);
                    }
                    break;

                    case "y":
                    {
                        a_y = int.Parse(a_charValue);
                    }
                    break;

                    case "width":
                    {
                        a_width = int.Parse(a_charValue);
                    }
                    break;

                    case "height":
                    {
                        a_height = int.Parse(a_charValue);
                    }
                    break;

                    case "xoffset":
                    {
                        a_xOffset = int.Parse(a_charValue);
                    }
                    break;

                    case "yoffset":
                    {
                        a_yOffset = int.Parse(a_charValue);
                    }
                    break;

                    case "xadvance":
                    {
                        XAdvance = int.Parse(a_charValue);
                    }
                    break;

                    case "page":
                    {
                        TexturePage = int.Parse(a_charValue);
                    }
                    break;

                    case "chnl":
                    {
                        Channel = int.Parse(a_charValue);
                    }
                    break;

                    default:
                    {
                        throw new ApplicationException($"Font {c_fontName} is corrupt...");
                    }
                }
            }

            Bounds = new Rectangle(a_x, a_y, a_width, a_height);
            Offset = new Point(a_xOffset, a_yOffset);

            CharTexture = new Texture(c_fontAtlasBitmap.Clone(Bounds, c_fontAtlasBitmap.PixelFormat));
        }

        public override string ToString()
        {
            return Char.ToString();
        }

        public int Channel
        {
            get;
            set;
        }

        public Rectangle Bounds
        {
            get;
            set;
        }

        public Point Offset
        {
            get;
            set;
        }

        public char Char
        {
            get;
            set;
        }

        public int TexturePage
        {
            get;
            set;
        }

        public int XAdvance
        {
            get;
            set;
        }

        public Texture CharTexture
        {
            get;
            private set;
        }
    }
}
