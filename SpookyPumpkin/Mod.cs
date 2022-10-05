using FUtility;
using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;
using SpookyPumpkinSO.Settings;
using System.Collections.Generic;
using System.Linq;

namespace SpookyPumpkinSO
{
    public class Mod : UserMod2
    {
        public static SPSettings Config { get; private set; }
        //public static bool isMorePlantsHere;

        public override void OnLoad(Harmony harmony)
        {
            GameTags.MaterialBuildingElements.Add(ModAssets.buildingPumpkinTag);

            base.OnLoad(harmony);

            ModAssets.ModPath = path;
            ModAssets.ReadPipTreats();

            Log.PrintVersion();

            PUtil.InitLibrary(false);

            new POptions().RegisterOptions(this, typeof(SPSettings));
            Config = POptions.ReadSettings<SPSettings>() ?? new SPSettings();
        }

        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
        {
            base.OnAllModsLoaded(harmony, mods);

            /*
            if (mods.Any(m => m.staticID == "sthmoreplants"))
            {
                //isMorePlantsHere = true;
                Integration.MorePlants.EntityConfigManagerPatch.Patch(harmony);
                Log.Info("Integration with More Plants initialized.");
            }
            */
        }
    }
}
