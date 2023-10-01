using Database;
using HarmonyLib;
using SpookyPumpkinSO.Content;

namespace SpookyPumpkinSO.Patches
{
	public class AccessorySlotsPatch
	{
		[HarmonyPatch(typeof(AccessorySlots), MethodType.Constructor, typeof(ResourceSet))]
		public class AccessorySlots_TargetMethod_Patch
		{
			public static void Postfix(AccessorySlots __instance, ResourceSet parent)
			{
				SPAccessories.Register(__instance, parent);
			}
		}
	}
}
