global using FUtility;
using DecorPackA.Buildings;
using DecorPackA.Patches;
using DecorPackA.Settings;
using FUtility.SaveData;
using HarmonyLib;
using KMod;
using System.Collections.Generic;
using System.IO;

namespace DecorPackA
{
	public class Mod : UserMod2
	{
		public const string PREFIX = "DecorPackA_";
		public static SaveDataManager<Config> config;
		public static Components.Cmps<FacadeRestorer> facadeRestorers = new();
		public static Harmony harmonyInstance;

		public static Config Settings => config.Settings;

		public override void OnLoad(Harmony harmony)
		{
			harmonyInstance = harmony;
			config = new SaveDataManager<Config>(Path.Combine(Manager.GetDirectory(), "config", "decorpacki"));

			if (Settings.GlassTile.UseDyeTC)
				AdditionalDetailsPanelPatch.Patch(harmony);

			base.OnLoad(harmony);

			Log.PrintVersion(this);

			Utils.RegisterDevTool<DPDevTool>("Mods/Decor Pack I");
		}

		public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
		{
			base.OnAllModsLoaded(harmony, mods);
			Integration.BluePrintsMod.TryPatch(harmony);
		}
	}
}
