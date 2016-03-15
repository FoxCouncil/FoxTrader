using System.Drawing;
using FoxTrader.UI.Skin;

namespace FoxTrader.UI.Control
{
    class MapControl : GameControl
    {
        public Constants.MapZoomState ZoomState
        {
            get;
        } = Constants.MapZoomState.Universe;

        public MapControl(GameControl c_controlParent) : base(c_controlParent)
        {
            MinimumSize = new Point(200, 200);
        }

        internal override void DoRender(SkinBase c_skin)
        {
            base.DoRender(c_skin);

            c_skin.Renderer.DrawLine(10, 10, 100, 1);
        }
    }
}
