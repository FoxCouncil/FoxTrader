using System.Drawing;
using FoxTrader.UI.Control;

namespace FoxTrader.UI.DragDrop
{
    internal class Package
    {
        public GameControl m_drawControl;
        public Point m_holdOffset;
        public bool m_isDraggable;
        public string m_name;
        public object m_userData;
    }
}