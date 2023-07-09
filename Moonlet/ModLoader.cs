using FUtility;
using System.IO;

namespace Moonlet
{
	public class ModLoader
	{
		public MoonletData data;
		public string path;
		public string title;
		public string staticID;

		public ModLoader(KMod.Mod mod, MoonletData data)
		{
			this.data = data;
			staticID = mod.staticID;
			title = mod.title;
			path = mod.ContentPath;
			Log.Info($"Initializing Moonlet mod {mod.title} ({mod.staticID})");
		}

		public void OnAllModsLoaded()
		{
		}

		public void LoadTranslations()
		{
			var translationsFolder = Path.Combine(path, data.TranslationsPath);

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
