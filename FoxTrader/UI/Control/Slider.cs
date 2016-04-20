using System;
using System.Drawing;
using FoxTrader.UI.ControlInternal;
using OpenTK.Input;

namespace FoxTrader.UI.Control
{
    /// <summary>Base slider</summary>
    internal class Slider : GameControl
    {
        protected readonly SliderBar m_sliderBar;
        protected float m_max;
        protected float m_min;
        protected int m_notchCount;
        protected bool m_snapToNotches;
        protected float m_value;

        /// <summary>Initializes a new instance of the <see cref="Slider" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        protected Slider(GameControl c_parentControl) : base(c_parentControl)
        {
            SetBounds(new Rectangle(0, 0, 32, 128));

            m_sliderBar = new SliderBar(this);
            m_sliderBar.Dragged += OnMoved;

            m_min = 0.0f;
            m_max = 1.0f;

            m_snapToNotches = false;
            m_notchCount = 5;
            m_value = 0.0f;

            KeyboardInputEnabled = true;
            IsTabable = true;
        }

        /// <summary>Number of notches on the slider axis</summary>
        public int NotchCount
        {
            get
            {
                return m_notchCount;
            }
            set
            {
                m_notchCount = value;
            }
        }

        /// <summary>Determines whether the slider should snap to notches</summary>
        public bool SnapToNotches
        {
            get
            {
                return m_snapToNotches;
            }
            set
            {
                m_snapToNotches = value;
            }
        }

        /// <summary>Minimum value</summary>
        public float Min
        {
            get
            {
                return m_min;
            }
            set
            {
                SetRange(value, m_max);
            }
        }

        /// <summary>Maximum value</summary>
        public float Max
        {
            get
            {
                return m_max;
            }
            set
            {
                SetRange(m_min, value);
            }
        }

        /// <summary>Current value</summary>
        public float Value
        {
            get
            {
                return m_min + (m_value * (m_max - m_min));
            }

            set
            {
                if (value < m_min)
                {
                    value = m_min;
                }

                if (value > m_max)
                {
                    value = m_max;
                }

                // Normalize Value
                value = (value - m_min) / (m_max - m_min);

                SetValueInternal(value);

                Redraw();
            }
        }

        /// <summary>Invoked when the value has been changed</summary>
        public event ValueEventHandler ValueChanged;

        public override void OnKeyDown(KeyboardKeyEventArgs c_keyboardKeyEventArgs)
        {
            switch (c_keyboardKeyEventArgs.Key)
            {
                case Key.Right:
                case Key.Up:
                {
                    Value = Value + 1;
                }
                break;

                case Key.Left:
                case Key.Down:
                {
                    Value = Value - 1;
                }
                break;

                case Key.Home:
                {
                    Value = m_min;
                }
                break;

                case Key.End:
                {
                    Value = m_max;
                }
                break;
            }
        }

        protected virtual void OnMoved(GameControl c_control)
        {
            SetValueInternal(CalculateValue());
        }

        protected virtual float CalculateValue()
        {
            return 0;
        }

        protected virtual void UpdateBarFromValue()
        {
        }

        protected virtual void SetValueInternal(float c_value)
        {
            if (m_snapToNotches)
            {
                c_value = (float)Math.Floor((c_value * m_notchCount) + 0.5f);
                c_value /= m_notchCount;
            }

            if (m_value != c_value)
            {
                m_value = c_value;

                ValueChanged?.Invoke(this);
            }

            UpdateBarFromValue();
        }

        /// <summary>Sets the value range</summary>
        /// <param name="c_min">Minimum value</param>
        /// <param name="c_max">Maximum value</param>
        public void SetRange(float c_min, float c_max)
        {
            m_min = c_min;
            m_max = c_max;
        }

        /// <summary>Renders the focus overlay</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void RenderFocus(Skin c_skin)
        {
            if (GetCanvas().KeyboardFocus != this || !IsTabable)
            {
                return;
            }

            c_skin.DrawKeyboardHighlight(this, RenderBounds, 0);
        }
    }
}