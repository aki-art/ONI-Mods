using KMod;
using System;
using System.IO;
using static Localization;

namespace FUtility
{
    public class Loc
    {
        private static bool translated = false;

        public static void Translate(Type root, bool generateTemplate = false)
        {
            RegisterForTranslation(root);
            LoadStrings();
            LocString.CreateLocStringKeys(root, null);

            if (generateTemplate)
                GenerateStringsTemplate(root, Path.Combine(Manager.GetDirectory(), "strings_templates"));
        }

        // Loads user created translations
        private static void LoadStrings()
        {
            string path = Path.Combine(Utils.ModPath, "translations", GetLocale()?.Code + ".po");
            if (File.Exists(path))
            {
                OverloadStrings(LoadStringsFile(path, false));
                translated = true;
            }
        }

        // Edits an existing STRING entry
        public static void AddOverride(string key, string newValue)
        {
            if (GetLocale() == null || translated)
                Strings.Add(key, newValue);
        }
    }
}
