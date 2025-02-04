using Moonlet.Utils;
using System.Collections.Generic;

namespace Moonlet
{
	public class MoonletData
	{
		public bool DebugLogging { get; set; } = true;

		[Doc("Intended for legacy uses of ID-s without the : prefix of the mod ID. For example, moving an existing mod to Moonlet and wanting to" +
			"not fiddle with worldgen files.")]
		public bool DisableIDPrefix { get; set; }

		public bool GenerateTranslationsTemplate { get; set; } = true;

		public string TranslationsPath { get; set; } = "moonlet/lang";

		public string AssetsPath { get; set; } = "moonlet/assets";

		public string DataPath { get; set; } = "moonlet/data";

		public string ModColor { get; set; }

		public string FlareIcon { get; set; }

		public string[] StringsOverloadTypes { get; set; } = null;

		public List<string> LoadAssetBundles { get; set; }

		public List<string> LoadFMODBanks { get; set; }
	}
}
