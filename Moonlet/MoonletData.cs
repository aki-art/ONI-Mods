using System.Collections.Generic;

namespace Moonlet
{
	public class MoonletData
	{
		public bool DebugLogging { get; set; } = true;

		public bool GenerateTranslationsTemplate { get; set; } = true;

		public string TranslationsPath { get; set; } = "moonlet/translations";

		public string AssetsPath { get; set; } = "moonlet/assets";

		public string DataPath { get; set; } = "moonlet/data";

		public string StringsOverloadType { get; set; } = null;

		public List<string> LoadAssetBundles { get; set; }

		public List<string> LoadFMODBanks { get; set; }
	}
}
