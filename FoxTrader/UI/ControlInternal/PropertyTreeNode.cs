using FoxTrader.UI.Control;

namespace FoxTrader.UI.ControlInternal
{
    /// <summary>Properties node</summary>
    internal class PropertyTreeNode : TreeNode
    {
        /// <summary>Initializes a new instance of the <see cref="PropertyTreeNode" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public PropertyTreeNode(GameControl c_parentControl) : base(c_parentControl)
        {
            m_title.TextColorOverride = Skin.m_colors.m_properties.m_title;
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(Skin c_skin)
        {
            c_skin.DrawPropertyTreeNode(this, m_innerControl.X, m_innerControl.Y);
        }
    }
}