global using FUtility;
using Backwalls.Buildings;
using Backwalls.Cmps;
using Backwalls.Settings;
using FUtility.SaveData;
using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
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
		public static float z;
		private static SaveDataManager<Config> config;
		public static string configFolder;

		public static Config Settings => config.Settings;

		public static void SaveSettings()
		{
			config.Write();
			UpdateLayerZ();
		}

		public override void OnLoad(Harmony harmony)
		{
			base.OnLoad(harmony);

			Log.PrintVersion(this);
			configFolder = Utils.ConfigPath(mod.staticID);
			config = new SaveDataManager<Config>(configFolder);

			PUtil.InitLibrary();
			BWActions.Register();

#if DEVTOOLS
			RegisterDevTool<BackwallsDevtool>("Mods/Backwalls");
#endif
		}

#if DEVTOOLS

		private static MethodInfo m_RegisterDevTool;

		public static void RegisterDevTool<T>(string path) where T : DevTool, new()
		{
			if (m_RegisterDevTool == null)
			{
				m_RegisterDevTool = AccessTools.DeclaredMethod(typeof(DevToolManager), "RegisterDevTool", new[]
				{
					typeof(string)
				});

				if (m_RegisterDevTool == null)
				{
					Log.Warning("DevToolManager.RegisterDevTool couldnt be found, skipping adding dev tools.");
					return;
				}

				m_RegisterDevTool
					.MakeGenericMethod(typeof(T))
					.Invoke(DevToolManager.Instance, new object[] { path });
			}
		}
#endif

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

			UpdateLayerZ();
		}

		public static void UpdateLayerZ()
		{
			switch (Settings.Layer)
			{
				case Config.WallLayer.Automatic:
					// this mod doesn't have a static ID
					var drywallMod = Type.GetType("DrywallHidesPipes.DrywallPatch, DrywallHidesPipes", false, false) != null;
					sceneLayer = drywallMod ? Grid.SceneLayer.InteriorWall : Grid.SceneLayer.Backwall;
					z = drywallMod ? -16 : -1;
					break;
				case Config.WallLayer.HidePipes:
					sceneLayer = Grid.SceneLayer.InteriorWall;
					z = -16;
					break;
				case Config.WallLayer.BehindPipes:
				default:
					sceneLayer = Grid.SceneLayer.Backwall;
					z = -1;
					break;
			}
		}
	}
}
