global using FUtility;

using HarmonyLib;
using Klei.CustomSettings;
using KMod;
using System.Collections.Generic;
using System.Linq;

namespace SPFrightfulLands
{
    public class Mod : UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            ModAssets.LoadAll();
            Log.PrintVersion(this);
        }

        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
        {
            base.OnAllModsLoaded(harmony, mods);
            CheckSpookyPumpkin(mods);
        }

        private void CheckSpookyPumpkin(IReadOnlyList<KMod.Mod> mods)
        {
            var stairsID = "SpookyPumpkinSO";
            var stairs = mods.First(m => m.staticID == stairsID);

            if (stairs == null || !stairs.IsActive())
            {

                Log.Warning($"This mod will now force disable itself to let you restart the game without a crash.");
                mod.SetEnabledForActiveDlc(false);

                throw new DependencyMissingException(stairsID);
            }
        }
    }
}
