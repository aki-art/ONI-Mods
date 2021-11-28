using HarmonyLib;
using System.Linq;

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

            [HarmonyPriority(Priority.High)] // Finish before other mods try to access building lists
            public static void Postfix()
            {
                BackgroundTilesManager.Instance.RegisterAll();

                var tiles = BackgroundTilesManager.Instance.tiles
                    .Where(x => !x.Value.BuildingComplete.HasTag(ModAssets.Tags.stainedGlass) || x.Value.BuildingComplete.HasTag("DecorPackA_DefaultStainedGlassTile"))
                    .Select(t => t.Key.Tag.ToString()).ToList();

                // add a category to put the backwalls in
                PlanScreen.PlanInfo planInfo = new PlanScreen.PlanInfo(
                    new HashedString(Mod.BackwallCategory),
                    false,
                    tiles);

                TUNING.BUILDINGS.PLANORDER.Add(planInfo);
            }
        }
    }
}
