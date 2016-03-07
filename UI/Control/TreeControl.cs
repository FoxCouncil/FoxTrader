using System.Drawing;
using FoxTrader.UI.Skin;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control
{
    /// <summary>Tree control</summary>
    internal class TreeControl : TreeNode
    {
        private readonly ScrollControl m_scrollControl;

        /// <summary>Initializes a new instance of the <see cref="TreeControl" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public TreeControl(GameControl c_parentControl) : base(c_parentControl)
        {
            m_treeControl = this;

            RemoveChild(m_toggleButton, true);
            m_toggleButton = null;
            RemoveChild(m_title, true);
            m_title = null;
            RemoveChild(m_innerControl, true);
            m_innerControl = null;

            AllowMultiSelect = false;

            m_scrollControl = new ScrollControl(this);
            m_scrollControl.Dock = Pos.Fill;
            m_scrollControl.EnableScroll(false, true);
            m_scrollControl.AutoHideBars = true;
            m_scrollControl.Margin = Margin.kOne;

            m_innerControl = m_scrollControl;

            m_scrollControl.SetInnerSize(1000, 1000); // TODO: why such arbitrary numbers?
        }

        /// <summary>Determines if multiple nodes can be selected at the same time</summary>
        public bool AllowMultiSelect
        {
            get;
            set;
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(SkinBase c_skin)
        {
            if (ShouldDrawBackground)
            {
                c_skin.DrawTreeControl(this);
            }
        }

        /// <summary>Handler invoked when control children's bounds change</summary>
        /// <param name="c_oldChildBounds"></param>
        /// <param name="c_child"></param>
        protected override void OnChildBoundsChanged(Rectangle c_oldChildBounds, GameControl c_child)
        {
            if (m_scrollControl != null)
            {
                m_scrollControl.UpdateScrollBars();
            }
        }

        /// <summary>Removes all child nodes</summary>
        public virtual void RemoveAll()
        {
            m_scrollControl.DeleteAll();
        }

        /// <summary>Handler for node added event</summary>
        /// <param name="c_node">Node added</param>
        public virtual void OnNodeAdded(TreeNode c_node)
        {
            c_node.LabelPressed += OnNodeSelected;
        }

        /// <summary>Handler for node selected event</summary>
        /// <param name="c_control">Node selected</param>
        protected virtual void OnNodeSelected(GameControl c_control)
        {
            if (!AllowMultiSelect /*|| InputHandler.InputHandler.IsKeyDown(Key.Control)*/)
            {
                UnselectAll();
            }
        }
    }
}