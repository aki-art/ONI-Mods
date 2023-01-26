using CrittersDropBones.Content.Buildings;
using CrittersDropBones.Content.Items;
using CrittersDropBones.Integration.SpookyPumpkin;

namespace CrittersDropBones.Content
{
    public class CDBRecipes
    {
        private const float WATERY_SOUP_COOK_TIME = 20f; // TODO
        private const float STOCKY_SOUP_COOK_TIME = 40f;
        private const float STOCK_COOK_TIME = Consts.CYCLE_LENGTH;

        public static void AddAllRecipes()
        {
            RecipeBuilder.Create(
                SlowCookerConfig.ID,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_SOUPSTOCK.RECIPE_DESC,
                STOCK_COOK_TIME)
                .NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
                .Input(GameTags.Water, 11)
                .Input(BoneConfig.ID, 5)
                .Output(SoupStockConfig.ID, 10)
                .SortOrder(-99)
                .Build();

            RecipeBuilder.Create(
                SlowCookerConfig.ID,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_SUPERHOTSOUP.DESC,
                STOCKY_SOUP_COOK_TIME)
                .Input(SoupStockConfig.ID, 10f)
                .Input(SpiceNutConfig.ID, 10f)
                .Input(GingerConfig.ID, 4f)
                .Output(SuperHotSoupConfig.ID, 10f)
                .Build();

            RecipeBuilder.Create(
                SlowCookerConfig.ID,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_FISHSOUP.DESC,
                STOCKY_SOUP_COOK_TIME)
                .Input(SoupStockConfig.ID, 10f)
                .Input(FishMeatConfig.ID, 10f)
                .Input(FishBoneConfig.ID, 5f)
                .Output(FishSoupConfig.ID, 10f)
                .Build();

            if (DlcManager.IsExpansion1Active())
            {
                RecipeBuilder.Create(
                    SlowCookerConfig.ID,
                    STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_GRUBGRUBGRUB.DESC,
                    STOCKY_SOUP_COOK_TIME)
                    .Input(SoupStockConfig.ID, 10f)
                    .Input(WormBasicFruitConfig.ID, 10f)
                    .Input(LettuceConfig.ID, 5f)
                    .Output(GrubGrubConfig.ID, 10f)
                    .Build();
            }

            RecipeBuilder.Create(
                SlowCookerConfig.ID,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_SLUDGE.DESC,
                WATERY_SOUP_COOK_TIME)
                .Input(GameTags.Water, 1f)
                .Input(GameTags.SlimeMold, 10f)
                .Output(SludgeConfig.ID, 10f)
                .SortOrder(10)
                .Build();

            RecipeBuilder.Create(
                SlowCookerConfig.ID,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_VEGETABLESOUP.DESC,
                WATERY_SOUP_COOK_TIME)
                .Input(GameTags.Water, 1f)
                .Input(SeaLettuceConfig.ID, 10f)
                .Input(BasicPlantFoodConfig.ID, 5f)
                .Input(MushroomConfig.ID, 10f)
                .Output(VegetableSoupConfig.ID, 10f)
                .SortOrder(10)
                .Build();

            RecipeBuilder.Create(
                SlowCookerConfig.ID,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_FRUITSOUP.DESC,
                WATERY_SOUP_COOK_TIME)
                .Input(GameTags.Water, 1f)
                .Input(PrickleFruitConfig.ID, 4f)
                .Output(FruitSoupConfig.ID, 1f)
                .SortOrder(10)
                .Build();

            RecipeBuilder.Create(
                SlowCookerConfig.ID,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_FRUITSOUP.DESC,
                WATERY_SOUP_COOK_TIME)
                .Input(GameTags.Water, 1f)
                .Input(ForestForagePlantConfig.ID, 4f)
                .Output(FruitSoupConfig.ID, 1f)
                .SortOrder(10)
                .Build();

            if (Mod.isPalmeraTreeHere)
            {
                RecipeBuilder.Create(
                SlowCookerConfig.ID,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_FRUITSOUP.DESC,
                WATERY_SOUP_COOK_TIME)
                .Input(GameTags.Water, 1f)
                .Input("PalmeraBerry", 4f)
                .Output(FruitSoupConfig.ID, 1f)
                .SortOrder(10)
                .Build();
            }

            if (Mod.isSpookyPumpkinHere)
            {
                RecipeBuilder.Create(
                SlowCookerConfig.ID,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_PUMPKINSOUP.DESC,
                WATERY_SOUP_COOK_TIME)
                .Input(GameTags.Water, 1f)
                .Input("SP_Pumpkin", 2f)
                .Input(BasicPlantFoodConfig.ID, 5f)
                .Output(PumpkinSoupConfig.ID, 10f)
                .SortOrder(10)
                .Build();
            }
        }
    }
}
