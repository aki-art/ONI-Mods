using FUtility;
using HarmonyLib;
using Slag.Content;
using Slag.Content.Critters.BrittleDrill;
using System;

namespace Slag.Patches
{
    public class DbPatch
    {
        [HarmonyPatch(typeof(Db), "Initialize")]
        public class Db_Initialize_Patch
        {
            public static void Postfix()
            {
                ConfigureGlassForgeRecipes();
                ConfigureCritterEggChances();
            }

            private static void ConfigureCritterEggChances()
            {
                MoleTuning.EGG_CHANCES_BASE.Add(new FertilityMonitor.BreedingChance
                {
                    egg = BrittleDrillConfig.EGG_ID,
                    weight = 0.02f
                });

                MoleTuning.EGG_CHANCES_DELICACY.Add(new FertilityMonitor.BreedingChance
                {
                    egg = BrittleDrillConfig.EGG_ID,
                    weight = 0.05f
                });

                //TUNING.CREATURES.EGG_CHANCE_MODIFIERS.
            }

            private static void ConfigureGlassForgeRecipes()
            {
                var description = global::STRINGS.BUILDINGS.PREFABS.GLASSFORGE.RECIPE_DESCRIPTION;
                var slagGlassName = ElementLoader.FindElementByHash(Elements.MoltenSlagGlass).name;
                var slagName = ElementLoader.FindElementByHash(Elements.Slag).name;
                var regolithName = ElementLoader.FindElementByHash(SimHashes.Regolith).name;

                RecipeBuilder.Create(GlassForgeConfig.ID, string.Format(description, slagGlassName, slagName), 40f)
                    .Input(ModAssets.Tags.slag, 100f)
                    .Output(Elements.MoltenSlagGlass.CreateTag(), 75f, ComplexRecipe.RecipeElement.TemperatureOperation.Melted)
                    .Build();

                RecipeBuilder.Create(GlassForgeConfig.ID, string.Format(description, slagGlassName, regolithName), 40f)
                    .Input(SimHashes.Regolith.CreateTag(), 250f)
                    .Output(Elements.MoltenSlagGlass.CreateTag(), 25f, ComplexRecipe.RecipeElement.TemperatureOperation.Melted)
                    .Build();
            }
        }
    }
}
