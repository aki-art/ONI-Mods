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
                var lime = SimHashes.Lime.CreateTag();

                AddBoneRecipe(BoneConfig.ID, 1f, lime, 1f);
                //A/ddBoneRecipe(LargeBoneConfig.ID, 1f, lime, 2f);
                AddBoneRecipe(FishBoneConfig.ID, 1f, lime, 0.5f);
            }
        }

        private static void AddBoneRecipe(Tag input, float amountIn, Tag output, float amountOut)
        {
            var desc = string.Format(global::STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.RECIPE_DESCRIPTION, input.ProperName(), output.ProperName());
            Utils.AddRecipe(RockCrusherConfig.ID, new RecipeElement(input, amountIn), new RecipeElement(output, amountOut), desc, 1);
        }
    }
}
