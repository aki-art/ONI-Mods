using FUtility;
using Harmony;
using Klei.AI;
using System.Collections.Generic;
using TUNING;
using static ComplexRecipe;

namespace SpookyPumpkin
{
    class PumpkinPatches
    {

        [HarmonyPatch(typeof(World), "OnSpawn")]
        public static class World_OnLoad_Patch
        {

            public static void Postfix()
            {
                var prefab = Assets.GetPrefab(GhostSquirrelConfig.ID);
                Util.KInstantiate(prefab, GameUtil.GetTelepad().transform.position).SetActive(true);
            }
        }

        public static class Mod_OnLoad
        {
            public static void OnLoad(string path)
            {
                Log.PrintVersion();
                ModAssets.Initialize(path);
            }
        }

        [HarmonyPatch(typeof(Localization), "Initialize")]
        class StringLocalisationPatch
        {
            public static void Postfix()
            {
                Loc.Translate(typeof(STRINGS));
            }
        }

        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                Buildings.RegisterBuildings(typeof(SpookyPumpkinConfig));
            }
        }

        [HarmonyPatch(typeof(ModifierSet))]
        [HarmonyPatch(nameof(ModifierSet.Initialize))]
        public static class ModifierSet_Initialize_Patch
        {
            public static void Postfix(ModifierSet __instance)
            {
                var effect = new Effect(ModAssets.spooked, STRINGS.DUPLICANTS.STATUSITEMS.SPOOKED.NAME, STRINGS.DUPLICANTS.STATUSITEMS.SPOOKED.TOOLTIP, 120f, true, false, false)
                {
                    SelfModifiers = new List<AttributeModifier>() {
                    new AttributeModifier(Db.Get().Attributes.Athletics.Id, 4),
                    new AttributeModifier(Db.Get().Amounts.Bladder.deltaAttribute.Id, 1f / 3f)
                }
                };

                __instance.effects.Add(effect);
            }
        }

        [HarmonyPatch(typeof(EntityConfigManager), "LoadGeneratedEntities")]
        public class EntityConfigManager_LoadGeneratedEntities_Patch
        {
            public static void Prefix()
            {
                CROPS.CROP_TYPES.Add(new Crop.CropVal(PumpkinConfig.ID, 0.25f * 600.0f, 2));
            }
        }

        [HarmonyPatch(typeof(CookingStationConfig), "ConfigureRecipes")]
        public static class Patch_CookingStationConfig_ConfigureRecipes
        {
            public static void Postfix()
            {
                AddPumpkinPieRecipe();
                AddToastedSeedsRecipe();
            }

            private static void AddPumpkinPieRecipe()
            {
                var input = new RecipeElement[]
                {
                        new RecipeElement(ColdWheatConfig.SEED_ID, 2f),
                        new RecipeElement(RawEggConfig.ID, 0.3f),
                        new RecipeElement(PumpkinConfig.ID, 1f)
                };

                var output = new RecipeElement[]
                {
                        new RecipeElement(PumkinPieConfig.ID, 1f)
                };

                string recipeID = ComplexRecipeManager.MakeRecipeID(GourmetCookingStationConfig.ID, input, output);

                PumkinPieConfig.recipe = new ComplexRecipe(recipeID, input, output)
                {
                    time = FOOD.RECIPES.STANDARD_COOK_TIME,
                    description = STRINGS.ITEMS.FOOD.PUMPKINPIE.DESC,
                    nameDisplay = RecipeNameDisplay.Result,
                    fabricators = new List<Tag> { CookingStationConfig.ID }
                };
            }

            private static void AddToastedSeedsRecipe()
            {
                var input = new RecipeElement[]
                {
                        new RecipeElement(PumpkinPlantConfig.SEED_ID, 5f),
                        new RecipeElement(TableSaltConfig.ID, 0.05f)
                };

                var output = new RecipeElement[]
                {
                        new RecipeElement(ToastedPumpkinSeedConfig.ID, 1f)
                };

                string recipeID = ComplexRecipeManager.MakeRecipeID(GourmetCookingStationConfig.ID, input, output);

                PumkinPieConfig.recipe = new ComplexRecipe(recipeID, input, output)
                {
                    time = FOOD.RECIPES.STANDARD_COOK_TIME,
                    description = STRINGS.ITEMS.FOOD.TOASTEDPUMPKINSEED.DESC,
                    nameDisplay = RecipeNameDisplay.Result,
                    fabricators = new List<Tag> { CookingStationConfig.ID }
                };
            }
        }
    }
}
