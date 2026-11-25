using HarmonyLib;

namespace GoldenThrone.Patches
{
    public class InventoryOrganizationPatch
    {
        [HarmonyPatch(typeof(InventoryOrganization), "GenerateSubcategories")]
        public class InventoryOrganization_GenerateSubcategories_Patch
        {
            public static void Postfix()
            {
                var pots = InventoryOrganization.subcategoryIdToPermitIdsMap[InventoryOrganization.PermitSubcategories.BUILDINGS_WASHROOM];
                pots.Add(GTFacades.GOLDEN_TOILET);
            }
        }
    }
}
