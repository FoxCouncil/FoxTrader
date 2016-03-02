namespace FoxTrader.UI.Control.Property
{
    /// <summary>Base control for property entry</summary>
    internal class PropertyBase : GameControl
    {
        /// <summary>Initializes a new instance of the <see cref="PropertyBase" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public PropertyBase(GameControl c_parentControl) : base(c_parentControl)
        {
            Height = 17;
        }

        /// <summary>Property value (todo: always string, which is ugly. do something about it)</summary>
        public virtual string Value
        {
            get
            {
                return null;
            }
            set
            {
                SetValue(value, false);
            }
        }

        /// <summary>Indicates whether the property value is being edited</summary>
        public virtual bool IsEditing => false;

        /// <summary>Invoked when the property value has been changed</summary>
        public event ValueEventHandler ValueChanged;

        protected virtual void DoChanged()
        {
            ValueChanged?.Invoke(this);
        }

        protected virtual void OnValueChanged(GameControl c_control)
        {
            DoChanged();
        }

        /// <summary>Sets the property value</summary>
        /// <param name="c_value">Value to set</param>
        /// <param name="c_fireEvents">Determines whether to fire "value changed" event</param>
        public virtual void SetValue(string c_value, bool c_fireEvents = false)
        {
        }
    }
}