using Lean.Localization;
using UnityEngine;

namespace DD.Utils
{
    public static class Localizator
    {
        public static string GetLocalizedText(string translationName, params object[] args)
        {
            if (string.IsNullOrEmpty(translationName))
                return translationName;

            string localizedText = LeanLocalization.GetTranslationText(translationName);

            if (string.IsNullOrEmpty(localizedText))
            {
                Debug.LogWarning($"Localization key '{translationName}' not found, using key as fallback");
                return translationName;
            }

            if (args != null && args.Length > 0)
            {
                try
                {
                    localizedText = string.Format(localizedText, args);
                }
                catch (System.FormatException)
                {
                    Debug.LogError($"Format error for key '{translationName}' with args: {string.Join(", ", args)}");
                    return localizedText;
                }
            }

            return localizedText;
        }

        public static string GetLocalizedTextWithFallback(string translationName, string fallbackText, params object[] args)
        {
            if (string.IsNullOrEmpty(translationName))
            {
                return fallbackText;
            }

            string localizedText = LeanLocalization.GetTranslationText(translationName);

            if (string.IsNullOrEmpty(localizedText))
            {
                Debug.LogWarning($"Localization key '{translationName}' not found, using key as fallback");
                return string.Format(fallbackText, args);
            }

            if (args != null && args.Length > 0)
            {
                try
                {
                    localizedText = string.Format(localizedText, args);
                }
                catch (System.FormatException)
                {
                    Debug.LogError($"Format error for key '{translationName}' with args: {string.Join(", ", args)}");
                    return string.Format(fallbackText, args);
                }
            }

            return localizedText;
        }
    }
}