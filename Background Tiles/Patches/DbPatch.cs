using BackgroundTiles.Buildings;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace BackgroundTiles.Patches
{
    class DbPatch
    {
        [HarmonyPatch(typeof(Db), "Initialize")]
        public static class Db_Initialize_Patch
        {
            public static void Postfix()
            {
                var iconNameMap = Traverse.Create(typeof(PlanScreen)).Field<Dictionary<HashedString, string>>("iconNameMap").Value;
                iconNameMap.Add(HashCache.Get().Add(Mod.BackwallCategory), "icon_action_region_disposal");
            }
        }
    }
}
