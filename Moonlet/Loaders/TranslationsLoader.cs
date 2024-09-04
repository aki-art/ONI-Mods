using Moonlet.TemplateLoaders;
using System;
using System.Collections.Generic;
using System.IO;

namespace Moonlet.Loaders
{
	public class TranslationsLoader() : ContentLoader("translations")
	{
		public Dictionary<string, TranslationLoader> translations = [];

		public void Add(string modId, string key, string value)
		{
			if (!translations.ContainsKey(modId))
				translations.Add(modId, new TranslationLoader(modId));

			translations[modId].Add(key, value);
		}

		public void RegisterAll()
		{
			Log.Debug("Loading translations...");
			foreach (var translationMod in translations.Values)
				translationMod.RegisterStrings();
		}

		public void LoadTranslations(string languageCode)
		{
			foreach (var mod in MoonletMods.Instance.moonletMods.Values)
			{
				if (!mod.data.StringsOverloadType.IsNullOrWhiteSpace())
				{
					var rootType = Type.GetType(mod.data.StringsOverloadType);
					if (rootType != null)
					{
						Localization.RegisterForTranslation(rootType);
						LocString.CreateLocStringKeys(rootType, null);

						Log.Debug($"Loaded strings into {mod.data.StringsOverloadType}.");
					}

					else Log.Warn($"{mod.data.StringsOverloadType} is null.", mod.staticID);
				}

				var filePath = Path.Combine(mod.GetTranslationsPath(), languageCode + ".po");

				if (File.Exists(filePath))
				{
					Log.Info($"Loaded {languageCode} translation for {mod.staticID}");
					var dict = Localization.LoadStringsFile(filePath, false);

					if (dict != null)
					{
						foreach (var key in dict)
							Strings.Add(key.Key, key.Value);
					}

					Localization.OverloadStrings(dict); // TODO: do i need this???
				}
			}
		}
	}
}
