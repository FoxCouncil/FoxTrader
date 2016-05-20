using System;
using System.Collections.Generic;
using System.Drawing;

namespace FoxTrader.UI.Font
{
    static class GameFontLoader
    {
        public static GameFont LoadFontFromTextFile(string fileName)
        {
            /*IDictionary<int, Page> pageData;
            IDictionary<Kerning, int> kerningDictionary;
            IDictionary<char, Character> charDictionary;
            string resourcePath;
            string[] lines;

            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName", "File name not specified");
            else if (!File.Exists(fileName))
                throw new FileNotFoundException(string.Format("Cannot find file '{0}'", fileName), fileName);

            pageData = new SortedDictionary<int, Page>();
            kerningDictionary = new Dictionary<Kerning, int>();
            charDictionary = new Dictionary<char, Character>();
            // font = new GameFont();

            resourcePath = Path.GetDirectoryName(fileName);
            lines = File.ReadAllLines(fileName);

            foreach (string line in lines)
            {
                string[] parts;

                parts = Split(line, ' ');

                if (parts.Length != 0)
                {
                    switch (parts[0])
                    {
                        case "info":
                        font.FamilyName = GetNamedString(parts, "face");
                        font.FontSize = GetNamedInt(parts, "size");
                        font.Bold = GetNamedBool(parts, "bold");
                        font.Italic = GetNamedBool(parts, "italic");
                        font.Charset = GetNamedString(parts, "charset");
                        font.Unicode = GetNamedBool(parts, "unicode");
                        font.StretchedHeight = GetNamedInt(parts, "stretchH");
                        font.Smoothed = GetNamedBool(parts, "smooth");
                        font.SuperSampling = GetNamedInt(parts, "aa");
                        font.Padding = ParsePadding(GetNamedString(parts, "padding"));
                        font.Spacing = ParsePoint(GetNamedString(parts, "spacing"));
                        font.OutlineSize = GetNamedInt(parts, "outline");
                        break;
                        case "common":
                        font.LineHeight = GetNamedInt(parts, "lineHeight");
                        font.BaseHeight = GetNamedInt(parts, "base");
                        font.TextureSize = new Size(GetNamedInt(parts, "scaleW"), GetNamedInt(parts, "scaleH"));
                        font.Packed = GetNamedBool(parts, "packed");
                        font.AlphaChannel = GetNamedInt(parts, "alphaChnl");
                        font.RedChannel = GetNamedInt(parts, "redChnl");
                        font.GreenChannel = GetNamedInt(parts, "greenChnl");
                        font.BlueChannel = GetNamedInt(parts, "blueChnl");
                        break;
                        case "page":
                        int id;
                        string name;
                        string textureId;

                        id = GetNamedInt(parts, "id");
                        name = GetNamedString(parts, "file");
                        textureId = Path.GetFileNameWithoutExtension(name);

                        pageData.Add(id, new Page(id, Path.Combine(resourcePath, name)));
                        break;
                        case "char":
                        Character charData;

                        charData = new Character { Char = (char)GetNamedInt(parts, "id"), Bounds = new Rectangle(GetNamedInt(parts, "x"), GetNamedInt(parts, "y"), GetNamedInt(parts, "width"), GetNamedInt(parts, "height")), Offset = new Point(GetNamedInt(parts, "xoffset"), GetNamedInt(parts, "yoffset")), XAdvance = GetNamedInt(parts, "xadvance"), TexturePage = GetNamedInt(parts, "page"), Channel = GetNamedInt(parts, "chnl") };
                        charDictionary.Add(charData.Char, charData);
                        break;
                        case "kerning":
                        Kerning key;

                        key = new Kerning((char)GetNamedInt(parts, "first"), (char)GetNamedInt(parts, "second"), GetNamedInt(parts, "amount"));

                        if (!kerningDictionary.ContainsKey(key))
                            kerningDictionary.Add(key, key.Amount);
                        break;
                    }
                }
            }

            font.Pages = ToArray(pageData.Values);
            font.Characters = charDictionary;
            font.Kernings = kerningDictionary;*/

            return null;
        }

        /// <summary>
        /// Loads a bitmap font from an XML file.
        /// </summary>
        /// <param name="fileName">Name of the file to load.</param>
        /// <returns></returns>
        public static GameFont LoadFontFromXmlFile(string fileName)
        {
            return null;
        }

        /// <summary>
        /// Returns a boolean from an array of name/value pairs.
        /// </summary>
        /// <param name="parts">The array of parts.</param>
        /// <param name="name">The name of the value to return.</param>
        /// <returns></returns>
        private static bool GetNamedBool(string[] parts, string name)
        {
            return GetNamedInt(parts, name) != 0;
        }

        /// <summary>
        /// Returns an integer from an array of name/value pairs.
        /// </summary>
        /// <param name="parts">The array of parts.</param>
        /// <param name="name">The name of the value to return.</param>
        /// <returns></returns>
        private static int GetNamedInt(string[] parts, string name)
        {
            return Convert.ToInt32(GetNamedString(parts, name));
        }

        /// <summary>
        /// Returns a string from an array of name/value pairs.
        /// </summary>
        /// <param name="parts">The array of parts.</param>
        /// <param name="name">The name of the value to return.</param>
        /// <returns></returns>
        private static string GetNamedString(string[] parts, string name)
        {
            string result;

            result = string.Empty;
            name = name.ToLowerInvariant();

            foreach (string part in parts)
            {
                int nameEndIndex;

                nameEndIndex = part.IndexOf("=");
                if (nameEndIndex != -1)
                {
                    string namePart;
                    string valuePart;

                    namePart = part.Substring(0, nameEndIndex).ToLowerInvariant();
                    valuePart = part.Substring(nameEndIndex + 1);

                    if (namePart == name)
                    {
                        if (valuePart.StartsWith("\"") && valuePart.EndsWith("\""))
                            valuePart = valuePart.Substring(1, valuePart.Length - 2);

                        result = valuePart;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Creates a Padding object from a string representation
        /// </summary>
        /// <param name="s">The string.</param>
        /// <returns></returns>
        private static Padding ParsePadding(string s)
        {
            string[] parts;

            parts = s.Split(',');

            return new Padding() { Left = Convert.ToInt32(parts[3].Trim()), Top = Convert.ToInt32(parts[0].Trim()), Right = Convert.ToInt32(parts[1].Trim()), Bottom = Convert.ToInt32(parts[2].Trim()) };
        }

        /// <summary>
        /// Creates a Point object from a string representation
        /// </summary>
        /// <param name="s">The string.</param>
        /// <returns></returns>
        private static Point ParsePoint(string s)
        {
            string[] parts;

            parts = s.Split(',');

            return new Point() { X = Convert.ToInt32(parts[0].Trim()), Y = Convert.ToInt32(parts[1].Trim()) };
        }

        /// <summary>
        /// Splits the specified string using a given delimiter, ignoring any instances of the delimiter as part of a quoted string.
        /// </summary>
        /// <param name="s">The string to split.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns></returns>
        private static string[] Split(string s, char delimiter)
        {
            string[] results;

            if (s.Contains("\""))
            {
                List<string> parts;
                int partStart;

                partStart = -1;
                parts = new List<string>();

                do
                {
                    int partEnd;
                    int quoteStart;
                    int quoteEnd;
                    bool hasQuotes;

                    quoteStart = s.IndexOf("\"", partStart + 1);
                    quoteEnd = s.IndexOf("\"", quoteStart + 1);
                    partEnd = s.IndexOf(delimiter, partStart + 1);

                    if (partEnd == -1)
                        partEnd = s.Length;

                    hasQuotes = quoteStart != -1 && partEnd > quoteStart && partEnd < quoteEnd;
                    if (hasQuotes)
                        partEnd = s.IndexOf(delimiter, quoteEnd + 1);

                    parts.Add(s.Substring(partStart + 1, partEnd - partStart - 1));

                    if (hasQuotes)
                        partStart = partEnd - 1;

                    partStart = s.IndexOf(delimiter, partStart + 1);
                } while (partStart != -1);

                results = parts.ToArray();
            }
            else
                results = s.Split(new char[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);

            return results;
        }

        /// <summary>
        /// Converts the given collection into an array
        /// </summary>
        /// <typeparam name="T">Type of the items in the array</typeparam>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        private static T[] ToArray<T>(ICollection<T> values)
        {
            T[] result;

            // avoid a forced .NET 3 dependency just for one call to Linq

            result = new T[values.Count];
            values.CopyTo(result, 0);

            return result;
        }

    }
}