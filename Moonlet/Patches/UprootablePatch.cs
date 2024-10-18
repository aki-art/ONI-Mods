using HarmonyLib;
using Moonlet.Scripts;

namespace Moonlet.Patches
{
	public class UprootablePatch
	{
		[HarmonyPatch(typeof(Uprootable), "Uproot")]
		public class Uprootable_Uproot_Patch
		{
			// Skipping this prefix, but will call it again without skipping when my animation has finished, 
			// and that will allow other mods patches through
			public static bool Prefix(Uprootable __instance) => Moonlet_ExtendedUprootable.Uproot(__instance);
		}
	}
}
