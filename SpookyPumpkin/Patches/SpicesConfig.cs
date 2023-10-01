using Database;
using HarmonyLib;
using SpookyPumpkinSO.Content;

namespace SpookyPumpkinSO.Patches
{
	public class SpicesConfig
	{
		[HarmonyPatch(typeof(Spices), MethodType.Constructor, typeof(ResourceSet))]
		public class Spices_TargetMethod_Patch
		{
			public static void Postfix(Spices __instance)
			{
				SPSpices.Register(__instance);
			}
		}
	}
}
