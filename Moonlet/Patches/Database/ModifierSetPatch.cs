using HarmonyLib;

namespace Moonlet.Patches.Database
{
	public class ModifierSetPatch
	{
		[HarmonyPatch(typeof(ModifierSet), nameof(ModifierSet.Initialize))]
		public class ModifierSet_Initialize_Patch
		{
			public static void Postfix(ModifierSet __instance)
			{
				Mod.effectsLoader.LoadContent(__instance);
			}
		}
	}
}
