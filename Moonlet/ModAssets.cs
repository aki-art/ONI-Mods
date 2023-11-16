using FUtility.FUI;
using HarmonyLib;
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


			MoonletMods.Instance.moonletMods.Do(mod =>
			{
				LoadBundles(mod.Value, FileUtil.delimiter);
			});

			Mod.zoneTypesLoader.ApplyToActiveTemplates(z => z.OnAssetsLoaded());
		}

		private static void LoadBundles(MoonletMod mod, string[] delimiter)
		{
			if (mod.data.LoadAssetBundles == null)
				return;

			foreach (var path in mod.data.LoadAssetBundles)
			{
				Log.Debug("loading assetbundle: " + path);

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

				Log.Debug(absolutePath);
				var bundleName = Path.GetFileName(absolutePath);

				if (ModAssets.GetBundle(bundleName) != null)
					continue;

				if (!File.Exists(absolutePath))
					Log.Warn($"File not found: {absolutePath}", mod.staticID);

				if (AssetBundle.LoadFromFile(path) == null)
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
