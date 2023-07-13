using FUtility;
using HarmonyLib;
using KMod;
using Moonlet.MoonletDevTools;
using Moonlet.Patches;
using System.Collections.Generic;
using System.IO;

namespace Moonlet
{
	public class Mod : UserMod2
	{
		public const string FILENAME = "moonlet_settings.yaml";
		public const string DEFAULT_FOLDER = "moonlet";

		public static List<ModLoader> modLoaders = new();

		public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
		{
			base.OnAllModsLoaded(harmony, mods);
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

			DevToolManager.Instance.RegisterDevTool<ZoneTypeDevTool>("Moonlet/Zonetypes");
			DevToolManager.Instance.RegisterDevTool<NoisePreviewDevTool>("Moonlet/Noise Preview");
		}

		private void LoadMod(KMod.Mod mod, MoonletData moonletData)
		{
			var modLoader = new ModLoader(mod, moonletData);
			modLoaders.Add(modLoader);
			modLoader.OnAllModsLoaded();
		}
	}
}
