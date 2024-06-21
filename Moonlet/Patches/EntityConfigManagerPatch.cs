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
				Log.Debug("LoadGeneratedEntities");
				Mod.itemsLoader.ApplyToActiveTemplates(loader => loader.LoadContent());
				Mod.decorPlantsLoader.ApplyToActiveTemplates(loader => loader.LoadContent());
			}
		}
	}
}
