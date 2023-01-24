using CrittersDropBones.Buildings.SlowCooker;
using CrittersDropBones.Integration.SpookyPumpkin;
using CrittersDropBones.Items;

namespace CrittersDropBones.Content
{
    public class CDBRecipes
    {
        private const float WATERY_SOUP_COOK_TIME = 200f;
        private const float STOCKY_SOUP_COOK_TIME = 40f;
        private const float STOCK_COOK_TIME = Consts.CYCLE_LENGTH;

        public static void AddAllRecipes()
        {
            RecipeBuilder.Create(
                SlowCookerConfig.ID,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_GRUBGRUB.DESC,
                STOCK_COOK_TIME)
                .Input(GameTags.Water, 11)
                .Input(BoneConfig.ID, 5)
                .Output(SoupStockConfig.ID, 10);

            RecipeBuilder.Create(
                SlowCookerConfig.ID,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_GRUBGRUB.DESC,
                STOCKY_SOUP_COOK_TIME)
                .Input(SoupStockConfig.ID, 10f)
                .Input(SpiceNutConfig.ID, 10f)
                .Output(SuperHotSoupConfig.ID, 10f);

            RecipeBuilder.Create(
                SlowCookerConfig.ID,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_GRUBGRUB.DESC,
                STOCKY_SOUP_COOK_TIME)
                .Input(SoupStockConfig.ID, 10f)
                .Input(FishMeatConfig.ID, 10f)
                .Input(FishBoneConfig.ID, 5f)
                .Output(FishSoupConfig.ID, 10f);

            RecipeBuilder.Create(
                SlowCookerConfig.ID,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_GRUBGRUB.DESC,
                STOCKY_SOUP_COOK_TIME)
                .Input(SoupStockConfig.ID, 10f)
                .Input(WormBasicFruitConfig.ID, 10f)
                .Input(LettuceConfig.ID, 5f)
                .Output(GrubGrubConfig.ID, 10f);

            RecipeBuilder.Create(
                SlowCookerConfig.ID,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_SLUDGE.DESC,
                WATERY_SOUP_COOK_TIME)
                .Input(GameTags.Water, 1f)
                .Input(GameTags.SlimeMold, 10f)
                .Output(SludgeConfig.ID, 10f);

            RecipeBuilder.Create(
                SlowCookerConfig.ID,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_VEGETABLESOUP.DESC,
                WATERY_SOUP_COOK_TIME)
                .Input(GameTags.Water, 1f)
                .Input(SeaLettuceConfig.ID, 10f)
                .Input(BasicPlantFoodConfig.ID, 5f)
                .Input(MushroomConfig.ID, 10f)
                .Output(VegetableSoupConfig.ID, 10f);

            RecipeBuilder.Create(
                SlowCookerConfig.ID,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_FRUITSOUP.DESC,
                WATERY_SOUP_COOK_TIME)
                .Input(GameTags.Water, 1f)
                .Input(PrickleFruitConfig.ID, 4f)
                .Output(FruitSoupConfig.ID, 1f);

            RecipeBuilder.Create(
                SlowCookerConfig.ID,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_FRUITSOUP.DESC,
                WATERY_SOUP_COOK_TIME)
                .Input(GameTags.Water, 1f)
                .Input(ForestForagePlantConfig.ID, 4f)
                .Output(FruitSoupConfig.ID, 1f);

            RecipeBuilder.Create(
                SlowCookerConfig.ID,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_FRUITSOUP.DESC,
                WATERY_SOUP_COOK_TIME)
                .Input(GameTags.Water, 1f)
                .Input("PalmeraBerry", 4f)
                .Output(FruitSoupConfig.ID, 1f);

            RecipeBuilder.Create(
                SlowCookerConfig.ID,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_PUMPKINSOUP.DESC,
                WATERY_SOUP_COOK_TIME)
                .Input(GameTags.Water, 1f)
                .Input("SP_Pumpkin", 2f)
                .Input(BasicPlantFoodConfig.ID, 5f)
                .Output(PumpkinSoupConfig.ID, 10f);
        }
    }
}
