using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;

// TODO: compile/run only on windows

namespace FoxTrader.UI.Platform
{
    /// <summary>Windows-specific utility functions</summary>
    public static class Windows
    {
        private const string kFontRegKey = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts";

        private static Dictionary<string, string> m_fontPaths;

        /// <summary>Gets a font file path from font name</summary>
        /// <param name="c_fontName">Font name</param>
        /// <returns>Font file path</returns>
        public static string GetFontPath(string c_fontName)
        {
            // is this reliable? we rely on lazy jitting to not run win32 code on linux
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                return null;
            }

            if (m_fontPaths == null)
            {
                InitFontPaths();
            }

            // ReSharper disable once PossibleNullReferenceException
            return !m_fontPaths.ContainsKey(c_fontName) ? null : m_fontPaths[c_fontName];
        }

        private static void InitFontPaths()
        {
            // very hacky but better than nothing
            m_fontPaths = new Dictionary<string, string>();
            var a_fontsDir = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);

            var a_key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default);
            var a_subkey = a_key.OpenSubKey(kFontRegKey);
            foreach (var a_fontName in a_subkey.GetValueNames())
            {
                var a_fontFile = (string)a_subkey.GetValue(a_fontName);
                if (!a_fontName.EndsWith(" (TrueType)"))
                {
                    continue;
                }
                var a_font = a_fontName.Replace(" (TrueType)", "");
                m_fontPaths[a_font] = Path.Combine(a_fontsDir, a_fontFile);
            }
            a_key.Dispose();
        }
    }
}