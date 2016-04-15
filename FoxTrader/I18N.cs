using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using FoxTrader.Properties;

namespace FoxTrader
{
    static class I18N
    {
        private static CultureInfo m_currentCulture;
        private static CultureInfo m_currentUICulture;

        private static Dictionary<string, ResourceManager> m_installedLanguages;

        public static string[] InstalledLanguages => m_installedLanguages?.Keys.ToArray() ?? new[] { "error" };

        public static string CurrentLanguage => m_currentUICulture.Name;

        public static int CurrentLanguageIndex => Array.IndexOf(InstalledLanguages, m_currentUICulture.Name);

        public static CultureInfo CurrentCulture => m_currentCulture;

        public static CultureInfo CurrentUICulture => m_currentUICulture;

        public static void Initialize()
        {
            m_installedLanguages = new Dictionary<string, ResourceManager> { { "en-US", Resources.ResourceManager } };

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

            var a_executingAssembly = Assembly.GetExecutingAssembly();
            var a_resourceNames = a_executingAssembly.GetManifestResourceNames();

            foreach (var a_resName in a_resourceNames)
            {
                if (a_resName.Contains("qps-PLOC"))
                {
                    m_installedLanguages.Add("qps-ploc", new ResourceManager(a_resName.Replace(".resources", ""), a_executingAssembly));

                    continue;
                }

                if (a_resName.Contains(".resources.dll"))
                {
                    var a_namedParts = a_resName.Split('.');
                    var a_languageId = a_namedParts[1];

                    var a_languageAssembly = a_executingAssembly.GetSatelliteAssembly(new CultureInfo(a_languageId));

                    m_installedLanguages.Add(a_languageId, new ResourceManager($"FoxTrader.Properties.Resources.{a_languageId}", a_languageAssembly));
                }
            }

            m_currentCulture = CultureInfo.CurrentCulture;
            m_currentUICulture = CultureInfo.CurrentUICulture;

#if DEBUG
            SetUICulture(Constants.kDefaultDebugLanguage);
#endif
        }

        public static void SetAllCulture(string c_newCultureString)
        {
            SetAllCulture(new CultureInfo(c_newCultureString));
        }

        public static void SetCulture(string c_newCultureString)
        {
            SetCulture(new CultureInfo(c_newCultureString));
        }

        public static void SetUICulture(string c_newCultureString)
        {
            SetUICulture(new CultureInfo(c_newCultureString));
        }

        public static void SetAllCulture(CultureInfo c_newCultureInfo)
        {
            m_currentCulture = c_newCultureInfo;
            m_currentUICulture = c_newCultureInfo;
        }

        public static void SetCulture(CultureInfo c_newCultureInfo)
        {
            m_currentCulture = c_newCultureInfo;
        }

        public static void SetUICulture(CultureInfo c_newCultureInfo)
        {
            m_currentUICulture = c_newCultureInfo;
        }

        public static string GetString(string c_textKey)
        {
            var a_translatedString = GetCurrentUIResourceManager().GetString(c_textKey);

            if (a_translatedString == null)
            {

                a_translatedString = m_installedLanguages[CultureInfo.DefaultThreadCurrentCulture.Name].GetString(c_textKey, CultureInfo.DefaultThreadCurrentUICulture);
            }

            return a_translatedString;
        }

        internal static object GetObject(string c_objectKey)
        {
            return GetCurrentUIResourceManager().GetObject(c_objectKey);
        }

        private static ResourceManager GetCurrentUIResourceManager()
        {
            ResourceManager a_resourceManager;

            if (m_installedLanguages.ContainsKey(m_currentUICulture.Name))
            {
                a_resourceManager = m_installedLanguages[m_currentUICulture.Name];
            }
            else
            {
                a_resourceManager = m_installedLanguages[CultureInfo.DefaultThreadCurrentUICulture.Name];
            }

            return a_resourceManager;
        }

        private static ResourceManager GetCurrentResourceManager()
        {
            ResourceManager a_resourceManager;

            if (m_installedLanguages.ContainsKey(m_currentCulture.Name))
            {
                a_resourceManager = m_installedLanguages[m_currentCulture.Name];
            }
            else
            {
                a_resourceManager = m_installedLanguages[CultureInfo.DefaultThreadCurrentCulture.Name];
            }

            return a_resourceManager;
        }
    }
}
