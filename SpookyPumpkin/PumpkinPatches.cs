using FUtility;
using Harmony;
using Klei.AI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using TUNING;
using static ComplexRecipe;

namespace SpookyPumpkin
{
    class PumpkinPatches
    {
        static List<string> spawnedWorlds = new List<string>();

        [HarmonyPatch(typeof(GlobalAssets), "OnPrefabInit")]
        public static class GlobalAssets_OnPrefabInit_Patch
        {
            public static void Postfix(Dictionary<string, string> ___SoundTable)
            {
                foreach (var item in ___SoundTable)
                {
                    Log.Debuglog($"{item.Key} | {item.Value}");
                }
            }
        }

        [HarmonyPatch(typeof(World), "OnSpawn")]
        public static class World_OnLoad_Patch
        {
            public static void Postfix()
            {
                string id = SaveLoader.Instance.GameInfo.colonyGuid.ToString();
                if (spawnedWorlds == null || !spawnedWorlds.Contains(id))
                {
                    var telepad = GameUtil.GetTelepad();
                    if (telepad != null)
                    {
                        var prefab = Assets.GetPrefab(GhostSquirrelConfig.ID);
                        GameUtil.KInstantiate(prefab, telepad.transform.position, Grid.SceneLayer.Creatures).SetActive(true);
                        spawnedWorlds.Add(id);
                        WriteSettingsToFile();
                    }
                    else Log.Warning("No Printing Pod, cannot spawn pip.");
                }
            }
        }

        public static class Mod_OnLoad
        {
            public static void OnLoad(string path)
            {
                Log.PrintVersion();
                ModAssets.Initialize(path);
                spawnedWorlds = LoadSettingsFromFile();
            }
        }

        [HarmonyPatch(typeof(Localization), "Initialize")]
        class StringLocalisationPatch
        {
            public static void Postfix()
            {
                Loc.Translate(typeof(STRINGS));
/*
                Strings.Add("STRINGS.ITEMS.FOOD.PUMPKINPIE.NAME", STRINGS.ITEMS.FOOD.SP_PUMPKINPIE.NAME);
                Strings.Add("STRINGS.ITEMS.FOOD.PUMPKIN.NAME", STRINGS.ITEMS.FOOD.SP_PUMPKIN.NAME);
                Strings.Add("STRINGS.ITEMS.FOOD.TOASTEDPUMPKINSEED.NAME", STRINGS.ITEMS.FOOD.SP_TOASTEDPUMPKINSEED.NAME);*/
            }
        }

        [HarmonyPatch(typeof(Db), "Initialize")]
        public static class Db_Initialize_Patch
        {
            public static void Prefix()
            {
                ModAssets.LateLoadAssets();
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
                var effect = new Effect(
                    id: ModAssets.spooked,
                    name: STRINGS.DUPLICANTS.STATUSITEMS.SPOOKED.NAME,
                    description: STRINGS.DUPLICANTS.STATUSITEMS.SPOOKED.TOOLTIP,
                    duration: 120f,
                    show_in_ui: true,
                    trigger_floating_text: true,
                    is_bad: false)
                    {
                        SelfModifiers = new List<AttributeModifier>() {
                        new AttributeModifier(Db.Get().Attributes.Athletics.Id, 8),
                        new AttributeModifier(Db.Get().Amounts.Bladder.deltaAttribute.Id, 2f / 3f)
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
                CROPS.CROP_TYPES.Add(new Crop.CropVal(PumpkinConfig.ID, 12f * 600.0f, 2));
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
                    description = STRINGS.ITEMS.FOOD.SP_PUMPKINPIE.DESC,
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
                    description = STRINGS.ITEMS.FOOD.SP_TOASTEDPUMPKINSEED.DESC,
                    nameDisplay = RecipeNameDisplay.Result,
                    fabricators = new List<Tag> { CookingStationConfig.ID }
                };
            }
        }

        [HarmonyPatch(typeof(DetailsScreen), "OnPrefabInit")]
        public static class DetailsScreen_OnPrefabInit_Patch
        {
            public static void Postfix()
            {
                FUtility.FUI.SideScreen.AddCustomSideScreen<GhostSquirrelSideScreen>("GhostSquirrelSideScreen", ModAssets.sideScreenPrefab);
            }
        }

        public static void WriteSettingsToFile()
        {
            var filePath = Path.Combine(ModAssets.ModPath, "pipworlds.json");
            try
            {
                using (var sw = new StreamWriter(filePath))
                {
                    var serializedUserSettings = JsonConvert.SerializeObject(spawnedWorlds, Formatting.Indented);
                    sw.Write(serializedUserSettings);
                }
            }
            catch (Exception e)
            {
                Log.Warning($"Couldn't write to {filePath}, {e.Message}");
            }

        }

        private static List<string> LoadSettingsFromFile()
        {
            var filePath = Path.Combine(ModAssets.ModPath, "pipworlds.json");
            List<string> userSettings = new List<string>();

            try
            {
                using (var r = new StreamReader(filePath))
                {
                    var json = r.ReadToEnd();
                    userSettings = JsonConvert.DeserializeObject<List<string>>(json);
                }
            }
            catch (Exception e)
            {
                Log.Warning($"Couldn't read {filePath}, {e.Message}. Using default settings.");
                return new List<string>();
            }

            return userSettings;
        }
    }
}
