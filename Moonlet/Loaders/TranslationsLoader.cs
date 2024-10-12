using Moonlet.TemplateLoaders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

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
			Log.Debug("AAA LoadTranslations");
			foreach (var mod in MoonletMods.Instance.moonletMods.Values)
			{
				var rootTypes = new List<Type>();

				if (mod.data.StringsOverloadTypes != null)
				{
					foreach (var type in mod.data.StringsOverloadTypes)
					{
						var rootType = Type.GetType(type);
						if (rootType == null)
							Log.Warn($"StringsOverloadType was defined but {type} does not exist.", mod.staticID);
					}

				}

				foreach (var rootType in rootTypes)
					Localization.RegisterForTranslation(rootType);

				var filePath = Path.Combine(mod.GetTranslationsPath(), languageCode + ".po");

				if (File.Exists(filePath))
				{
					Log.Info($"Loaded {languageCode} translation for {mod.staticID}");

					var dict = Localization.LoadStringsFile(filePath, false);

					if (dict == null)
						return;

					var loadedStrings = new HashSet<string>();

					foreach (var rootType in rootTypes)
						OverLoadType(mod, dict, loadedStrings, rootType);

					foreach (var entry in dict)
					{
						if (!loadedStrings.Contains(entry.Key))
						{
							Log.Debug($"additional string entry: {entry.Key} {entry.Value}");
							Strings.Add(entry.Key, entry.Value);
						}
					}
				}
			}
		}

		private static void OverLoadType(MoonletMod mod, Dictionary<string, string> dict, HashSet<string> loadedStrings, Type rootType)
		{
			string errors = "", linkErrors = "", linkCountErrors = "";
			var path = $"{rootType.Namespace}.{rootType.Name}";

			OverloadStrings(dict, path, rootType, ref errors, ref linkErrors, ref linkCountErrors, loadedStrings);

			Log.Debug($"Loaded strings into {mod.data.StringsOverloadTypes}.");

			LocString.CreateLocStringKeys(rootType, null);
		}

		public static void OverloadStrings(Dictionary<string, string> translatedStrings, string path, Type rootType, ref string parameterErrors, ref string linkErrors, ref string linkCountErrors, HashSet<string> loadedStrings)
		{
			var fields = rootType.GetFields();

			foreach (FieldInfo fieldInfo in fields)
			{
				if (fieldInfo.FieldType != typeof(LocString))
					continue;

				var stringKey = path + "." + fieldInfo.Name;
				loadedStrings.Add(stringKey);

				if (translatedStrings.TryGetValue(stringKey, out string value))
				{
					var locString = (LocString)fieldInfo.GetValue(null);
					var value2 = new LocString(value, stringKey);

					if (!Localization.AreParametersPreserved(locString.text, value))
						parameterErrors = parameterErrors + "\t" + stringKey + "\n";
					else if (!Localization.HasSameOrLessLinkCountAsEnglish(locString.text, value))
						linkCountErrors = linkCountErrors + "\t" + stringKey + "\n";
					else if (!Localization.HasMatchingLinkTags(value))
						linkErrors = linkErrors + "\t" + stringKey + "\n";
					else
						fieldInfo.SetValue(null, value2);
				}
			}

			var nestedTypes = rootType.GetNestedTypes();
			foreach (Type type in nestedTypes)
			{
				string path2 = path + "." + type.Name;
				OverloadStrings(translatedStrings, path2, type, ref parameterErrors, ref linkErrors, ref linkCountErrors, loadedStrings);
			}
		}

	}
}
