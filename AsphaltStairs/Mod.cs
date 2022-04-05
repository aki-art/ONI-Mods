using FUtility;
using HarmonyLib;
using KMod;
using System.Collections.Generic;
using System.Linq;

namespace AsphaltStairs
{
    public class Mod : UserMod2
    {
        public static readonly Tag stairsTag = TagManager.Create("Stairs");

        public override void OnLoad(Harmony harmony)
        {
            Log.PrintVersion();
            base.OnLoad(harmony);
        }

        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
        {
            CheckIfStairsModIsHere(mods);

            base.OnAllModsLoaded(harmony, mods);

            Integration.Asphalt.TrySetSpeedModifier();
            Integration.Stairs.TryPatch(harmony);
        }

        private void CheckIfStairsModIsHere(IReadOnlyList<KMod.Mod> mods)
        {
            var stairsID = "Stairs";
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
