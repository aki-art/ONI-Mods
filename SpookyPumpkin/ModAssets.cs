

using FUtility;
using FUtility.FUI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SpookyPumpkinSO
{
	public class ModAssets
	{
		public static readonly Tag buildingPumpkinTag = TagManager.Create("SP_BuildPumpkin", STRINGS.ITEMS.FOOD.SP_PUMPKIN.NAME);
		public static HashSet<Tag> pipTreats;

		public class Prefabs
		{
			public static GameObject sideScreenPrefab;
		}

		public static class Colors
		{
			public static Color
				pumpkinOrange = Util.ColorFromHex("ffa63b"),
				ghostlyColor = new Color(0, 0.8f, 1.0f),
				transparentTint = new Color(1f, 1f, 1f, 0.4f);
		}

		public static class Sounds
		{
			public const string
				CHEER = "SpookyPumpkin_Cheers",
				EVILLAUGH = "SpookyPumpkin_EvilLaugh";
		}

		public static string ModPath { get; internal set; }

		public static void LateLoadAssets()
		{
			var bundle = FAssets.LoadAssetBundle("sp_uiasset");

			Prefabs.sideScreenPrefab = bundle.LoadAsset<GameObject>("GhostPipSideScreen");
			TMPConverter.ReplaceAllText(Prefabs.sideScreenPrefab, false);

			var path = Path.Combine(Utils.ModPath, "assets");
			AudioUtil.LoadSound(Sounds.CHEER, Path.Combine(path, "333404__jayfrosting__cheer-2.wav"));
			AudioUtil.LoadSound(Sounds.EVILLAUGH, Path.Combine(path, "236982__devengarber__jacob-allen-evil-laugh.wav"));
		}

		public static List<string> ReadPipTreats()
		{
			if (ReadJSON("piptreats", out var json))
				return JsonConvert.DeserializeObject<List<string>>(json);
			else
				return new List<string>();
		}

		private static bool ReadJSON(string filename, out string json, bool log = true)
		{
			json = null;
			try
			{
				using (var r = new StreamReader(Path.Combine(ModPath, filename + ".json")))
				{
					json = r.ReadToEnd();
				}
			}
			catch (Exception e)
			{
				if (log)
				{
					Log.Warning($"Couldn't read {filename}.json, {e.Message}. Using defaults.");
				}

				return false;
			}

			return true;
		}
	}
}
