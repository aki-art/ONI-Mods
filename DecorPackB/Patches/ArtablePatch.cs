using DecorPackB.Content;
using HarmonyLib;

namespace DecorPackB.Patches
{
	public class ArtablePatch
	{
		[HarmonyPatch(typeof(Artable), "SetDefault")]
		public class Artable_SetDefault_Patch
		{
			public static void Postfix(Artable __instance)
			{
				__instance.gameObject.Trigger(DPIIHashes.FossilStageUnset);
			}
		}
	}
}
