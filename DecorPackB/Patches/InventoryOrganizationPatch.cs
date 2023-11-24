using DecorPackB.Content.ModDb;
using HarmonyLib;

namespace DecorPackB.Patches
{
	public class InventoryOrganizationPatch
	{
		[HarmonyPatch(typeof(InventoryOrganization), "GenerateSubcategories")]
		public class InventoryOrganization_GenerateSubcategories_Patch
		{
			public static void Postfix()
			{
				DPInventory.ConfigureSubCategories();
			}
		}
	}
}
