using FUtility;
using HarmonyLib;
using SpookyPumpkinSO.Content;
using System.Linq;

namespace SpookyPumpkinSO.Patches
{
    public class ClothingAlterationStationConfigPatch
    {
        [HarmonyPatch(typeof(ClothingAlterationStationConfig), "ConfigureRecipes")]
        public class ClothingAlterationStationConfig_ConfigureRecipes_Patch
        {
            public static void Postfix()
            {
                var fabricationTime = 3f; //TUNING.EQUIPMENT.VESTS.CUSTOM_CLOTHING_FABTIME;

                RecipeBuilder
                    .Create(ClothingAlterationStationConfig.ID, "desc", fabricationTime)
                    .NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)

                    .Input(FunkyVestConfig.ID, 1f, false)
                    .Input(BasicFabricConfig.ID, 3f)

                    .FacadeOutput(HalloweenCostumeConfig.ID, 1f, SPEquippableFacades.SKELLINGTON)

                    .Build();
            }
        }
    }
}