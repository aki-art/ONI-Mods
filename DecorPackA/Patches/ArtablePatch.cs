using DecorPackA.Buildings.GlassSculpture;
using HarmonyLib;

namespace DecorPackA.Patches
{
	public class ArtablePatch
	{
		[HarmonyPatch(typeof(Artable), "OnSpawn")]
		public class Artable_OnSpawn_Patch
		{
			public static void Prefix(Artable __instance)
			{
				if(__instance.IsPrefabID(GlassSculptureConfig.ID))
				{
					if (Db.GetArtableStages().TryGet(__instance.currentStage) == null)
					{
						__instance.currentStage = "Default";
					}
				}
			}
		}

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
