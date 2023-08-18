using HarmonyLib;
using Moonlet.Loaders;

namespace Moonlet.Patches
{
	[HarmonyPatch(typeof(ModifierSet), "Initialize")]
	public class ModifierSet_Initialize_Patch
	{
		public static void Postfix(ModifierSet __instance)
		{
			ModEffectsLoader.Register(__instance);
		}
	}
}
