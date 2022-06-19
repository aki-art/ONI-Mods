using FUtility;
using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;
using SpookyPumpkin.Settings;

namespace SpookyPumpkin
{
    public class Mod : UserMod2
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
            var pOptions = new POptions();
            pOptions.RegisterOptions(this, typeof(SPSettings));
            Config = new SPSettings();

            var newOptions = POptions.ReadSettings<SPSettings>();
            if (newOptions != null)
            {
                Config = newOptions;
            }
        }
    }
}
