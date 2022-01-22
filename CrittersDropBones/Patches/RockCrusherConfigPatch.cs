using CrittersDropBones.Items;
using FUtility;
using HarmonyLib;
using static ComplexRecipe;

namespace CrittersDropBones.Patches
{
    public class RockCrusherConfigPatch
    {
        // Adds recipes to the Rock Crusher
        [HarmonyPatch(typeof(RockCrusherConfig), "ConfigureBuildingTemplate")]
        public static class RockCrusherConfig_ConfigureBuildingTemplate_Patch
        {
            public static void Postfix()
            {
                Tag lime = SimHashes.Lime.CreateTag();

                AddBoneRecipe(MediumBoneConfig.ID, 1f, lime, 1f);
                AddBoneRecipe(LargeBoneConfig.ID, 1f, lime, 2f);
                AddBoneRecipe(FishBoneConfig.ID, 1f, lime, 0.5f);
            }
        }

        private static void AddBoneRecipe(Tag input, float amountIn, Tag output, float amountOut)
        {
            string desc = string.Format(STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.RECIPE_DESCRIPTION, input.ProperName(), output.ProperName());
            Utils.AddRecipe(RockCrusherConfig.ID, new RecipeElement(input, amountIn), new RecipeElement(output, amountOut), desc, 1);
        }
    }
}
