using FUtility;
using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;
using SpookyPumpkinSO.Settings;

namespace SpookyPumpkinSO
{
    public class SpookyPumpkinMod : UserMod2
	{
		public static SPSettings Config { get; private set; }

		public override void OnLoad(Harmony harmony)
		{
			GameTags.MaterialBuildingElements.Add(ModAssets.buildingPumpkinTag);

			base.OnLoad(harmony);

			ModAssets.ModPath = path;
			ModAssets.ReadPipTreats();

			Log.PrintVersion();

			PUtil.InitLibrary(false);
			POptions pOptions = new POptions();
			pOptions.RegisterOptions(this, typeof(SPSettings));
			Config = new SPSettings();

			var newOptions = POptions.ReadSettings<SPSettings>();
			if (newOptions != null) Config = newOptions;
		}
	}
}
