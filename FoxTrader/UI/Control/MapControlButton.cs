using System.Drawing;
using FoxTrader.Interfaces;

namespace FoxTrader.UI.Control
{
    class MapControlButton : GameControl
    {
        public IMapObject MapObject
        {
            get;
        }

        public MapControlButton(GameControl c_parentControl, IMapObject c_mapObject) : base(c_parentControl)
        {
            MapObject = c_mapObject;

            // Text = MapObject.Name;
            X = MapObject.Position.X;
            Y = MapObject.Position.Y;
            UserData = MapObject.Index;

            // TextColor = Color.White;
            // AutoSizeToContents = true;
        }

        protected override void Render(Skin c_skin)
        {
            var a_renderer = c_skin.Renderer;

            var a_size = 12;

            var a_baseX = 2;
            var a_baseY = Height + 3;

            a_renderer.EndClip();
            a_renderer.DrawColor = Color.White;
            a_renderer.DrawLinedRect(new Rectangle(new Point(a_baseX - (a_size / 2), a_baseY - (a_size / 2)), new Size(a_size - 1, a_size - 1)));
            a_renderer.DrawLine(a_baseX, a_baseY, a_baseX + 15, a_baseY - 15);
            a_renderer.StartClip();
        }

        public override void UpdateColors()
        {
            // Do Nothing
        }
    }
}
