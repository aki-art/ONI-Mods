using FUtility;
using HarmonyLib;
using Kigurumis.Content;
using System.Linq;

namespace Kigurumis.Patches
{
    public class ClothingAlterationStationConfigPatch
    {
        [HarmonyPatch(typeof(ClothingAlterationStationConfig), "ConfigureRecipes")]
        public class ClothingAlterationStationConfig_ConfigureRecipes_Patch
        {
            public static void Postfix()
            {
                var kigurumis = Db.Get().EquippableFacades.resources
                    .FindAll(match => match.DefID == HoodedKigurumiConfig.ID);

                foreach (var kigu in kigurumis)
                {
                    var fabricationTime = TUNING.EQUIPMENT.VESTS.CUSTOM_CLOTHING_FABTIME;
#if DEBUG
                    fabricationTime = 5f;
#endif

                    RecipeBuilder
                        .Create(ClothingAlterationStationConfig.ID, "desc", fabricationTime)
                        .NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)

                        .Input(FunkyVestConfig.ID, 1f, false)
                        .Input(BasicFabricConfig.ID, 3f)

                        .FacadeOutput(HoodedKigurumiConfig.ID, 1f, kigu.Id)

                        .Build();
                }
            }
        }
    }
}
