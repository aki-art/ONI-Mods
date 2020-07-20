using Harmony;
using Slag.Buildings;
using System.Collections.Generic;
using static ComplexRecipe;
using static Utils.Buildings;

namespace Slag
{
    class Patches
    {
        public static class Mod_OnLoad
        {
            public static void OnLoad(string path)
            {
            }
        }

        [HarmonyPatch(typeof(Game))]
        [HarmonyPatch("OnSpawn")]
        public static class World_OnLoadLevel_Patch
        {
            public static void Postfix()
            {
                ModAssets.miteRandom = new SeededRandom(SaveLoader.Instance.worldDetailSave.globalWorldSeed);
            }
        }

        [HarmonyPatch(typeof(EntityConfigManager), "LoadGeneratedEntities")]
        public class EntityConfigManager_LoadGeneratedEntities_Patch
        {
            public static void Prefix()
            {
                GameTags.MaterialBuildingElements.Add(ModAssets.slagWoolTag);
            }
        }

        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                var buildingsToRegister = new List<BuildingConfig>()
                {
                    new BuildingConfig(FiltrationTileConfig.ID, typeof(FiltrationTileConfig)),
                    new BuildingConfig(InsulatedManualAirlockConfig.ID, typeof(InsulatedManualAirlockConfig)),
                    new BuildingConfig(InsulatedMechanizedAirlockConfig.ID, typeof(InsulatedMechanizedAirlockConfig)),
                    new BuildingConfig(SpinnerConfig.ID, typeof(SpinnerConfig))
                };

                RegisterAllBuildings(buildingsToRegister);
            }
        }
        
        // Adding the recipes here, which is the surest way to include modded items.
        [HarmonyPatch(typeof(EquipmentConfigManager))]
        [HarmonyPatch("RegisterEquipment")]
        public static class EquipmentConfigManager_RegisterEquipment_Patch
        {
            public static void Postfix()
            {
                var cloth = Assets.Prefabs[Assets.Prefabs.Count - 1];
                SpinnerConfig.AddRecipe(new RecipeElement(cloth.PrefabID(), 1f), new RecipeElement(BasicFabricConfig.ID, 1f), "Desc", 5);
            }
        }
    }
}
