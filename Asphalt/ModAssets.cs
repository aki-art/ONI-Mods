using FUtility;
using FUtility.FUI;
using System.IO;
using UnityEngine;

namespace Asphalt
{
	public class ModAssets
	{
		public static Texture2D bitumenTexture;

		public static string[] ROAD_SURFACE_MATERIALS = new[]
		{
			ModTags.RoadSurfaceMaterial.ToString()
		};

		public class Prefabs
		{
			public static GameObject settingsDialog;
		}

		public class Colors
		{
			public static readonly Color bitumen = new Color32(65, 65, 79, 255);
		}

		public static void LoadAssets()
		{
			var path = Path.Combine(Utils.ModPath, "assets", "solid_bitumen.png");
			bitumenTexture = FAssets.LoadTexture(path);
		}

		public static void LateLoadAssets()
		{
			AssetBundle bundle = FAssets.LoadAssetBundle("asphaltassets");

			Prefabs.settingsDialog = bundle.LoadAsset<GameObject>("SettingsDialog");
			TMPConverter.ReplaceAllText(Prefabs.settingsDialog);
		}
	}
}
