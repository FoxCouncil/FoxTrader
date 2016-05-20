using System;
using System.Collections.Generic;
using System.Linq;

namespace FoxTrader.UI.Font
{
    public class GameFontKerning
    {
        private readonly string m_fontName;

        public GameFontKerning(string c_fontName, IEnumerable<string> c_fontDataLineParsed)
        {
            m_fontName = c_fontName;

            foreach (var a_infoLine in c_fontDataLineParsed.Skip(1))
            {
                if (a_infoLine == string.Empty)
                {
                    continue;
                }

                var a_charLineParsed = a_infoLine.Split('=');

                if (a_charLineParsed.Length < 2)
                {
                    throw new ApplicationException($"Font {m_fontName} is corrupt...");
                }

                var a_charKey = a_charLineParsed[0];
                var a_charValue = a_charLineParsed[1].Replace("\"", string.Empty);

                switch (a_charKey)
                {
                    case "first":
                    {
                        FirstCharacter = (char)int.Parse(a_charValue);
                    }
                    break;

                    case "second":
                    {
                        SecondCharacter = (char)int.Parse(a_charValue);
                    }
                    break;

                    case "amount":
                    {
                        Amount = int.Parse(a_charValue);
                    }
                    break;

                    default:
                    {
                        throw new ApplicationException($"Font {c_fontName} is corrupt...");
                    }
                }
            }
        }

        public char FirstCharacter
        {
            get;
            set;
        }

        public char SecondCharacter
        {
            get;
            set;
        }

        public int Amount
        {
            get;
            set;
        }

        public override string ToString()
        {
            return $"{FirstCharacter} to {SecondCharacter} = {Amount}";
        }
    }
}