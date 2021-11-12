using HarmonyLib;

namespace BackgroundTiles.Patches
{
    public class GeneratedBuildingsPatch
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                BackgroundTilesManager.Instance.SetBaseTemplate();
            }

            public static void Postfix()
            {
                // add a category to put the backwalls in
                PlanScreen.PlanInfo planInfo = new PlanScreen.PlanInfo(
                    new HashedString(Mod.BackwallCategory),
                    false,
                    BackgroundTilesManager.Instance.GetTileIDs());

                TUNING.BUILDINGS.PLANORDER.Add(planInfo);

            }
        }
    }
}
