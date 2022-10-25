using FUtility;
using HarmonyLib;
using SpookyPumpkinSO.Content;

namespace SpookyPumpkinSO.Patches
{
    public class ClothingAlterationStationConfigPatch
    {
        [HarmonyPatch(typeof(ClothingAlterationStationConfig), "ConfigureRecipes")]
        public class ClothingAlterationStationConfig_ConfigureRecipes_Patch
        {
            public static void Postfix()
            {
                var fabricationTime = TUNING.EQUIPMENT.VESTS.CUSTOM_CLOTHING_FABTIME;
#if DEBUG
                fabricationTime = 1f;
#endif

                AddCostumeRecipe(SPEquippableFacades.SKELLINGTON, "desc", fabricationTime);
                AddCostumeRecipe(SPEquippableFacades.SCARECROW, "desc", fabricationTime);
                AddCostumeRecipe(SPEquippableFacades.VAMPIRE, "desc", fabricationTime);
            }

            private static void AddCostumeRecipe(string facadeID, string description, float fabricationTime)
            {
                RecipeBuilder
                    .Create(ClothingAlterationStationConfig.ID, description, fabricationTime)
                    .NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)

                    .Input(FunkyVestConfig.ID, 1f, false)
                    .Input(BasicFabricConfig.ID, 3f)

                    .FacadeOutput(HalloweenCostumeConfig.ID, 1f, facadeID)

                    .Build(facadeID);
            }
        }
    }
}