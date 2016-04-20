using FoxTrader.UI.DragDrop;

namespace FoxTrader.UI.Control
{
    /// <summary>Titlebar for DockedTabControl</summary>
    internal class TabTitleBar : Label
    {
        public TabTitleBar(GameControl c_parentControl) : base(c_parentControl)
        {
            MouseInputEnabled = true;
            TextPadding = new Padding(5, 2, 5, 2);
            Padding = new Padding(1, 2, 1, 2);

            DragAndDrop_SetPackage(true, "TabWindowMove");
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(Skin c_skin)
        {
            c_skin.DrawTabTitleBar(this);
        }

        public override void DragAndDrop_StartDragging(Package c_package, int c_x, int c_y)
        {
            DragAndDrop.m_sourceControl = Parent;
            DragAndDrop.m_sourceControl.DragAndDrop_StartDragging(c_package, c_x, c_y);
        }

        public void UpdateFromTab(TabButton c_button)
        {
            Text = c_button.Text;
            SizeToContents();
        }
    }
}