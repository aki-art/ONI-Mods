using DecorPackB.Items;
using FUtility;
using HarmonyLib;

namespace DecorPackB.Patches
{
    public class ComplexFabricatorPatch
    {
        [HarmonyPatch(typeof(ComplexFabricator), "SpawnOrderProduct")]
        public class ComplexFabricator_SpawnOrderProduct_Patch
        {
            public static void Postfix(ComplexFabricator __instance, ComplexRecipe recipe)
            {
                if (recipe.id == "RockCrusher_I_Fossil_O_Lime_SedimentaryRock")
                {
                    float chance = DlcManager.IsExpansion1Active() ?
                        Mod.Settings.FossilDisplay.FossileNoduleFromFossilChance_SpacedOut :
                        Mod.Settings.FossilDisplay.FossileNoduleFromFossilChance_VanillaOrClassic;

                    if (UnityEngine.Random.value < chance)
                    {
                        Utils.Spawn(FossilNoduleConfig.ID, __instance.transform.position + __instance.outputOffset);
                    }
                }
            }
        }
    }
}
