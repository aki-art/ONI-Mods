using DecorPackA.Buildings.GlassSculpture;
using HarmonyLib;

namespace DecorPackA.Patches
{
	public class ArtablePatch
	{
		[HarmonyPatch(typeof(Artable), "SetStage")]
		public static class Artable_SetStage_Patch
		{
			public static void Postfix(Artable __instance)
			{
				__instance.Trigger(ModEvents.OnArtableStageSet);

				if (__instance.TryGetComponent(out Fabulous fabulous))
					fabulous.RefreshFab();
			}
		}


		[HarmonyPatch(typeof(Artable), "SetDefault")]
		public class Artable_SetDefault_Patch
		{
			public static void Postfix(Artable __instance)
			{
				__instance.Trigger(ModEvents.OnArtableStageSet);
			}
		}
	}
}
