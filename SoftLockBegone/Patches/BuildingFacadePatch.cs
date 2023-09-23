using FUtility;
using HarmonyLib;

namespace SoftLockBegone.Patches
{
	public class BuildingFacadePatch
	{
		[HarmonyPatch(typeof(BuildingFacade), "OnSpawn")]
		public class BuildingFacade_OnSpawn_Patch
		{
			public static void Prefix(BuildingFacade __instance)
			{
				if (__instance.currentFacade != null && Db.Get().Permits.BuildingFacades.TryGet(__instance.currentFacade) == null)
				{
					Log.Warning($"Invalid building Facade {__instance.currentFacade} for {__instance.PrefabID()}. Restoring to default.");
					__instance.currentFacade = null;
				}
			}
		}
	}
}
