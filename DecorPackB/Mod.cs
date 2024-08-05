global using FUtility;

using DecorPackB.Content;
using DecorPackB.Settings;
using FUtility.Components;
using FUtility.SaveData;
using HarmonyLib;
using KMod;

namespace DecorPackB
{
	public class Mod : UserMod2
	{
		public const string PREFIX = "DecorPackB_";
		private static SaveDataManager<Config> config;
		public static Components.Cmps<Restorer> restorers = new();
		public static bool DebugMode = true;

		public static Config Settings => config.Settings;

		public override void OnLoad(Harmony harmony)
		{
			base.OnLoad(harmony);
			config = new SaveDataManager<Config>(Utils.ConfigPath(mod.staticID));
			GameTags.MaterialBuildingElements.Add(DPTags.buildingFossilNodule);
		}
	}
}
