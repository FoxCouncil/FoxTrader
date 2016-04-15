using FoxTrader.UI.ControlInternal;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control
{
    /// <summary>Property table/tree</summary>
    internal class PropertyTree : TreeControl
    {
        /// <summary>Initializes a new instance of the <see cref="PropertyTree" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public PropertyTree(GameControl c_parentControl) : base(c_parentControl)
        {
        }

        /// <summary>Adds a new properties node</summary>
        /// <param name="c_label">Node label</param>
        /// <returns>Newly created control</returns>
        public Properties Add(string c_label)
        {
            TreeNode a_node = new PropertyTreeNode(this);
            a_node.Text = c_label;
            a_node.Dock = Pos.Top;

            var a_props = new Properties(a_node);
            a_props.Dock = Pos.Top;

            return a_props;
        }
    }
}