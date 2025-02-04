using Moonlet.Utils;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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
		public Color color;
		public List<string> loadedBundles = [];
		public bool loadsElementTextures;

		public MoonletMod(KMod.Mod mod)
		{
			var moonletConfigPath = Path.Combine(mod.ContentPath, FILENAME);
			var configExists = File.Exists(moonletConfigPath);

			data = configExists ? FileUtil.ReadYaml<MoonletData>(moonletConfigPath) : new MoonletData();
			staticID = mod.staticID;
			title = mod.title;
			path = mod.ContentPath;

			if (data.ModColor == null)
			{
				var seed = mod.staticID.GetHashCode();
				var rand = new SeededRandom(seed);
				color = Color.HSVToRGB(rand.RandomValue(), 0.8f, 1f);
			}
			else
			{
				color = Util.ColorFromHex(data.ModColor);
			}

			if (data.DebugLogging) Log.EnableDebugLogging(mod.staticID);

			ModAssets.LoadBundles(this, FileUtil.delimiter);

			Log.Info($"Initialized Moonlet Mod", mod.staticID);
		}


		public static bool IsMoonletMod(KMod.Mod mod) => Directory.Exists(Path.Combine(mod.ContentPath, DEFAULT_FOLDER));

		public string GetDataPath(string contentPath, string dlcId)
		{
			return dlcId.IsNullOrWhiteSpace()
				? Path.Combine(path, data.DataPath, contentPath)
				: Path.Combine(path, data.DataPath, "dlc", dlcId, contentPath);

		}

		public string GetAssetPath(string contentPath) => Path.Combine(path, data.AssetsPath, contentPath);

		public string GetTranslationsPath() => Path.Combine(path, data.TranslationsPath);
	}
}
