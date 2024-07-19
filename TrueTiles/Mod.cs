using FUtility;
using FUtility.SaveData;
using HarmonyLib;
using KMod;
using Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using TrueTiles.Patches;
using TrueTiles.Settings;
using static Rendering.BlockTileRenderer;
using Directory = System.IO.Directory;

namespace TrueTiles
{
	public class Mod : UserMod2
	{
		private const string ADDON_PATH = "true_tiles_addon";
		private static SaveDataManager<Config> config;
		public static Harmony harmonyInstance;
		public static HashSet<string> moddedPacksPaths;
		public static HashSet<string> moddedPackIds;
		public static Config Settings => config.Settings;

		public static string GetExternalSavePath() => Path.Combine(Util.RootFolder(), "mods", "tile_texture_packs");

		public static string GetLocalSavePath() => Path.Combine(ModPath, "tiles");

		public static string ModPath { get; private set; }

		public static List<string> addonPacks;

		// mods can call this with reflection to register any extra packs. 
		// make sure you do it BEFORE Db.Init. OnAllModsLoaded is the recommended entry point.
		public static void AddPack(string pack)
		{
			if (addonPacks == null)
				addonPacks = new List<string>();

			addonPacks.Add(pack);
		}

		public override void OnLoad(Harmony harmony)
		{
			ModPath = Utils.ModPath;
			config = new SaveDataManager<Config>(path);
			Setup();
			GenerateData(path);
			harmonyInstance = harmony;

			Log.PrintVersion(this);

			base.OnLoad(harmony);
		}

		public static void SaveConfig() => config.Write(true);

		public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
		{
		}

		private void Setup()
		{
			BlockTileRendererPatch.GetRenderInfoLayerMethod = AccessTools.Method(typeof(BlockTileRenderer), "GetRenderInfoLayer", new Type[] { typeof(bool), typeof(SimHashes) });
			BlockTileRendererPatch.GetRenderLayerForTileMethod = AccessTools.Method(typeof(BlockTileRendererPatch), "GetRenderLayerForTile", new Type[] { typeof(RenderInfoLayer), typeof(BuildingDef), typeof(SimHashes) });
		}

		private void GenerateData(string path)
		{
#if DATAGEN
			var root = FileUtil.GetOrCreateDirectory(Path.Combine(path, "true_tiles_addon"));
			new Datagen.PackDataGen(root);
			new Datagen.TileDataGen(root);
#endif
		}

		public static void ScanOtherMods()
		{
			moddedPacksPaths = [];
			moddedPackIds = [];

			foreach (var mod in Global.Instance.modManager.mods)
			{
				var folder = Path.Combine(mod.ContentPath, ADDON_PATH);
				if (Directory.Exists(folder))
				{
					foreach (var possiblePackFolder in Directory.GetDirectories(folder))
					{
						Log.Info($"Loading a pack from {mod.staticID} {possiblePackFolder}");
						AddPack(possiblePackFolder);
						moddedPacksPaths.Add(mod.ContentPath);
						moddedPackIds.Add(mod.staticID);
					}
				}
			}
		}
	}
}
