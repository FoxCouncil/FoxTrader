using System;
using System.Drawing;
using FoxTrader.UI.Platform;
using FoxTrader.UI.Skin;
using OpenTK.Input;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control
{
    /// <summary>Text box (editable)</summary>
    internal class TextBox : Label
    {
        private Rectangle m_caretBounds;
        private int m_cursorEnd;

        private int m_cursorPos;

        private float m_lastInputTime;
        private bool m_selectAll;

        private Rectangle m_selectionBounds;

        /// <summary>Initializes a new instance of the <see cref="TextBox" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public TextBox(GameControl c_parentControl) : base(c_parentControl)
        {
            SetSize(200, 20);

            MouseInputEnabled = true;
            KeyboardInputEnabled = true;

            Alignment = Pos.Left | Pos.CenterV;
            TextPadding = new Padding(4, 2, 4, 2);

            m_cursorPos = 0;
            m_cursorEnd = 0;
            m_selectAll = false;

            TextColor = Color.FromArgb(255, 50, 50, 50); // TODO: From Skin

            IsTabable = true;

            AddAccelerator("Ctrl + C", OnCopy);
            AddAccelerator("Ctrl + X", OnCut);
            AddAccelerator("Ctrl + V", OnPaste);
            AddAccelerator("Ctrl + A", OnSelectAll);

            DoubleClicked += (c_control, c_args) =>
            {
                OnSelectAll(this);
            };
        }

        protected override bool AccelOnlyFocus => true;

        protected override bool NeedsInputChars => true;

        /// <summary>Determines whether text should be selected when the control is focused</summary>
        public bool SelectAllOnFocus
        {
            get
            {
                return m_selectAll;
            }
            set
            {
                m_selectAll = value;
                if (value)
                {
                    OnSelectAll(this);
                }
            }
        }

        /// <summary>Indicates whether the text has active selection</summary>
        public bool HasSelection => m_cursorPos != m_cursorEnd;

        /// <summary>Current cursor position (character index)</summary>
        public int CursorPos
        {
            get
            {
                return m_cursorPos;
            }

            set
            {
                if (m_cursorPos == value)
                {
                    return;
                }

                m_cursorPos = value;

                RefreshCursorBounds();
            }
        }

        public int CursorEnd
        {
            get
            {
                return m_cursorEnd;
            }

            set
            {
                if (m_cursorEnd == value)
                {
                    return;
                }

                m_cursorEnd = value;

                RefreshCursorBounds();
            }
        }

        /// <summary>Invoked when the text has changed</summary>
        public event TextEventHandler TextChanged;

        /// <summary>Invoked when the submit key has been pressed</summary>
        public event MouseButtonEventHandler SubmitPressed;

        /// <summary>Determines whether the control can insert text at a given cursor position</summary>
        /// <param name="c_text">Text to check</param>
        /// <param name="c_position">Cursor position</param>
        /// <returns>True if allowed</returns>
        protected virtual bool IsTextAllowed(string c_text, int c_position)
        {
            return true;
        }

        /// <summary>Renders the focus overlay</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void RenderFocus(SkinBase c_skin)
        {
            // nothing
        }

        /// <summary>Handler for text changed event</summary>
        protected override void OnTextChanged()
        {
            base.OnTextChanged();

            if (m_cursorPos > TextLength)
            {
                m_cursorPos = TextLength;
            }

            if (m_cursorEnd > TextLength)
            {
                m_cursorEnd = TextLength;
            }

            if (TextChanged != null)
            {
                TextChanged.Invoke(this);
            }
        }

        /// <summary>Inserts text at current cursor position, erasing selection if any</summary>
        /// <param name="c_text">Text to insert</param>
        private void InsertText(string c_text)
        {
            // TODO: Make sure fits (implement maxlength)
            if (HasSelection)
            {
                EraseSelection();
            }

            if (m_cursorPos > TextLength)
            {
                m_cursorPos = TextLength;
            }

            if (!IsTextAllowed(c_text, m_cursorPos))
            {
                return;
            }

            var a_str = Text;

            a_str = a_str.Insert(m_cursorPos, c_text);

            SetText(a_str);

            m_cursorPos += c_text.Length;
            m_cursorEnd = m_cursorPos;

            RefreshCursorBounds();
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(SkinBase c_skin)
        {
            base.Render(c_skin);

            if (ShouldDrawBackground)
            {
                c_skin.DrawTextBox(this);
            }

            if (!HasFocus)
            {
                return;
            }

            if (m_cursorPos != m_cursorEnd)
            {
                c_skin.Renderer.DrawColor = Color.FromArgb(200, 50, 170, 255);
                c_skin.Renderer.DrawFilledRect(m_selectionBounds);
            }

            var a_time = Neutral.GetTimeInSeconds() - m_lastInputTime;

            if ((a_time % 1.0f) <= 0.5f)
            {
                c_skin.Renderer.DrawColor = Color.Black;
                c_skin.Renderer.DrawFilledRect(m_caretBounds);
            }
        }

        protected virtual void RefreshCursorBounds()
        {
            m_lastInputTime = Neutral.GetTimeInSeconds();

            MakeCaretVisible();

            var a_pointA = GetCharacterPosition(m_cursorPos);
            var a_pointB = GetCharacterPosition(m_cursorEnd);

            m_selectionBounds.X = Math.Min(a_pointA.X, a_pointB.X);
            m_selectionBounds.Y = TextY - 1;
            m_selectionBounds.Width = Math.Max(a_pointA.X, a_pointB.X) - m_selectionBounds.X;
            m_selectionBounds.Height = TextHeight + 2;

            m_caretBounds.X = a_pointA.X;
            m_caretBounds.Y = TextY - 1;
            m_caretBounds.Width = 1;
            m_caretBounds.Height = TextHeight + 2;

            Redraw();
        }

        /// <summary>Handler for Paste event</summary>
        /// <param name="c_fromControl">Source control</param>
        protected override void OnPaste(GameControl c_fromControl)
        {
            base.OnPaste(c_fromControl);
            InsertText(Neutral.GetClipboardText());
        }

        /// <summary>Handler for Copy event</summary>
        /// <param name="c_fromControl">Source control</param>
        protected override void OnCopy(GameControl c_fromControl)
        {
            if (!HasSelection)
            {
                return;
            }

            base.OnCopy(c_fromControl);

            Neutral.SetClipboardText(GetSelection());
        }

        /// <summary>Handler for Cut event</summary>
        /// <param name="c_fromControl">Source control</param>
        protected override void OnCut(GameControl c_fromControl)
        {
            if (!HasSelection)
            {
                return;
            }

            base.OnCut(c_fromControl);

            Neutral.SetClipboardText(GetSelection());
            EraseSelection();
        }

        /// <summary>Handler for Select All event</summary>
        /// <param name="c_fromControl">Source control</param>
        protected override void OnSelectAll(GameControl c_fromControl)
        {
            m_cursorEnd = 0;
            m_cursorPos = TextLength;

            RefreshCursorBounds();
        }

        public override void OnKeyDown(KeyboardKeyEventArgs c_keyboardKeyEventArgs)
        {
            base.OnKeyDown(c_keyboardKeyEventArgs);

            switch (c_keyboardKeyEventArgs.Key)
            {
                case Key.BackSpace:
                {
                    if (HasSelection)
                    {
                        EraseSelection();
                        return;
                    }

                    if (m_cursorPos == 0)
                    {
                        return;
                    }

                    DeleteText(m_cursorPos - 1, 1);
                }
                break;

                case Key.Delete:
                {
                    if (HasSelection)
                    {
                        EraseSelection();
                        return;
                    }

                    if (m_cursorPos >= TextLength)
                    {
                        return;
                    }

                    DeleteText(m_cursorPos, 1);
                }
                break;

                case Key.Left:
                {
                    if (m_cursorPos > 0)
                    {
                        m_cursorPos--;
                    }

                    if (!c_keyboardKeyEventArgs.Keyboard.IsKeyDown(Key.ShiftLeft))
                    {
                        m_cursorEnd = m_cursorPos;
                    }

                    RefreshCursorBounds();
                }
                break;

                case Key.Right:
                {
                    if (m_cursorPos < TextLength)
                    {
                        m_cursorPos++;
                    }

                    if (!c_keyboardKeyEventArgs.Keyboard.IsKeyDown(Key.ShiftLeft))
                    {
                        m_cursorEnd = m_cursorPos;
                    }

                    RefreshCursorBounds();
                }
                break;

                case Key.Home:
                {
                    m_cursorPos = 0;

                    if (!c_keyboardKeyEventArgs.Keyboard.IsKeyDown(Key.ShiftLeft))
                    {
                        m_cursorEnd = m_cursorPos;
                    }

                    RefreshCursorBounds();
                }
                break;

                case Key.End:
                {
                    m_cursorPos = TextLength;

                    if (!c_keyboardKeyEventArgs.Keyboard.IsKeyDown(Key.ShiftLeft))
                    {
                        m_cursorEnd = m_cursorPos;
                    }

                    RefreshCursorBounds();
                }
                break;

                default:
                {
                    var a_filteredCharacter = Renderer.TranslateChar(c_keyboardKeyEventArgs.Key);

                    if (a_filteredCharacter == '\t')
                    {
                        return;
                    }

                    InsertText(a_filteredCharacter.ToString());
                }
                break;
            }
        }

        public override void OnKeyUp(KeyboardKeyEventArgs c_keyboardKeyEventArgs)
        {
            base.OnKeyUp(c_keyboardKeyEventArgs);

            switch (c_keyboardKeyEventArgs.Key)
            {
                case Key.Enter:
                case Key.KeypadEnter:
                {
                    OnReturn();

                    if (HasFocus)
                    {
                        OnBlur();
                    }
                }
                break;
            }
        }

        /// <summary>Returns currently selected text</summary>
        /// <returns>Current selection</returns>
        public string GetSelection()
        {
            if (!HasSelection)
            {
                return string.Empty;
            }

            var a_start = Math.Min(m_cursorPos, m_cursorEnd);
            var a_end = Math.Max(m_cursorPos, m_cursorEnd);

            var a_string = Text;

            return a_string.Substring(a_start, a_end - a_start);
        }

        /// <summary>Deletes text</summary>
        /// <param name="c_startPos">Starting cursor position</param>
        /// <param name="c_length">Length in characters</param>
        public virtual void DeleteText(int c_startPos, int c_length)
        {
            var a_string = Text;

            a_string = a_string.Remove(c_startPos, c_length);

            SetText(a_string);

            if (m_cursorPos > c_startPos)
            {
                CursorPos = m_cursorPos - c_length;
            }

            CursorEnd = m_cursorPos;
        }

        /// <summary>Deletes selected text</summary>
        public virtual void EraseSelection()
        {
            var a_start = Math.Min(m_cursorPos, m_cursorEnd);
            var a_end = Math.Max(m_cursorPos, m_cursorEnd);

            DeleteText(a_start, a_end - a_start);

            // Move the cursor to the start of the selection, 
            // since the end is probably outside of the string now.
            m_cursorPos = a_start;
            m_cursorEnd = a_start;
        }

        public override void OnMouseDown(MouseButtonEventArgs c_mouseButtonEventArgs)
        {
            base.OnMouseDown(c_mouseButtonEventArgs);

            if (m_selectAll)
            {
                OnSelectAll(this);
                return;
            }

            CursorPos = GetClosestCharacter(c_mouseButtonEventArgs.Position);

            if (!GetCanvas().ShiftKeyToggled)
            {
                CursorEnd = CursorPos;
            }

            GetCanvas().MouseFocus = this;
        }

        public override void OnMouseUp(MouseButtonEventArgs c_mouseButtonEventArgs)
        {
            base.OnMouseUp(c_mouseButtonEventArgs);

            if (GetCanvas().MouseFocus == this)
            {
                CursorPos = GetClosestCharacter(c_mouseButtonEventArgs.Position);
                GetCanvas().MouseFocus = null;
            }
        }

        /// <summary>Handler invoked on mouse moved event</summary>
        /// <param name="c_x">X coordinate</param>
        /// <param name="c_y">Y coordinate</param>
        /// <param name="c_dx">X change</param>
        /// <param name="c_dy">Y change</param>
        public override void OnMouseMoved(MouseMoveEventArgs c_mouseEventArgs)
        {
            base.OnMouseMoved(c_mouseEventArgs);

            if (GetCanvas().MouseFocus != this)
            {
                return;
            }

            var a_cursorPosition = GetClosestCharacter(c_mouseEventArgs.Position);

            CursorPos = a_cursorPosition;
        }

        protected virtual void MakeCaretVisible()
        {
            var a_caretPosition = GetCharacterPosition(m_cursorPos).X - TextX;

            // If the caret is already in a semi-good position, leave it.
            {
                var a_realCaretPos = a_caretPosition + TextX;

                if (a_realCaretPos > Width * 0.1f && a_realCaretPos < Width * 0.9f)
                {
                    return;
                }
            }

            // The ideal position is for the caret to be right in the middle
            var a_idealX = (int)(-a_caretPosition + Width * 0.5f);

            // Don't show too much whitespace to the right
            if (a_idealX + TextWidth < Width - TextPadding.m_right)
            {
                a_idealX = -TextWidth + (Width - TextPadding.m_right);
            }

            // Or the left
            if (a_idealX > TextPadding.m_left)
            {
                a_idealX = TextPadding.m_left;
            }

            SetTextPosition(a_idealX, TextY);
        }

        /// <summary>Lays out the control's interior according to alignment, padding, dock etc</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void OnLayout(SkinBase c_skin)
        {
            base.OnLayout(c_skin);

            RefreshCursorBounds();
        }

        /// <summary>Handler for the return key</summary>
        protected virtual void OnReturn()
        {
            SubmitPressed?.Invoke(this, null);
        }
    }
}