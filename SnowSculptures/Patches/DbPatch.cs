using FUtility;
using HarmonyLib;
using SnowSculptures.Content;
using SnowSculptures.Content.Buildings;

namespace SnowSculptures.Patches
{
    public class DbPatch
    {
        [HarmonyPatch(typeof(Db), "Initialize")]
        public class Db_Initialize_Patch
        {
            public static void Postfix()
            {
                SnowSculptureConfig.RegisterArtableStages(Db.Get().ArtableStages);
                SnowStatusItems.CreateStatusItems();

                ModUtil.AddBuildingToPlanScreen(Consts.BUILD_CATEGORY.FURNITURE, SnowSculptureConfig.ID, Consts.SUB_BUILD_CATEGORY.Furniture.SCULPTURE, IceSculptureConfig.ID);
                ModUtil.AddBuildingToPlanScreen(Consts.BUILD_CATEGORY.UTILITIES, GlassCaseConfig.ID, Consts.SUB_BUILD_CATEGORY.Utilities.TEMPERATURE);
                ModUtil.AddBuildingToPlanScreen(Consts.BUILD_CATEGORY.FURNITURE, SnowMachineConfig.ID);

                BuildingUtil.AddToResearch(SnowSculptureConfig.ID, Consts.TECH.DECOR.ARTISTRY);
                BuildingUtil.AddToResearch(GlassCaseConfig.ID, Consts.TECH.DECOR.ARTISTRY);
                BuildingUtil.AddToResearch(SnowMachineConfig.ID, Consts.TECH.DECOR.INTERIOR_DECOR);

                ConfigureRecipes();

                ModAssets.LoadAssets();
            }

            private static void ConfigureRecipes()
            {
                var desc = string.Format(
                    global::STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.LIME_RECIPE_DESCRIPTION, 
                    SimHashes.Ice.CreateTag().ProperName(), 
                    SimHashes.Snow.CreateTag().ProperName());
                
                RecipeBuilder.Create(RockCrusherConfig.ID, desc, 20f)
                    .Input(SimHashes.Ice.CreateTag(), 100f)
                    .Output(SimHashes.Snow.CreateTag(), 100f)
                    .Build();
            }
        }
    }
}
