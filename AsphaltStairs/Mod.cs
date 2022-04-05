using HarmonyLib;
using KMod;
using System.Collections.Generic;

namespace AsphaltStairs
{
    public class Mod : UserMod2
    {
        public static readonly Tag stairsTag = TagManager.Create("Stairs");

        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
        {
            base.OnAllModsLoaded(harmony, mods);
            HarmonyPatches.harmony = harmony;
        }
    }
}
