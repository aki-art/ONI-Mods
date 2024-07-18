using HarmonyLib;

namespace MoreMarbleSculptures.Patches
{
	public class InventoryOrganizationPatch
	{
		[HarmonyPatch(typeof(InventoryOrganization), "GenerateSubcategories")]
		public class InventoryOrganization_GenerateSubcategories_Patch
		{
			public static void Postfix()
			{
				var sculptures = InventoryOrganization.subcategoryIdToPermitIdsMap[InventoryOrganization.PermitSubcategories.BUILDING_SCULPTURE];

				foreach (var sculpture in Mod.mySculptureIds)
					sculptures.Add(sculpture);
			}
		}
	}
}
