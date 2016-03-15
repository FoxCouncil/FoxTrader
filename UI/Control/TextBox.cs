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
        public event ButtonEventHandler SubmitPressed;

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

        /// <summary>Handler for character input event</summary>
        /// <param name="c_char">Character typed</param>
        /// <returns>True if handled</returns>
        protected override bool OnChar(char c_char)
        {
            base.OnChar(c_char);

            if (c_char == '\t')
            {
                return false;
            }

            InsertText(c_char.ToString());

            return true;
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

        /// <summary>Handler invoked on mouse double click (left) event</summary>
        /// <param name="c_x">X coordinate</param>
        /// <param name="c_xY">Y coordinate</param>
        protected override void OnMouseDoubleClickedLeft(int c_x, int c_xY)
        {
            OnSelectAll(this);
        }

        /// <summary>Handler for Return keyboard event</summary>
        /// <param name="c_isButtonDown">Indicates whether the key was pressed or released</param>
        /// <returns>True if handled</returns>
        protected override bool OnKeyReturn(bool c_isButtonDown)
        {
            base.OnKeyReturn(c_isButtonDown);

            if (c_isButtonDown)
            {
                return true;
            }

            OnReturn();

            OnKeyTab(true);

            if (HasFocus)
            {
                Blur();
            }

            return true;
        }

        /// <summary>Handler for Backspace keyboard event</summary>
        /// <param name="c_isButtonDown">Indicates whether the key was pressed or released</param>
        /// <returns>True if handled</returns>
        protected override bool OnKeyBackspace(bool c_isButtonDown)
        {
            base.OnKeyBackspace(c_isButtonDown);

            if (!c_isButtonDown)
            {
                return true;
            }

            if (HasSelection)
            {
                EraseSelection();
                return true;
            }

            if (m_cursorPos == 0)
            {
                return true;
            }

            DeleteText(m_cursorPos - 1, 1);

            return true;
        }

        /// <summary>Handler for Delete keyboard event</summary>
        /// <param name="c_isButtonDown">Indicates whether the key was pressed or released</param>
        /// <returns>True if handled</returns>
        protected override bool OnKeyDelete(bool c_isButtonDown)
        {
            base.OnKeyDelete(c_isButtonDown);

            if (!c_isButtonDown)
            {
                return true;
            }

            if (HasSelection)
            {
                EraseSelection();
                return true;
            }

            if (m_cursorPos >= TextLength)
            {
                return true;
            }

            DeleteText(m_cursorPos, 1);

            return true;
        }

        /// <summary>Handler for Left Arrow keyboard event</summary>
        /// <param name="c_isButtonDown">Indicates whether the key was pressed or released</param>
        /// <returns>True if handled</returns>
        protected override bool OnKeyLeft(bool c_isButtonDown)
        {
            base.OnKeyLeft(c_isButtonDown);

            if (!c_isButtonDown)
            {
                return true;
            }

            if (m_cursorPos > 0)
            {
                m_cursorPos--;
            }

            if (!FoxTraderWindow.Instance.IsShiftDown)
            {
                m_cursorEnd = m_cursorPos;
            }

            RefreshCursorBounds();

            return true;
        }

        /// <summary>Handler for Right Arrow keyboard event</summary>
        /// <param name="c_isButtonDown">Indicates whether the key was pressed or released</param>
        /// <returns>True if handled</returns>
        protected override bool OnKeyRight(bool c_isButtonDown)
        {
            base.OnKeyRight(c_isButtonDown);

            if (!c_isButtonDown)
            {
                return true;
            }

            if (m_cursorPos < TextLength)
            {
                m_cursorPos++;
            }

            if (!FoxTraderWindow.Instance.IsShiftDown)
            {
                m_cursorEnd = m_cursorPos;
            }

            RefreshCursorBounds();

            return true;
        }

        /// <summary>Handler for Home keyboard event</summary>
        /// <param name="c_isButtonDown">Indicates whether the key was pressed or released</param>
        /// <returns>True if handled</returns>
        protected override bool OnKeyHome(bool c_isButtonDown)
        {
            base.OnKeyHome(c_isButtonDown);

            if (!c_isButtonDown)
            {
                return true;
            }

            m_cursorPos = 0;

            if (!FoxTraderWindow.Instance.IsShiftDown)
            {
                m_cursorEnd = m_cursorPos;
            }

            RefreshCursorBounds();

            return true;
        }

        /// <summary>Handler for End keyboard event</summary>
        /// <param name="c_isButtonDown">Indicates whether the key was pressed or released</param>
        /// <returns>True if handled</returns>
        protected override bool OnKeyEnd(bool c_isButtonDown)
        {
            base.OnKeyEnd(c_isButtonDown);

            m_cursorPos = TextLength;

            if (!FoxTraderWindow.Instance.IsShiftDown)
            {
                m_cursorEnd = m_cursorPos;
            }

            RefreshCursorBounds();

            return true;
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

        /// <summary>Handler invoked on mouse click (left) event</summary>
        /// <param name="c_x">X coordinate</param>
        /// <param name="c_y">Y coordinate</param>
        /// <param name="c_down">If set to <c>true</c> mouse button is down</param>
        protected override void OnMouseClickedLeft(int c_x, int c_y, bool c_down)
        {
            base.OnMouseClickedLeft(c_x, c_y, c_down);

            if (m_selectAll)
            {
                OnSelectAll(this);
                return;
            }

            var a_cursorPosition = GetClosestCharacter(c_x, c_y);

            if (c_down)
            {
                CursorPos = a_cursorPosition;

                if (!FoxTraderWindow.Instance.IsShiftDown)
                {
                    CursorEnd = a_cursorPosition;
                }

                FoxTraderWindow.Instance.MouseFocus = this;
            }
            else
            {
                if (FoxTraderWindow.Instance.MouseFocus == this)
                {
                    CursorPos = a_cursorPosition;
                    FoxTraderWindow.Instance.MouseFocus = null;
                }
            }
        }

        /// <summary>Handler invoked on mouse moved event</summary>
        /// <param name="c_x">X coordinate</param>
        /// <param name="c_y">Y coordinate</param>
        /// <param name="c_dx">X change</param>
        /// <param name="c_dy">Y change</param>
        protected override void OnMouseMoved(MouseState c_mouseState, int c_x, int c_y, int c_dx, int c_dy)
        {
            base.OnMouseMoved(c_mouseState, c_x, c_y, c_dx, c_dy);

            if (FoxTraderWindow.Instance.MouseFocus != this)
            {
                return;
            }

            var a_cursorPosition = GetClosestCharacter(c_x, c_y);

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
        protected override void Layout(SkinBase c_skin)
        {
            base.Layout(c_skin);

            RefreshCursorBounds();
        }

        /// <summary>Handler for the return key</summary>
        protected virtual void OnReturn()
        {
            if (SubmitPressed != null)
            {
                SubmitPressed.Invoke(this);
            }
        }
    }
}