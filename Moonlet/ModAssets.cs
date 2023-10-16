using FUtility.FUI;
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
		}
	}
}
