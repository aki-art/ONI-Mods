/*using Database;
using HarmonyLib;

namespace GoldenThrone.Patches
{
    public class KleiPermitVisUtilPatch
    {
        [HarmonyPatch(typeof(KleiPermitVisUtil), "ConfigureToRenderBuilding", [typeof(KBatchedAnimController), typeof(BuildingFacadeResource)])]
        public class KleiPermitVisUtil_ConfigureToRenderBuilding_Patch
        {
            public static void Postfix(KBatchedAnimController buildingKAnim, BuildingFacadeResource buildingPermit)
            {
                if (buildingPermit.Id == GTFacades.GOLDEN_TOILET)
                    buildingKAnim.Play("blueprint");
            }
        }
    }
}
*/