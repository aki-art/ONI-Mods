using HarmonyLib;
using UnityEngine;

namespace PrintingPodRecharge.Patches
{
	public class CraftingTableConfigPatch
	{
		[HarmonyPatch(typeof(CraftingTableConfig), "ConfigureBuildingTemplate")]
		public class CraftingTableConfig_ConfigureBuildingTemplate_Patch
		{
			public static void Postfix(GameObject go)
			{
				go.GetComponent<Storage>().SetDefaultStoredItemModifiers(
				[
					Storage.StoredItemModifier.Hide,
					Storage.StoredItemModifier.Preserve,
					Storage.StoredItemModifier.Seal
				]);
			}
		}
	}
}
