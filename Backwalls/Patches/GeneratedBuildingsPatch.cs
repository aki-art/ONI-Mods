using Backwalls.Buildings;
using FUtility;
using HarmonyLib;

namespace Backwalls.Patches
{
    public class GeneratedBuildingsPatch
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                ModUtil.AddBuildingToPlanScreen(Consts.BUILD_CATEGORY.BASE, DecorativeBackwallConfig.ID, Consts.SUB_BUILD_CATEGORY.Base.TILES, ExteriorWallConfig.ID);
                ModUtil.AddBuildingToPlanScreen(Consts.BUILD_CATEGORY.BASE, SealedBackwallConfig.ID, Consts.SUB_BUILD_CATEGORY.Base.TILES, ExteriorWallConfig.ID);

                BuildingUtil.AddToResearch(SealedBackwallConfig.ID, Consts.TECH.EXOSUITS.SUITS);
            }

            public static void Postfix()
            {
                Log.Debuglog("Adding variants");

                foreach (var def in Assets.BuildingDefs)
                {
                    var allowed = !def.BuildingComplete.HasTag(ModAssets.Tags.noBackwall) || def.PrefabID == TileConfig.ID;
                    if (def.BlockTileAtlas != null && allowed)
                    {
                        Mod.variants[def.PrefabID] = new BackwallPattern(def);
                    }
                }

                // Adds a solid color variant
                var sprite = Assets.GetBuildingDef(ExteriorWallConfig.ID).GetUISprite();
                var solidColor = new BackwallPattern("BlankPattern", "Solid Color", ModAssets.blankTileTex, sprite, 999)
                {
                    biomeTint = 0
                };

                Mod.variants.Add("BlankPattern", solidColor);
            }
        }
    }
}
