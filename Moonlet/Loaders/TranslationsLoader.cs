using System.Collections.Generic;

namespace Moonlet.Loaders
{
	public class TranslationsLoader() : ContentLoader("translations")
	{
		public Dictionary<string, string> translations = new();

		public void Add(string key, string value)
		{
			translations.Add(key, value);
		}

		public void RegisterAll()
		{
			foreach (var translation in translations)
				Strings.Add(translation.Key, translation.Value);

			translations.Clear();
		}
	}
}
