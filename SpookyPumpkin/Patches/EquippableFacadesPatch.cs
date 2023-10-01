using Database;
using HarmonyLib;
using SpookyPumpkinSO.Content;

namespace SpookyPumpkinSO.Patches
{
	public class EquippableFacadesPatch
	{
		[HarmonyPatch(typeof(EquippableFacades), MethodType.Constructor, typeof(ResourceSet))]
		public class EquippableFacades_Ctor_Patch
		{
			public static void Postfix(EquippableFacades __instance)
			{
				SPEquippableFacades.Register(__instance);
			}
		}
	}
}
