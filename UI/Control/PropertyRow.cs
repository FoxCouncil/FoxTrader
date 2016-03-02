using FoxTrader.UI.Control.Property;
using FoxTrader.UI.ControlInternal;
using FoxTrader.UI.Skin;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control
{
    /// <summary>Single property row</summary>
    internal class PropertyRow : GameControl
    {
        private readonly Label m_label;
        private readonly PropertyBase m_property;
        private bool m_lastEditing;
        private bool m_lastHover;

        /// <summary>Initializes a new instance of the <see cref="PropertyRow" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        /// <param name="c_prop">Property control associated with this row</param>
        public PropertyRow(GameControl c_parentControl, PropertyBase c_prop) : base(c_parentControl)
        {
            var a_label = new PropertyRowLabel(this);
            a_label.Dock = Pos.Left;
            a_label.Alignment = Pos.Left | Pos.Top;
            a_label.Margin = new Margin(2, 2, 0, 0);
            m_label = a_label;

            m_property = c_prop;
            m_property.Parent = this;
            m_property.Dock = Pos.Fill;
            m_property.ValueChanged += OnValueChanged;
        }

        /// <summary>Indicates whether the property value is being edited</summary>
        public bool IsEditing => m_property != null && m_property.IsEditing;

        /// <summary>Property value</summary>
        public string Value
        {
            get
            {
                return m_property.Value;
            }
            set
            {
                m_property.Value = value;
            }
        }

        /// <summary>Indicates whether the control is hovered by mouse pointer</summary>
        public override bool IsHovered => base.IsHovered || (m_property != null && m_property.IsHovered);

        /// <summary>Property name</summary>
        public string Label
        {
            get
            {
                return m_label.Text;
            }
            set
            {
                m_label.Text = value;
            }
        }

        /// <summary>Invoked when the property value has changed</summary>
        public event ValueEventHandler ValueChanged;

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(SkinBase c_skin)
        {
            /* SORRY */
            if (IsEditing != m_lastEditing)
            {
                OnEditingChanged();
                m_lastEditing = IsEditing;
            }

            if (IsHovered != m_lastHover)
            {
                OnHoverChanged();
                m_lastHover = IsHovered;
            }
            /* SORRY */

            c_skin.DrawPropertyRow(this, m_label.Right, IsEditing, IsHovered | m_property.IsHovered);
        }

        /// <summary>Lays out the control's interior according to alignment, padding, dock etc</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Layout(SkinBase c_skin)
        {
            var a_parent = Parent as Properties;
            if (null == a_parent)
            {
                return;
            }

            m_label.Width = a_parent.SplitWidth;

            if (m_property != null)
            {
                Height = m_property.Height;
            }
        }

        protected virtual void OnValueChanged(GameControl c_control)
        {
            if (ValueChanged != null)
            {
                ValueChanged.Invoke(this);
            }
        }

        private void OnEditingChanged()
        {
            m_label.Redraw();
        }

        private void OnHoverChanged()
        {
            m_label.Redraw();
        }
    }
}