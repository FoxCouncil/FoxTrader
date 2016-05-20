using System.IO;

namespace FoxTrader.UI.Font
{
    public struct Padding
    {
        public Padding(int c_left, int c_top, int c_right, int c_bottom) : this()
        {
            Top = c_top;
            Left = c_left;
            Right = c_right;
            Bottom = c_bottom;
        }

        public override string ToString()
        {
            return $"{Left}, {Top}, {Right}, {Bottom}";
        }

        public int Top
        {
            get;
            set;
        }

        public int Left
        {
            get;
            set;
        }

        public int Right
        {
            get;
            set;
        }

        public int Bottom
        {
            get;
            set;
        }
    }

    public struct Page
    {
        public Page(int c_id, string c_fileName) : this()
        {
            FileName = c_fileName;
            Id = c_id;
        }

        public override string ToString()
        {
            return $"{Id} ({Path.GetFileName(FileName)})";
        }

        public string FileName
        {
            get;
            set;
        }

        public int Id
        {
            get;
            set;
        }
    }
}