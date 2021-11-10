using BackgroundTiles.Buildings;
using FUtility;
using HarmonyLib;
using System.Linq;

namespace BackgroundTiles.Patches
{
    class GeneratedBuildingsPatch
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                BackgroundTilesRegistry.SetBaseTemplate();
            }

            public static void Postfix()
            {
                var items = from tiles in BackgroundTilesRegistry.tiles select tiles.Key.Tag.ToString();

                // add a category to put the backwalls in
                PlanScreen.PlanInfo planInfo = new PlanScreen.PlanInfo(new HashedString(Mod.BackwallCategory), false, items.ToList());
                TUNING.BUILDINGS.PLANORDER.Add(planInfo);
            }
        }
    }
}
