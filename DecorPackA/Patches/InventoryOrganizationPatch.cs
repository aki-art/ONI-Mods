using HarmonyLib;

namespace DecorPackA.Patches
{
	public class InventoryOrganizationPatch
	{
		[HarmonyPatch(typeof(InventoryOrganization), "GenerateSubcategories")]
		public class InventoryOrganization_GenerateSubcategories_Patch
		{
			public static void Postfix()
			{
				DPFacades.ConfigureSubCategories();
			}
		}
	}
}
