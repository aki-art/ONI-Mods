using FUtility;
using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;
using SpookyPumpkinSO.Content.Cmps;
using SpookyPumpkinSO.Settings;

namespace SpookyPumpkinSO
{
	public class Mod : UserMod2
	{
		public static Components.Cmps<SpiceRestorer> spiceRestorers = new();
		public static Components.Cmps<FacePaint> facePaints = new();
		public static Components.Cmps<SpookyPumpkin_FacadeRestorer> facadeRestorers = new();
		public static Harmony harmonyInstance;

		public static SPSettings Config { get; private set; }

		public override void OnLoad(Harmony harmony)
		{
			harmonyInstance = harmony;
			GameTags.MaterialBuildingElements.Add(ModAssets.buildingPumpkinTag);

			base.OnLoad(harmony);

			ModAssets.ModPath = path;
			ModAssets.ReadPipTreats();

			Log.PrintVersion();

			PUtil.InitLibrary(false);

			new POptions().RegisterOptions(this, typeof(SPSettings));
			Config = POptions.ReadSettings<SPSettings>() ?? new SPSettings();
		}
	}
}
