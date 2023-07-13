using FUtility;
using System.Collections.Generic;
using System.IO;

namespace Moonlet.Loaders
{
	public class ModTranslationsLoader : BaseLoader
	{
		public string TranslationsFolder => Path.Combine(path, data.TranslationsPath);

		public ModTranslationsLoader(KMod.Mod mod, MoonletData data) : base(mod, data)
		{
		}

		public void LoadTranslations()
		{
			var keysFile = Path.Combine(TranslationsFolder, KEYS);

			var defaultKeys = FileUtil.Read<Dictionary<string, string>>(keysFile);
			if (defaultKeys != null)
			{
				foreach (var key in defaultKeys)
					Strings.Add(key.Key, key.Value);
			}

			var translationsFolder = TranslationsFolder;

			if (File.Exists(translationsFolder))
			{
				if (LoadStrings(translationsFolder))
					Log.Debuglog($"Loaded translations for {staticID}");
			}
		}

		// Loads user created translations
		private static bool LoadStrings(string translationsPath)
		{
			var code = Localization.GetLocale()?.Code;

			if (code.IsNullOrWhiteSpace())
				code = "en";

			var path = Path.Combine(translationsPath, "translations", code + ".po");

			if (File.Exists(path))
			{
				Localization.OverloadStrings(Localization.LoadStringsFile(path, false));
				Log.Info($"Loaded translation file for {code}.");

				return true;
			}

			return false;
		}
	}
}
