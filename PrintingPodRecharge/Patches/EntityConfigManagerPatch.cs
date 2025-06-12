using HarmonyLib;

namespace PrintingPodRecharge.Patches
{
	public class EntityConfigManagerPatch
	{
		[HarmonyPatch(typeof(EntityConfigManager), "LoadGeneratedEntities")]
		public class EntityConfigManager_LoadGeneratedEntities_Patch
		{
		}
	}
}
