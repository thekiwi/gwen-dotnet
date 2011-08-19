using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Gwen.Controls
{
    public class TextBox : Label
    {
        protected bool m_bSelectAll;

        protected int m_iCursorPos;
        protected int m_iCursorEnd;

        protected Rectangle m_rectSelectionBounds;
        protected Rectangle m_rectCaretBounds;

        protected double m_fLastInputTime;

        public override bool AccelOnlyFocus { get { return true; } }
        public override bool NeedsInputChars { get { return true; } }
        public bool SelectAllOnFocus { get { return m_bSelectAll; } set { m_bSelectAll = value; if (value) onSelectAll(this); } }
        public bool HasSelection { get { return m_iCursorPos != m_iCursorEnd; } }

        public event ControlCallback OnTextChanged;
        public event ControlCallback OnReturnPressed;

        public int CursorPos
        {
            get { return m_iCursorPos; }
            set
            {
                if (m_iCursorPos == value) return;

                m_iCursorPos = value;
                RefreshCursorBounds();
            }
        }

        public int CursorEnd
        {
            get { return m_iCursorEnd; }
            set
            {
                if (m_iCursorEnd == value) return;

                m_iCursorEnd = value;
                RefreshCursorBounds();
            }
        }

        protected virtual bool IsTextAllowed(String str, int iPos)
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

            m_iCursorPos = 0;
            m_iCursorEnd = 0;
            m_bSelectAll = false;

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

            if (m_iCursorPos > TextLength) m_iCursorPos = TextLength;
            if (m_iCursorEnd > TextLength) m_iCursorEnd = TextLength;

            if (OnTextChanged != null)
                OnTextChanged.Invoke(this);
        }

        internal override bool onChar(char chr)
        {
            base.onChar(chr);

            if (chr == '\t') return false;

            InsertText(chr.ToString());
            return true;
        }

        void InsertText(String strInsert)
        {
            // TODO: Make sure fits (implement maxlength)

            if (HasSelection)
            {
                EraseSelection();
            }

            if (m_iCursorPos > TextLength)
                m_iCursorPos = TextLength;

            if (!IsTextAllowed(strInsert, m_iCursorPos))
                return;

            String str = Text;
            str = str.Insert(m_iCursorPos, strInsert);
            SetText(str);

            m_iCursorPos += strInsert.Length;
            m_iCursorEnd = m_iCursorPos;

            RefreshCursorBounds();
        }

        protected override void Render(Skin.Base skin)
        {
            base.Render(skin);

            if (ShouldDrawBackground)
                skin.DrawTextBox(this);

            if (!HasFocus) return;

            // Draw selection.. if selected..
            if (m_iCursorPos != m_iCursorEnd)
            {
                skin.Renderer.DrawColor = Color.FromArgb(200, 50, 170, 255);
                skin.Renderer.DrawFilledRect(m_rectSelectionBounds);
            }

            // Draw caret
            if (Math.IEEERemainder(Platform.Windows.GetTimeInSeconds() - m_fLastInputTime, 1.0) > 0.5)
                skin.Renderer.DrawColor = Color.White;
            else
                skin.Renderer.DrawColor = Color.Black;

            skin.Renderer.DrawFilledRect(m_rectCaretBounds);
        }

        protected virtual void RefreshCursorBounds()
        {
            m_fLastInputTime = Platform.Windows.GetTimeInSeconds();

            MakeCaretVisible();

            Point pA = GetCharacterPosition(m_iCursorPos);
            Point pB = GetCharacterPosition(m_iCursorEnd);

            m_rectSelectionBounds.X = Math.Min(pA.X, pB.X);
            m_rectSelectionBounds.Y = m_Text.Y - 1;
            m_rectSelectionBounds.Width = Math.Max(pA.X, pB.X) - m_rectSelectionBounds.X;
            m_rectSelectionBounds.Height = m_Text.Height + 2;

            m_rectCaretBounds.X = pA.X;
            m_rectCaretBounds.Y = m_Text.Y - 1;
            m_rectCaretBounds.Width = 1;
            m_rectCaretBounds.Height = m_Text.Height + 2;

            Redraw();
        }

        internal override void onPaste(Base from)
        {
            base.onPaste(from);
            InsertText(Platform.Windows.GetClipboardText());
        }

        internal override void onCopy(Base from)
        {
            if (!HasSelection) return;
            base.onCopy(from);

            Platform.Windows.SetClipboardText(GetSelection());
        }

        internal override void onCut(Base from)
        {
            if (!HasSelection) return;
            base.onCut(from);

            Platform.Windows.SetClipboardText(GetSelection());
            EraseSelection();
        }

        internal override void onSelectAll(Base from)
        {
            //base.onSelectAll(from);
            m_iCursorEnd = 0;
            m_iCursorPos = TextLength;

            RefreshCursorBounds();
        }

        internal override void onMouseDoubleClickLeft(int x, int y)
        {
            //base.onMouseDoubleClickLeft(x, y);
            onSelectAll(this);
        }

        internal override bool onKeyReturn(bool bDown)
        {
            base.onKeyReturn(bDown);
            if (bDown) return true;

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

        internal override bool onKeyBackspace(bool bDown)
        {
            base.onKeyBackspace(bDown);

            if (!bDown) return true;
            if (HasSelection)
            {
                EraseSelection();
                return true;
            }

            if (m_iCursorPos == 0) return true;

            DeleteText(m_iCursorPos - 1, 1);

            return true;
        }

        internal override bool onKeyDelete(bool bDown)
        {
            base.onKeyDelete(bDown);
            if (!bDown) return true;
            if (HasSelection)
            {
                EraseSelection();
                return true;
            }

            if (m_iCursorPos >= TextLength) return true;

            DeleteText(m_iCursorPos, 1);

            return true;
        }

        internal override bool onKeyLeft(bool bDown)
        {
            base.onKeyLeft(bDown);
            if (!bDown) return true;

            if (m_iCursorPos > 0)
                m_iCursorPos--;

            if (!Input.Input.IsShiftDown)
            {
                m_iCursorEnd = m_iCursorPos;
            }

            RefreshCursorBounds();
            return true;
        }

        internal override bool onKeyRight(bool bDown)
        {
            base.onKeyRight(bDown);
            if (!bDown) return true;

            if (m_iCursorPos < TextLength)
                m_iCursorPos++;

            if (!Input.Input.IsShiftDown)
            {
                m_iCursorEnd = m_iCursorPos;
            }

            RefreshCursorBounds();
            return true;
        }

        internal override bool onKeyHome(bool bDown)
        {
            base.onKeyHome(bDown);
            if (!bDown) return true;
            m_iCursorPos = 0;

            if (!Input.Input.IsShiftDown)
            {
                m_iCursorEnd = m_iCursorPos;
            }

            RefreshCursorBounds();
            return true;
        }

        internal override bool onKeyEnd(bool bDown)
        {
            base.onKeyEnd(bDown);
            m_iCursorPos = TextLength;

            if (!Input.Input.IsShiftDown)
            {
                m_iCursorEnd = m_iCursorPos;
            }

            RefreshCursorBounds();
            return true;
        }

        public String GetSelection()
        {
            if (!HasSelection) return String.Empty;

            int iStart = Math.Min(m_iCursorPos, m_iCursorEnd);
            int iEnd = Math.Max(m_iCursorPos, m_iCursorEnd);

            String str = Text;
            return str.Substring(iStart, iEnd - iStart);
        }

        public virtual void DeleteText(int iStartPos, int iLength)
        {
            String str = Text;
            str = str.Remove(iStartPos, iLength);
            SetText(str);

            if (m_iCursorPos > iStartPos)
            {
                CursorPos = m_iCursorPos - iLength;
            }

            CursorEnd = m_iCursorPos;
        }

        public virtual void EraseSelection()
        {
            int iStart = Math.Min(m_iCursorPos, m_iCursorEnd);
            int iEnd = Math.Max(m_iCursorPos, m_iCursorEnd);

            DeleteText(iStart, iEnd - iStart);

            // Move the cursor to the start of the selection, 
            // since the end is probably outside of the string now.
            m_iCursorPos = iStart;
            m_iCursorEnd = iStart;
        }

        internal override void onMouseClickLeft(int x, int y, bool bDown)
        {
            base.onMouseClickLeft(x, y, bDown);
            if (m_bSelectAll)
            {
                onSelectAll(this);
                //m_bSelectAll = false;
                return;
            }

            int iChar = m_Text.GetClosestCharacter(m_Text.CanvasPosToLocal(new Point(x, y)));

            if (bDown)
            {
                CursorPos = iChar;

                if (!Input.Input.IsShiftDown)
                    CursorEnd = iChar;

                Global.MouseFocus = this;
            }
            else
            {
                if (Global.MouseFocus == this)
                {
                    CursorPos = iChar;
                    Global.MouseFocus = null;
                }
            }
        }

        internal override void onMouseMoved(int x, int y, int dx, int dy)
        {
            base.onMouseMoved(x, y, dx, dy);
            if (Global.MouseFocus != this) return;

            int iChar = m_Text.GetClosestCharacter(m_Text.CanvasPosToLocal(new Point(x, y)));

            CursorPos = iChar;
        }

        public virtual void MakeCaretVisible()
        {
            int iCaretPos = m_Text.GetCharacterPosition(m_iCursorPos).X;

            // If the carat is already in a semi-good position, leave it.
            {
                int iRealCaretPos = iCaretPos + m_Text.X;
                if (iRealCaretPos > Width*0.1 && iRealCaretPos < Width*0.9)
                    return;
            }

            // The ideal position is for the carat to be right in the middle
            int idealx = Global.Trunc(-iCaretPos + Width*0.5);

            // Don't show too much whitespace to the right
            if (idealx + m_Text.Width < Width - m_rTextPadding.Right)
                idealx = -m_Text.Width + (Width - m_rTextPadding.Right);

            // Or the left
            if (idealx > m_rTextPadding.Left)
                idealx = m_rTextPadding.Left;

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
