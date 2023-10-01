using Database;
using HarmonyLib;
using SpookyPumpkinSO.Content;

namespace SpookyPumpkinSO.Patches
{
	public class DuplicantStatusItemsPatch
	{
		[HarmonyPatch(typeof(DuplicantStatusItems), MethodType.Constructor, typeof(ResourceSet))]
		public class DuplicantStatusItems_Ctor_Patch
		{
			public static void Postfix(DuplicantStatusItems __instance)
			{
				SPStatusItems.Register(__instance);
			}
		}
	}
}
