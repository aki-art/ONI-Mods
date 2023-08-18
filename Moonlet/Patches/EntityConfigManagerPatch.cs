using HarmonyLib;

namespace Moonlet.Patches
{
	public class EntityConfigManagerPatch
	{
		[HarmonyPatch(typeof(EntityConfigManager), "LoadGeneratedEntities")]
		public class EntityConfigManager_LoadGeneratedEntities_Patch
		{
			public static void Prefix()
			{
				foreach (var mod in Mod.modLoaders)
				{
					mod.entitiesLoader.LoadPois();
					mod.entitiesLoader.LoadItems();
				}
			}
		}
	}
}
