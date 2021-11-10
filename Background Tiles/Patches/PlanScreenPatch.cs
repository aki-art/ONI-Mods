using BackgroundTiles.Buildings;
using FUtility;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace BackgroundTiles.Patches
{
    [HarmonyPatch(typeof(PlanScreen), "OnPrefabInit")]
    public static class PlanScreen_OnPrefabInit_Patch
    {
        public static void Prefix(Dictionary<HashedString, string> ___iconNameMap)
        {
            Log.Debuglog("Planscreen Onprefabinit happens");
            /*
            var items = from tiles in BackgroundTilesRegistry.tiles select tiles.Key.Tag.ToString();
            // add a category to put the backwalls in
            PlanScreen.PlanInfo planInfo = new PlanScreen.PlanInfo(new HashedString(Mod.BackwallCategory), false, items.ToList());
            TUNING.BUILDINGS.PLANORDER.Add(planInfo);
            */

            // register icon
            //if(HashCache.Get().Get((HashedString)Mod.BackwallCategory) is null) {
            //}
        }
    }
}
