using FUtility;
using HarmonyLib;
using KMod;
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
				Log.Debuglog($"{folder}");

				var moonletConfigPath = Path.Combine(folder, FILENAME);

				var configExists = File.Exists(moonletConfigPath);

				// there is user configuration, load from that
				if (configExists)
					LoadMod(mod, FileUtil.Read<MoonletData>(moonletConfigPath));
				else
				{
					// try loading default configuration
					var moonletFolder = Path.Combine(folder, DEFAULT_FOLDER);

					Log.Debuglog($"moonlet: {moonletFolder}");

					if (System.IO.Directory.Exists(moonletFolder))
						LoadMod(mod, new MoonletData());
				}
			}
		}

		private void LoadMod(KMod.Mod mod, MoonletData moonletData)
		{
			var modLoader = new ModLoader(mod, moonletData);
			modLoaders.Add(modLoader);
			modLoader.OnAllModsLoaded();
		}
	}
}
