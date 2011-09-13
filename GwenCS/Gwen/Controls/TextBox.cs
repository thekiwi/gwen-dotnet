using System;
using System.Drawing;

namespace Gwen.Controls
{
    public class TextBox : Label
    {
        protected bool m_SelectAll;

        protected int m_CursorPos;
        protected int m_CursorEnd;

        protected Rectangle m_SelectionBounds;
        protected Rectangle m_CaretBounds;

        protected float m_LastInputTime;

        protected override bool AccelOnlyFocus { get { return true; } }
        protected override bool NeedsInputChars { get { return true; } }
        public bool SelectAllOnFocus { get { return m_SelectAll; } set { m_SelectAll = value; if (value) onSelectAll(this); } }
        public bool HasSelection { get { return m_CursorPos != m_CursorEnd; } }

        public event ControlCallback OnTextChanged;
        public event ControlCallback OnReturnPressed;

        public int CursorPos
        {
            get { return m_CursorPos; }
            set
            {
                if (m_CursorPos == value) return;

                m_CursorPos = value;
                RefreshCursorBounds();
            }
        }

        public int CursorEnd
        {
            get { return m_CursorEnd; }
            set
            {
                if (m_CursorEnd == value) return;

                m_CursorEnd = value;
                RefreshCursorBounds();
            }
        }

        protected virtual bool IsTextAllowed(String str, int pos)
        {
            return true;
        }

        public TextBox(Base parent)
            : base(parent)
        {
            SetSize(200, 20);

            MouseInputEnabled = true;
            KeyboardInputEnabled = true;

            Alignment = Pos.Left | Pos.CenterV;
            TextPadding = new Padding(4, 2, 4, 2);

            m_CursorPos = 0;
            m_CursorEnd = 0;
            m_SelectAll = false;

            TextColor = Color.FromArgb(255, 50, 50, 50); // TODO: From Skin

            IsTabable = true;

            AddAccelerator("Ctrl + C", onCopy);
            AddAccelerator("Ctrl + X", onCut);
            AddAccelerator("Ctrl + V", onPaste);
            AddAccelerator("Ctrl + A", onSelectAll);
        }

        protected override void RenderFocus(Skin.Base skin)
        {
            // nothing
        }

        protected override void onTextChanged()
        {
            base.onTextChanged();

            if (m_CursorPos > TextLength) m_CursorPos = TextLength;
            if (m_CursorEnd > TextLength) m_CursorEnd = TextLength;

            if (OnTextChanged != null)
                OnTextChanged.Invoke(this);
        }

        protected override bool onChar(char chr)
        {
            base.onChar(chr);

            if (chr == '\t') return false;

            InsertText(chr.ToString());
            return true;
        }

        void InsertText(String insert)
        {
            // TODO: Make sure fits (implement maxlength)

            if (HasSelection)
            {
                EraseSelection();
            }

            if (m_CursorPos > TextLength)
                m_CursorPos = TextLength;

            if (!IsTextAllowed(insert, m_CursorPos))
                return;

            String str = Text;
            str = str.Insert(m_CursorPos, insert);
            SetText(str);

            m_CursorPos += insert.Length;
            m_CursorEnd = m_CursorPos;

            RefreshCursorBounds();
        }

        protected override void Render(Skin.Base skin)
        {
            base.Render(skin);

            if (ShouldDrawBackground)
                skin.DrawTextBox(this);

            if (!HasFocus) return;

            // Draw selection.. if selected..
            if (m_CursorPos != m_CursorEnd)
            {
                skin.Renderer.DrawColor = Color.FromArgb(200, 50, 170, 255);
                skin.Renderer.DrawFilledRect(m_SelectionBounds);
            }

            // Draw caret
            if (Math.IEEERemainder(Platform.Neutral.GetTimeInSeconds() - m_LastInputTime, 1.0f) > 0.5f) // todo: ok?
                skin.Renderer.DrawColor = Color.White;
            else
                skin.Renderer.DrawColor = Color.Black;

            skin.Renderer.DrawFilledRect(m_CaretBounds);
        }

        protected virtual void RefreshCursorBounds()
        {
            m_LastInputTime = Platform.Neutral.GetTimeInSeconds();

            MakeCaretVisible();

            Point pA = GetCharacterPosition(m_CursorPos);
            Point pB = GetCharacterPosition(m_CursorEnd);

            m_SelectionBounds.X = Math.Min(pA.X, pB.X);
            m_SelectionBounds.Y = m_Text.Y - 1;
            m_SelectionBounds.Width = Math.Max(pA.X, pB.X) - m_SelectionBounds.X;
            m_SelectionBounds.Height = m_Text.Height + 2;

            m_CaretBounds.X = pA.X;
            m_CaretBounds.Y = m_Text.Y - 1;
            m_CaretBounds.Width = 1;
            m_CaretBounds.Height = m_Text.Height + 2;

            Redraw();
        }

        protected override void onPaste(Base from)
        {
            base.onPaste(from);
            InsertText(Platform.Neutral.GetClipboardText());
        }

        protected override void onCopy(Base from)
        {
            if (!HasSelection) return;
            base.onCopy(from);

            Platform.Neutral.SetClipboardText(GetSelection());
        }

        protected override void onCut(Base from)
        {
            if (!HasSelection) return;
            base.onCut(from);

            Platform.Neutral.SetClipboardText(GetSelection());
            EraseSelection();
        }

        protected override void onSelectAll(Base from)
        {
            //base.onSelectAll(from);
            m_CursorEnd = 0;
            m_CursorPos = TextLength;

            RefreshCursorBounds();
        }

        protected override void onMouseDoubleClickLeft(int x, int y)
        {
            //base.onMouseDoubleClickLeft(x, y);
            onSelectAll(this);
        }

        protected override bool onKeyReturn(bool down)
        {
            base.onKeyReturn(down);
            if (down) return true;

            onEnter();

            // Try to move to the next control, as if tab had been pressed
            onKeyTab(true);

            // If we still have focus, blur it.
            if (HasFocus)
            {
                Blur();
            }

            return true;
        }

        protected override bool onKeyBackspace(bool down)
        {
            base.onKeyBackspace(down);

            if (!down) return true;
            if (HasSelection)
            {
                EraseSelection();
                return true;
            }

            if (m_CursorPos == 0) return true;

            DeleteText(m_CursorPos - 1, 1);

            return true;
        }

        protected override bool onKeyDelete(bool down)
        {
            base.onKeyDelete(down);
            if (!down) return true;
            if (HasSelection)
            {
                EraseSelection();
                return true;
            }

            if (m_CursorPos >= TextLength) return true;

            DeleteText(m_CursorPos, 1);

            return true;
        }

        protected override bool onKeyLeft(bool down)
        {
            base.onKeyLeft(down);
            if (!down) return true;

            if (m_CursorPos > 0)
                m_CursorPos--;

            if (!Input.Input.IsShiftDown)
            {
                m_CursorEnd = m_CursorPos;
            }

            RefreshCursorBounds();
            return true;
        }

        protected override bool onKeyRight(bool down)
        {
            base.onKeyRight(down);
            if (!down) return true;

            if (m_CursorPos < TextLength)
                m_CursorPos++;

            if (!Input.Input.IsShiftDown)
            {
                m_CursorEnd = m_CursorPos;
            }

            RefreshCursorBounds();
            return true;
        }

        protected override bool onKeyHome(bool down)
        {
            base.onKeyHome(down);
            if (!down) return true;
            m_CursorPos = 0;

            if (!Input.Input.IsShiftDown)
            {
                m_CursorEnd = m_CursorPos;
            }

            RefreshCursorBounds();
            return true;
        }

        protected override bool onKeyEnd(bool down)
        {
            base.onKeyEnd(down);
            m_CursorPos = TextLength;

            if (!Input.Input.IsShiftDown)
            {
                m_CursorEnd = m_CursorPos;
            }

            RefreshCursorBounds();
            return true;
        }

        public String GetSelection()
        {
            if (!HasSelection) return String.Empty;

            int start = Math.Min(m_CursorPos, m_CursorEnd);
            int end = Math.Max(m_CursorPos, m_CursorEnd);

            String str = Text;
            return str.Substring(start, end - start);
        }

        public virtual void DeleteText(int startPos, int length)
        {
            String str = Text;
            str = str.Remove(startPos, length);
            SetText(str);

            if (m_CursorPos > startPos)
            {
                CursorPos = m_CursorPos - length;
            }

            CursorEnd = m_CursorPos;
        }

        public virtual void EraseSelection()
        {
            int start = Math.Min(m_CursorPos, m_CursorEnd);
            int end = Math.Max(m_CursorPos, m_CursorEnd);

            DeleteText(start, end - start);

            // Move the cursor to the start of the selection, 
            // since the end is probably outside of the string now.
            m_CursorPos = start;
            m_CursorEnd = start;
        }

        protected override void onMouseClickLeft(int x, int y, bool down)
        {
            base.onMouseClickLeft(x, y, down);
            if (m_SelectAll)
            {
                onSelectAll(this);
                //m_SelectAll = false;
                return;
            }

            int c = m_Text.GetClosestCharacter(m_Text.CanvasPosToLocal(new Point(x, y)));

            if (down)
            {
                CursorPos = c;

                if (!Input.Input.IsShiftDown)
                    CursorEnd = c;

                Global.MouseFocus = this;
            }
            else
            {
                if (Global.MouseFocus == this)
                {
                    CursorPos = c;
                    Global.MouseFocus = null;
                }
            }
        }

        protected override void onMouseMoved(int x, int y, int dx, int dy)
        {
            base.onMouseMoved(x, y, dx, dy);
            if (Global.MouseFocus != this) return;

            int c = m_Text.GetClosestCharacter(m_Text.CanvasPosToLocal(new Point(x, y)));

            CursorPos = c;
        }

        public virtual void MakeCaretVisible()
        {
            int caretPos = m_Text.GetCharacterPosition(m_CursorPos).X;

            // If the carat is already in a semi-good position, leave it.
            {
                int realCaretPos = caretPos + m_Text.X;
                if (realCaretPos > Width*0.1f && realCaretPos < Width*0.9f)
                    return;
            }

            // The ideal position is for the carat to be right in the middle
            int idealx = (int)(-caretPos + Width * 0.5f);

            // Don't show too much whitespace to the right
            if (idealx + m_Text.Width < Width - m_TextPadding.Right)
                idealx = -m_Text.Width + (Width - m_TextPadding.Right);

            // Or the left
            if (idealx > m_TextPadding.Left)
                idealx = m_TextPadding.Left;

            m_Text.SetPos(idealx, m_Text.Y);
        }

        protected override void Layout(Skin.Base skin)
        {
            base.Layout(skin);

            RefreshCursorBounds();
        }

        protected virtual void onEnter()
        {
            if (OnReturnPressed != null)
                OnReturnPressed.Invoke(this);
        }
    }
}
