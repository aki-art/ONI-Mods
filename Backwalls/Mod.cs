global using FUtility;
using Backwalls.Buildings;
using Backwalls.Cmps;
using Backwalls.Settings;
using FUtility.SaveData;
using HarmonyLib;
using KMod;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Backwalls
{
	public class Mod : UserMod2
	{
		public static BackwallRenderer renderer;
		public static bool isTrueTilesHere;
		public static bool isNoZoneTintHere;
		public static Dictionary<string, BackwallPattern> variants = new Dictionary<string, BackwallPattern>();
		public static Grid.SceneLayer sceneLayer;
		private static SaveDataManager<Config> config;
		public static string configFolder;

		public static Config Settings => config.Settings;

		public static void SaveSettings()
		{
			config.Write();
		}

		public override void OnLoad(Harmony harmony)
		{
			base.OnLoad(harmony);

			Log.PrintVersion(this);
			configFolder = Utils.ConfigPath(mod.staticID);
			config = new SaveDataManager<Config>(configFolder);

			// Utils.RegisterDevTool<BackwallsDevtool>("Mods/Backwalls");
		}

		public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
		{
			base.OnAllModsLoaded(harmony, mods);

			foreach (var mod in mods)
			{
				if (mod.IsEnabledForActiveDlc())
				{
					switch (mod.staticID)
					{
						case "TrueTiles":
							isTrueTilesHere = true;
							break;
						case "NoZoneTint":
							isNoZoneTintHere = true;
							break;
					}

					if (isNoZoneTintHere && isTrueTilesHere)
					{
						break;
					}
				}
			}

			if (isTrueTilesHere)
			{
				ModAssets.uiSprites = new Dictionary<string, Sprite>();
				Integration.TrueTilesPatches.Patch(harmony);
			}

			switch (Settings.Layer)
			{
				case Config.WallLayer.Automatic:
					// this mod doesn't have a static ID
					var drywallMod = Type.GetType("DrywallHidesPipes.DrywallPatch, DrywallHidesPipes", false, false) != null;
					sceneLayer = drywallMod ? Grid.SceneLayer.LogicGatesFront : Grid.SceneLayer.Backwall;
					break;
				case Config.WallLayer.HidePipes:
					sceneLayer = Grid.SceneLayer.LogicGatesFront;
					break;
				case Config.WallLayer.BehindPipes:
				default:
					sceneLayer = Grid.SceneLayer.Backwall;
					break;
			}
		}
	}
}
