using HarmonyLib;
using SpookyPumpkinSO.Content;

namespace SpookyPumpkinSO.Patches
{
	public class ModifierSetPatch
	{
		[HarmonyPatch(typeof(ModifierSet), nameof(ModifierSet.Initialize))]
		public static class ModifierSet_Initialize_Patch
		{
			public static void Postfix(ModifierSet __instance)
			{
				SPEffects.Register(__instance);
			}
		}
	}
}
