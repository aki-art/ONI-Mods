using Harmony;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Slag.Critter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Utils;

namespace Slag.Items
{
    class ItemPatches
    {
        private static Dictionary<string, Dictionary<string, Dictionary<string, float>>> moltWeights;
        public static class Mod_OnLoad
        {
            public static void OnLoad(string path)
            {
                LoadMiteRewards();
            }
        }

        [HarmonyPatch(typeof(Db))]
        [HarmonyPatch("Initialize")]
        public static class Db_Initialize_Patch
        {
            public static void Prefix()
            {

                var test = GetMetalRewards("worthless");

                foreach (var t in test)
                {
                    Debug.Log(t.element + ", " + t.weight);
                }
            }
        }

        public static List<WeightedMetalOption> GetMetalRewards(string tier)
        {
            if (moltWeights == null)
            {
                Log.Warning("Molt weights have not been assigned yet!");
                return new List<WeightedMetalOption>();
            }

            var options = new List<WeightedMetalOption>();
            var mysteryMetals = moltWeights["mysteryMetal"][tier];

            foreach (var kvp in mysteryMetals)
            {
                if(Enum.TryParse(kvp.Key, out SimHashes simHash))
                { 
                    options.Add(new WeightedMetalOption(simHash, kvp.Value));
                }
                else
                {
                    Log.Warning("Invalid Element: " + kvp.Key);
                }
            }

            return options;
        }
        private static void LoadMiteRewards()
        {
            var filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets", "mite_rewards.json");
            Log.Debuglog("Loading mite reward file from: " + filePath);

            try
            {
                using (var r = new StreamReader(filePath))
                {
                    var json = r.ReadToEnd();
                    moltWeights = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, float>>>>(json);
                    //Log.Info(dataModel["mysteryMetal"]["worthless"]["Sand"]);
                }
                //Log.Info(data);
            }
            catch (Exception e)
            {
                Log.Warning($"Couldn't read {filePath}, {e.Message}.");
            }
        }


        [HarmonyPatch(typeof(RockCrusherConfig), "ConfigureBuildingTemplate")]
        public static class RockCrusherConfig_ConfigureBuildingTemplate_Patch
        {
            public static void Postfix()
            {
                var sandstone = ElementLoader.FindElementByHash(SimHashes.SandStone);

                ComplexRecipe.RecipeElement[] input = new ComplexRecipe.RecipeElement[]
                {
                     new ComplexRecipe.RecipeElement(sandstone.tag, 10f),
                };

                ComplexRecipe.RecipeElement[] output = new ComplexRecipe.RecipeElement[]
                {
                     new ComplexRecipe.RecipeElement(MysteryOreConfig.ID, 110f),
                };

                string id = ComplexRecipeManager.MakeRecipeID(MetalRefineryConfig.ID, input, output);

                new ComplexRecipe(id, input, output)
                {
                    time = 40f,
                    description = string.Format("test"),
                    nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
                    fabricators = new List<Tag> { TagManager.Create(RockCrusherConfig.ID) }
                };

                ComplexRecipe.RecipeElement[] input2 = new ComplexRecipe.RecipeElement[]
                {
                     new ComplexRecipe.RecipeElement(sandstone.tag, 10f),
                };

                ComplexRecipe.RecipeElement[] output2 = new ComplexRecipe.RecipeElement[]
                {
                     new ComplexRecipe.RecipeElement(MysteryMetalConfig.ID, 110f),
                };

                string id2 = ComplexRecipeManager.MakeRecipeID(MetalRefineryConfig.ID, input2, output2);

                new ComplexRecipe(id2, input2, output2)
                {
                    time = 40f,
                    description = string.Format("test"),
                    nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
                    fabricators = new List<Tag> { TagManager.Create(RockCrusherConfig.ID) }
                };
            }
        }
    }
}
