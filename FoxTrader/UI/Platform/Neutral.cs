using System;
using System.Threading;
using System.Windows.Forms;

namespace FoxTrader.UI.Platform
{
    /// <summary>Platform-agnostic utility functions</summary>
    public static class Neutral
    {
        private static DateTime m_lastTime;
        private static float m_currentTime;

        /// <summary>Changes the mouse cursor</summary>
        /// <param name="c_cursor">Cursor type</param>
        public static void SetCursor(Cursor c_cursor)
        {
            Cursor.Current = c_cursor;
        }

        /// <summary>Gets text from clipboard</summary>
        /// <returns>Clipboard text</returns>
        public static string GetClipboardText()
        {
            // code from http://forums.getpaint.net/index.php?/topic/13712-trouble-accessing-the-clipboard/page__view__findpost__p__226140
            var a_ret = string.Empty;
            var a_staThread = new Thread(() =>
            {
                try
                {
                    if (!Clipboard.ContainsText())
                    {
                        return;
                    }
                    a_ret = Clipboard.GetText();
                }
                catch (Exception)
                {
                }
            });
            a_staThread.SetApartmentState(ApartmentState.STA);
            a_staThread.Start();
            a_staThread.Join();
            // at this point either you have clipboard data or an exception
            return a_ret;
        }

        /// <summary>Sets the clipboard text</summary>
        /// <param name="c_text">Text to set</param>
        /// <returns>True if succeeded</returns>
        public static bool SetClipboardText(string c_text)
        {
            var a_ret = false;
            var a_staThread = new Thread(() =>
            {
                try
                {
                    Clipboard.SetText(c_text);
                    a_ret = true;
                }
                catch (Exception)
                {
                }
            });
            a_staThread.SetApartmentState(ApartmentState.STA);
            a_staThread.Start();
            a_staThread.Join();
            // at this point either you have clipboard data or an exception
            return a_ret;
        }

        /// <summary>Gets time since last measurement</summary>
        /// <returns>Time interval in seconds</returns>
        public static float GetTimeInSeconds()
        {
            var a_time = DateTime.UtcNow;
            var a_diff = a_time - m_lastTime;
            var a_seconds = a_diff.TotalSeconds;
            if (a_seconds > 0.1)
            {
                a_seconds = 0.1;
            }
            m_currentTime += (float)a_seconds;
            m_lastTime = a_time;
            return m_currentTime;
        }

        /// <summary>Displays an open file dialog</summary>
        /// <param name="c_title">Dialog title</param>
        /// <param name="c_startPath">Initial path</param>
        /// <param name="c_extension">File extension filter</param>
        /// <param name="c_callback">Callback that is executed after the dialog completes</param>
        /// <returns>True if succeeded</returns>
        public static bool FileOpen(string c_title, string c_startPath, string c_extension, Action<string> c_callback)
        {
            var a_dialog = new OpenFileDialog { Title = c_title, InitialDirectory = c_startPath, DefaultExt = @"*.*", Filter = c_extension, CheckPathExists = true, Multiselect = false };
            if (a_dialog.ShowDialog() == DialogResult.OK)
            {
                c_callback?.Invoke(a_dialog.FileName);
            }
            else
            {
                c_callback?.Invoke(string.Empty);
                return false;
            }

            return true;
        }

        /// <summary>Displays a save file dialog</summary>
        /// <param name="c_title">Dialog title</param>
        /// <param name="c_startPath">Initial path</param>
        /// <param name="c_extension">File extension filter</param>
        /// <param name="c_callback">Callback that is executed after the dialog completes</param>
        /// <returns>True if succeeded</returns>
        public static bool FileSave(string c_title, string c_startPath, string c_extension, Action<string> c_callback)
        {
            var a_dialog = new SaveFileDialog { Title = c_title, InitialDirectory = c_startPath, DefaultExt = @"*.*", Filter = c_extension, CheckPathExists = true, OverwritePrompt = true };
            if (a_dialog.ShowDialog() == DialogResult.OK)
            {
                c_callback?.Invoke(a_dialog.FileName);
            }
            else
            {
                c_callback?.Invoke(string.Empty);
                return false;
            }

            return true;
        }
    }
}