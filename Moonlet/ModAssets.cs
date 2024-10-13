using FUtility.FUI;
using Moonlet.Utils;
using System.IO;
using TMPro;
using UnityEngine;

namespace Moonlet
{
	public class ModAssets
	{
		public static class Prefabs
		{
			public static GameObject
				devConsolePrefab;
		}

		public static class Fonts
		{
			public static TMP_FontAsset simplyMono;
		}


		public static void LoadAssets()
		{
			var bundle = FUtility.Assets.LoadAssetBundle("moonletassets", platformSpecific: true);

			Prefabs.devConsolePrefab = bundle.LoadAsset<GameObject>("Assets/Moonlet/UIPrefabs/DevConsole_tmpconverted.prefab");

			TMPConverter.ReplaceAllText(Prefabs.devConsolePrefab);

			Mod.zoneTypesLoader.ApplyToActiveTemplates(z => z.OnAssetsLoaded());
		}

		public static void LoadBundles(MoonletMod mod, string[] delimiter)
		{
			if (mod.data.LoadAssetBundles == null)
				return;

			foreach (var path in mod.data.LoadAssetBundles)
			{
				var parts = path.Split(delimiter, System.StringSplitOptions.RemoveEmptyEntries);
				string absolutePath = "";

				if (parts.Length > 1)
				{
					if (parts[0] == "root")
						absolutePath = Path.Combine(mod.path, parts[1]);

				}
				else
				{
					absolutePath = Path.Combine(mod.GetAssetPath("assetbundles"), parts[0]);
				}

				absolutePath = absolutePath.Replace("{platform}", FileUtil.GetPlatformString());

				var bundleName = Path.GetFileName(absolutePath);

				if (GetBundle(bundleName) != null)
					continue;

				if (!File.Exists(absolutePath))
					Log.Warn($"File not found: {absolutePath}", mod.staticID);

				if (AssetBundle.LoadFromFile(absolutePath) == null)
					Log.Warn($"Failed to load AssetBundle from path {absolutePath}", mod.staticID);
			}
		}

		public static AssetBundle GetBundle(string name)
		{
			foreach (var bundle in AssetBundle.GetAllLoadedAssetBundles())
			{
				if (bundle.name == name)
					return bundle;
			}

			return null;
		}
	}
}
