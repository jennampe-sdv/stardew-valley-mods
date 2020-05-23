﻿using StardewModdingAPI;
using StardewValley;
using System.Collections.Generic;

namespace ShopTileFramework.Utility
{
    /// <summary>
    /// This class stores the current language and handles translation work
    /// </summary>
    class Translations
    {
        private static LocalizedContentManager.LanguageCode selectedLanguage;

        /// <summary>
        /// Given the english string, and then a dictionary of localized versions of the string,
        /// return the string of the current selected language
        /// Pretty much copy&pasted from Json Assets
        /// </summary>
        /// <param name="english">the english string</param>
        /// <param name="translations">each key is a language code with the value being the translated string</param>
        /// <returns>The string of the current language if available, english as a default</returns>
        public static string Localize(string english, Dictionary<string, string> translations)
        {
            if (selectedLanguage == LocalizedContentManager.LanguageCode.en)
                return english;
            if (translations == null || !translations.ContainsKey(selectedLanguage.ToString()))
                return english;
            return translations[selectedLanguage.ToString()];
        }

        /// <summary>
        /// Update the selectedLanguage to the current language on save loaded, in case it has been changed
        /// </summary>
        internal static void UpdateSelectedLanguage()
        {
            selectedLanguage = LocalizedContentManager.CurrentLanguageCode;
            ModEntry.monitor.Log($"Updating current language settings: {selectedLanguage}", ModEntry.VerboseLogging ? LogLevel.Debug : LogLevel.Trace);
        }
    }
}