using FUtility.FUI;
using System.Collections.Generic;
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
			//Fonts.simplyMono = bundle.LoadAsset<TMP_FontAsset>("Assets/Moonlet/Fonts/SimplyMono-Book SDF.asset");

			TMP_FontAsset roboto = null;
			var fonts = new List<TMP_FontAsset>(Resources.FindObjectsOfTypeAll<TMP_FontAsset>());
			foreach(var font in fonts)
			{
				Log.Debug("font. " + font.name);
				if(font.name == "RobotoCondensed-Regular")
				{
					roboto = font;
					Log.Debug("found roboto");
				}
			}

			if (roboto == null) Log.Warn("Roboto is null");

			TMPConverter.ReplaceAllText(Prefabs.devConsolePrefab, fontLookup: new()
			{
				{
					"SimplyMono-Book SDF", roboto
				}
			});
		}
	}
}
