using FoxTrader.UI.Skin;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control
{
    /// <summary>Menu strip</summary>
    internal class MenuStrip : Menu
    {
        /// <summary>Initializes a new instance of the <see cref="MenuStrip" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public MenuStrip(GameControl c_parentControl) : base(c_parentControl)
        {
            SetBounds(0, 0, 200, 22);
            Dock = Pos.Top;
            m_innerControl.Padding = new Padding(5, 0, 0, 0);
        }

        /// <summary>Determines whether the menu should open on mouse hover</summary>
        protected override bool ShouldHoverOpenMenu => IsMenuOpen();

        /// <summary>Closes the current menu</summary>
        public override void Close()
        {
        }

        /// <summary>Renders under the actual control (shadows etc)</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void RenderUnder(SkinBase c_skin)
        {
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(SkinBase c_skin)
        {
            c_skin.DrawMenuStrip(this);
        }

        /// <summary>Lays out the control's interior according to alignment, padding, dock etc</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void OnLayout(SkinBase c_skin)
        {
            //TODO: We don't want to do vertical sizing the same as Menu, do nothing for now
        }

        /// <summary>Add item handler</summary>
        /// <param name="c_item">Item added</param>
        protected override void OnAddItem(MenuItem c_item)
        {
            c_item.Dock = Pos.Left;
            c_item.TextPadding = new Padding(5, 0, 5, 0);
            c_item.Padding = new Padding(10, 0, 10, 0);
            c_item.SizeToContents();
            c_item.IsOnStrip = true;
            c_item.MouseOver += OnHoverItem;
        }
    }
}