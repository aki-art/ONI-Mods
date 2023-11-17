using GravitasBigStorage.Content;
using HarmonyLib;

namespace GravitasBigStorage.Patches
{
	public class InventoryOrganizationPatch
	{
		[HarmonyPatch(typeof(InventoryOrganization), "GenerateSubcategories")]
		public class InventoryOrganization_GenerateSubcategories_Patch
		{
			public static void Postfix()
			{
				GBSFacades.AddSubCategory();
			}
		}
	}
}
