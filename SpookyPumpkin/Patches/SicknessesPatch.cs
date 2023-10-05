using Database;
using HarmonyLib;
using SpookyPumpkinSO.Content;

namespace SpookyPumpkinSO.Patches
{
	public class SicknessesPatch
	{
		[HarmonyPatch(typeof(Sicknesses), MethodType.Constructor, typeof(ResourceSet))]
		public class TargetType_Ctor_Patch
		{
			public static void Postfix(Sicknesses __instance)
			{
				SPSicknesses.Register(__instance);
			}
		}
	}
}
