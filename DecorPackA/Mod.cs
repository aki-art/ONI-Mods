global using FUtility;
using DecorPackA.Buildings;
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
		public static Components.Cmps<FacadeRestorer> facadeRestorers = [];
		public static Harmony harmonyInstance;

		public static SaveDataManager<Config> config;
		public static Config Settings => config.Settings;

		public override void OnLoad(Harmony harmony)
		{
			harmonyInstance = harmony;

			base.OnLoad(harmony);

			Log.PrintVersion(this);

			config = new SaveDataManager<Config>(Path.Combine(Manager.GetDirectory(), "config", "decorpacki"));
		}

		public static void SaveSettings() => config.Write();

		public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
		{
			base.OnAllModsLoaded(harmony, mods);
			Integration.BluePrintsMod.TryPatch(harmony);
		}
	}
}
