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

            if(TranslationExists(out string path))
                LoadStrings(path);

            LocString.CreateLocStringKeys(root, null);

            if(generateTemplate)
                GenerateStringsTemplate(root, Path.Combine(Manager.GetDirectory(), "strings_templates"));
        }

        private static bool TranslationExists(out string path)
        {
            var locale = GetLocale();
            path = locale != null ?
                Path.Combine(Utils.ModPath, "translations", locale.Code + ".po") :
                null;

            return path != null;
        }

        private static void LoadStrings(string path)
        {
            if (File.Exists(path))
                OverloadStrings(LoadStringsFile(path, false));
        }
    }
}
