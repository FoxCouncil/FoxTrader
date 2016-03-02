using System.Linq;
using FoxTrader.UI.ControlInternal;
using FoxTrader.UI.Skin;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control
{
    /// <summary>Tree control node</summary>
    internal class TreeNode : GameControl
    {
        public const int kTreeIndentation = 14;
        private bool m_selected;
        protected Button m_title;
        protected Button m_toggleButton;

        protected TreeControl m_treeControl;

        /// <summary>Initializes a new instance of the <see cref="TreeNode" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public TreeNode(GameControl c_parentControl) : base(c_parentControl)
        {
            m_toggleButton = new TreeToggleButton(this);
            m_toggleButton.SetBounds(0, 0, 15, 15);
            m_toggleButton.Toggled += OnToggleButtonPress;

            m_title = new TreeNodeLabel(this);
            m_title.Dock = Pos.Top;
            m_title.Margin = new Margin(16, 0, 0, 0);
            m_title.DoubleClickedLeft += OnDoubleClickName;
            m_title.Clicked += OnClickName;

            m_innerControl = new GameControl(this) { Dock = Pos.Top, Height = 100, Margin = new Margin(kTreeIndentation, 1, 0, 0) };
            m_innerControl.Hide();

            IsRoot = false;
            m_selected = false;
            IsSelectable = true;
        }

        /// <summary>Indicates whether this is a root node</summary>
        public bool IsRoot
        {
            get;
            set;
        }

        /// <summary>Parent tree control</summary>
        public TreeControl TreeControl
        {
            get
            {
                return m_treeControl;
            }
            set
            {
                m_treeControl = value;
            }
        }

        /// <summary>Determines whether the node is selectable</summary>
        public bool IsSelectable
        {
            get;
            set;
        }

        /// <summary>Indicates whether the node is selected</summary>
        public bool IsSelected
        {
            get
            {
                return m_selected;
            }
            set
            {
                if (!IsSelectable)
                {
                    return;
                }
                if (IsSelected == value)
                {
                    return;
                }

                m_selected = value;

                if (m_title != null)
                {
                    m_title.ToggleState = value;
                }

                if (SelectionChanged != null)
                {
                    SelectionChanged.Invoke(this);
                }

                // propagate to root parent (tree)
                if (m_treeControl != null && m_treeControl.SelectionChanged != null)
                {
                    m_treeControl.SelectionChanged.Invoke(this);
                }

                if (value)
                {
                    if (Selected != null)
                    {
                        Selected.Invoke(this);
                    }

                    if (m_treeControl != null && m_treeControl.Selected != null)
                    {
                        m_treeControl.Selected.Invoke(this);
                    }
                }
                else
                {
                    if (Unselected != null)
                    {
                        Unselected.Invoke(this);
                    }

                    if (m_treeControl != null && m_treeControl.Unselected != null)
                    {
                        m_treeControl.Unselected.Invoke(this);
                    }
                }
            }
        }

        /// <summary>Node's label</summary>
        public string Text
        {
            get
            {
                return m_title.Text;
            }
            set
            {
                m_title.Text = value;
            }
        }

        /// <summary>Invoked when the node label has been pressed</summary>
        public event ButtonEventHandler LabelPressed;

        /// <summary>Invoked when the node's selected state has changed</summary>
        public event SelectionEventHandler SelectionChanged;

        /// <summary>Invoked when the node has been selected</summary>
        public event SelectionEventHandler Selected;

        /// <summary>Invoked when the node has been unselected</summary>
        public event SelectionEventHandler Unselected;

        /// <summary>Invoked when the node has been expanded</summary>
        public event CollapsibleNodeEventHandler Expanded;

        /// <summary>Invoked when the node has been collapsed</summary>
        public event CollapsibleNodeEventHandler Collapsed;

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(SkinBase c_skin)
        {
            var a_bottom = 0;
            if (m_innerControl.Children.Count > 0)
            {
                a_bottom = m_innerControl.Children.Last().Y + m_innerControl.Y;
            }

            c_skin.DrawTreeNode(this, m_innerControl.IsVisible, IsSelected, m_title.Height, m_title.TextRight, (int)(m_toggleButton.Y + m_toggleButton.Height * 0.5f), a_bottom, m_treeControl == Parent); // IsRoot
        }

        /// <summary>Lays out the control's interior according to alignment, padding, dock etc</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Layout(SkinBase c_skin)
        {
            if (m_toggleButton != null)
            {
                if (m_title != null)
                {
                    m_toggleButton.SetPosition(0, (m_title.Height - m_toggleButton.Height) * 0.5f);
                }

                if (m_innerControl.Children.Count == 0)
                {
                    m_toggleButton.Hide();
                    m_toggleButton.ToggleState = false;
                    m_innerControl.Hide();
                }
                else
                {
                    m_toggleButton.Show();
                    m_innerControl.SizeToChildren(false, true);
                }
            }

            base.Layout(c_skin);
        }

        /// <summary>Function invoked after layout</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void PostLayout(SkinBase c_skin)
        {
            if (SizeToChildren(false, true))
            {
                InvalidateParent();
            }
        }

        /// <summary>Adds a new child node</summary>
        /// <param name="c_label">Node's label</param>
        /// <returns>Newly created control</returns>
        public TreeNode AddNode(string c_label)
        {
            var a_node = new TreeNode(this);
            a_node.Text = c_label;
            a_node.Dock = Pos.Top;
            a_node.IsRoot = this is TreeControl;
            a_node.TreeControl = m_treeControl;

            if (m_treeControl != null)
            {
                m_treeControl.OnNodeAdded(a_node);
            }

            return a_node;
        }

        /// <summary>Opens the node</summary>
        public void Open()
        {
            m_innerControl.Show();
            if (m_toggleButton != null)
            {
                m_toggleButton.ToggleState = true;
            }

            if (Expanded != null)
            {
                Expanded.Invoke(this);
            }
            if (m_treeControl != null && m_treeControl.Expanded != null)
            {
                m_treeControl.Expanded.Invoke(this);
            }

            Invalidate();
        }

        /// <summary>Closes the node</summary>
        public void Close()
        {
            m_innerControl.Hide();
            if (m_toggleButton != null)
            {
                m_toggleButton.ToggleState = false;
            }

            if (Collapsed != null)
            {
                Collapsed.Invoke(this);
            }
            if (m_treeControl != null && m_treeControl.Collapsed != null)
            {
                m_treeControl.Collapsed.Invoke(this);
            }

            Invalidate();
        }

        /// <summary>Opens the node and all child nodes</summary>
        public void ExpandAll()
        {
            Open();
            foreach (var a_child in Children)
            {
                var a_node = a_child as TreeNode;
                if (a_node == null)
                {
                    continue;
                }
                a_node.ExpandAll();
            }
        }

        /// <summary>Clears the selection on the node and all child nodes</summary>
        public void UnselectAll()
        {
            IsSelected = false;
            if (m_title != null)
            {
                m_title.ToggleState = false;
            }

            foreach (var a_child in Children)
            {
                var a_node = a_child as TreeNode;
                if (a_node == null)
                {
                    continue;
                }
                a_node.UnselectAll();
            }
        }

        /// <summary>Handler for the toggle button</summary>
        /// <param name="c_control">Event source</param>
        protected virtual void OnToggleButtonPress(GameControl c_control)
        {
            if (m_toggleButton.ToggleState)
            {
                Open();
            }
            else
            {
                Close();
            }
        }

        /// <summary>Handler for label double click</summary>
        /// <param name="c_control">Event source</param>
        protected virtual void OnDoubleClickName(GameControl c_control)
        {
            if (!m_toggleButton.IsVisible)
            {
                return;
            }
            m_toggleButton.Toggle();
        }

        /// <summary>Handler for label click</summary>
        /// <param name="c_control">Event source</param>
        protected virtual void OnClickName(GameControl c_control)
        {
            if (LabelPressed != null)
            {
                LabelPressed.Invoke(this);
            }
            IsSelected = !IsSelected;
        }
    }
}