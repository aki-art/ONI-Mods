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
                ModUtil.AddBuildingToPlanScreen(Consts.BUILD_CATEGORY.FURNITURE, BackwallConfig.ID, Consts.SUB_BUILD_CATEGORY.Furniture.LIGHTS, FloorLampConfig.ID);
            }

            public static void Postfix()
            {
                BackwallVariant.InitDefaultMaterial();

                foreach(var def in Assets.BuildingDefs)
                {
                    if(def.BlockTileAtlas != null)
                    {
                        Mod.variants.Add(new BackwallVariant(def));
                    }
                }
            }
        }
    }
}
