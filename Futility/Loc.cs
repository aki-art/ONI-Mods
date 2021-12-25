using KMod;
using System;
using System.IO;
using static Localization;

namespace FUtility
{
    public class Loc
    {
        public static void Translate(Type root, bool generateTemplate = false)
        {
            RegisterForTranslation(root);
            LoadStrings();
            LocString.CreateLocStringKeys(root, null);

            if (generateTemplate)
            {
                GenerateStringsTemplate(root, Path.Combine(Manager.GetDirectory(), "strings_templates"));
            }
        }

        // Loads user created translations
        private static void LoadStrings()
        {
            string code = GetLocale()?.Code;

            if (code.IsNullOrWhiteSpace())
            {
                return;
            }

            string path = Path.Combine(Utils.ModPath, "translations", code + ".po");

            if (File.Exists(path))
            {
                OverloadStrings(LoadStringsFile(path, false));
                Log.Info($"Found translation file for {code}.");
            }
        }
    }
}
