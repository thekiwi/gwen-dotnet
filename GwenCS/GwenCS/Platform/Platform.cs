using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Gwen.Platform
{
    public static class Windows
    {
        private static DateTime m_LastTime;
        private static double m_CurrentTime;

        public static void SetCursor(Cursor cursor)
        {
            Cursor.Current = cursor;
        }

        public static String GetClipboardText()
        {
            // code from http://forums.getpaint.net/index.php?/topic/13712-trouble-accessing-the-clipboard/page__view__findpost__p__226140
            String ret = String.Empty;
            Thread staThread = new Thread(
                () =>
                {
                    try
                    {
                        if (!Clipboard.ContainsText())
                            return;
                        ret = Clipboard.GetText();
                    }
                    catch (Exception)
                    {
                        return;
                    }
                });
            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            staThread.Join();
            // at this point either you have clipboard data or an exception
            return ret;
        }

        public static bool SetClipboardText(String text)
        {
            bool ret = false;
            Thread staThread = new Thread(
                () =>
                {
                    try
                    {
                        Clipboard.SetText(text);
                        ret = true;
                    }
                    catch (Exception)
                    {
                        return;
                    }
                });
            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            staThread.Join();
            // at this point either you have clipboard data or an exception
            return ret;
        }

        public static double GetTimeInSeconds()
        {
            var time = DateTime.UtcNow;
            var diff = time - m_LastTime;
            var seconds = diff.TotalSeconds;
            if (seconds > 0.1)
                seconds = 0.1;
            m_CurrentTime += seconds;
            m_LastTime = time;
            return m_CurrentTime;
        }

        public static bool FileOpen(String title, String startPath, String extension, Action<String> callback)
        {
            var dialog = new OpenFileDialog
                             {
                                 Title = title,
                                 InitialDirectory = startPath,
                                 DefaultExt = @"*.*",
                                 Filter = extension,
                                 CheckPathExists = true,
                                 Multiselect = false
                             };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (callback != null)
                {
                    callback(dialog.FileName);
                }
            }
            else
            {
                if (callback != null)
                {
                    callback(String.Empty);
                }
            }

            return true;
        }

        public static bool FileSave(String name, String startPath, String extension, Action<String> callback)
        {
            var dialog = new SaveFileDialog
            {
                Title = name,
                InitialDirectory = startPath,
                DefaultExt = @"*.*",
                Filter = extension,
                CheckPathExists = true,
                OverwritePrompt = true
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (callback != null)
                {
                    callback(dialog.FileName);
                }
            }
            else
            {
                if (callback != null)
                {
                    callback(String.Empty);
                }
            }

            return true;
        }
    }
}
