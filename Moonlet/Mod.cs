using FUtility;
using HarmonyLib;
using KMod;
using Moonlet.Loaders;
using Moonlet.MoonletDevTools;
using Moonlet.Patches;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Moonlet
{
	public class Mod : UserMod2
	{
		public const string FILENAME = "moonlet_settings.yaml";
		public const string DEFAULT_FOLDER = "moonlet";

		public static List<ModLoader> modLoaders = new();
		public static SharedElementsLoader sharedElementsLoader;
		public static SharedZoneTypeLoader sharedZoneTypeLoader;
		public static HashSet<string> loadedModIds = new();

		public override void OnLoad(Harmony harmony)
		{
#if DEBUG
			Harmony.DEBUG = true;
#endif
			base.OnLoad(harmony);
			Log.PrintVersion(this);
		}

		public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
		{
			base.OnAllModsLoaded(harmony, mods);

			loadedModIds = mods.Select(mod => mod.staticID).ToHashSet();

			foreach (var mod in mods)
			{
				if (!mod.IsEnabledForActiveDlc())
					continue;

				var folder = mod.ContentPath;
				var moonletConfigPath = Path.Combine(folder, FILENAME);
				var configExists = File.Exists(moonletConfigPath);

				// there is user configuration, load from that
				if (configExists)
					LoadMod(mod, FileUtil.Read<MoonletData>(moonletConfigPath));
				else
				{
					// try loading default configuration
					var moonletFolder = Path.Combine(folder, DEFAULT_FOLDER);

					if (System.IO.Directory.Exists(moonletFolder))
						LoadMod(mod, new MoonletData());
				}
			}

			if (PatchTracker.loadsElements)
				ConditionalPatches.PatchElements(harmony);

			if (PatchTracker.loadsElements || PatchTracker.loadsZoneTypes)
				ConditionalPatches.PatchEnums(harmony);

			DevToolManager.Instance.RegisterDevTool<ConsoleDevTool>("Moonlet/Console");
			DevToolManager.Instance.RegisterDevTool<ZoneTypeDevTool>("Moonlet/Zonetypes");
			DevToolManager.Instance.RegisterDevTool<NoisePreviewDevTool>("Moonlet/Noise Preview");
		}

		private void LoadMod(KMod.Mod mod, MoonletData moonletData)
		{
			var modLoader = new ModLoader(mod, moonletData);
			modLoaders.Add(modLoader);
			modLoader.OnAllModsLoaded();

			sharedElementsLoader = new SharedElementsLoader();
			sharedElementsLoader.PreLoadYamls();

			sharedZoneTypeLoader = new SharedZoneTypeLoader();
			sharedZoneTypeLoader.PreLoad();
		}

		public static void AddStrings(string key, string value)
		{
			Strings.Add(key, value);
			ModLoader.locstringKeys.Add(key, value);
		}
	}
}
