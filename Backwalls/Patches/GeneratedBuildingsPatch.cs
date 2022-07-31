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
                ModUtil.AddBuildingToPlanScreen(Consts.BUILD_CATEGORY.FURNITURE, DecorativeBackwallConfig.ID, Consts.SUB_BUILD_CATEGORY.Furniture.LIGHTS, FloorLampConfig.ID);
                ModUtil.AddBuildingToPlanScreen(Consts.BUILD_CATEGORY.UTILITIES, SealedBackwallConfig.ID, Consts.SUB_BUILD_CATEGORY.Utilities.OTHER_UTILITIES, ExteriorWallConfig.ID);

                BuildingUtil.AddToResearch(SealedBackwallConfig.ID, Consts.TECH.EXOSUITS.SUITS);
            }

            public static void Postfix()
            {
                BackwallPattern.InitDefaultMaterial();

                foreach (var def in Assets.BuildingDefs)
                {
                    if (def.BlockTileAtlas != null && !def.BuildingComplete.HasTag(ModAssets.Tags.noBackwall))
                    {
                        Mod.variants.Add(new BackwallPattern(def));
                    }
                }

                // Adds a solid color variant
                var sprite = Assets.GetBuildingDef(ExteriorWallConfig.ID).GetUISprite();
                var solidColor = new BackwallPattern("BlankPattern", "Solid Color", ModAssets.blankTileTex, sprite, 999);
                solidColor.biomeTint = 0;
                Mod.variants.Add(solidColor);
            }
        }
    }
}
