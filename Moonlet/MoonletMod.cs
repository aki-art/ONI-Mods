using Moonlet.Utils;
using System.IO;

namespace Moonlet
{
	public class MoonletMod
	{
		public const string FILENAME = "moonlet_settings.yaml";
		public const string DEFAULT_FOLDER = "moonlet";

		public MoonletData data;
		public string staticID;
		public string path;
		public string title;

		public MoonletMod(KMod.Mod mod)
		{
			var moonletConfigPath = Path.Combine(mod.ContentPath, FILENAME);
			var configExists = File.Exists(moonletConfigPath);

			data = configExists ? FileUtil.ReadYaml<MoonletData>(moonletConfigPath) : new MoonletData();
			staticID = mod.staticID;
			title = mod.title;
			path = mod.ContentPath;

			if (data.DebugLogging) Log.EnableDebugLogging(mod.staticID);

			ModAssets.LoadBundles(this, FileUtil.delimiter);

			Log.Info($"Initialized Moonlet Mod", mod.staticID);
		}

		public static bool IsMoonletMod(KMod.Mod mod) => Directory.Exists(Path.Combine(mod.ContentPath, DEFAULT_FOLDER));

		public string GetDataPath(string contentPath) => Path.Combine(path, data.DataPath, contentPath);

		public string GetAssetPath(string contentPath) => Path.Combine(path, data.AssetsPath, contentPath);

		public string GetTranslationsPath() => Path.Combine(path, data.TranslationsPath);
	}
}
