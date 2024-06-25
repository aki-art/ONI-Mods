using HarmonyLib;

namespace Moonlet.Patches
{
	public class EntityConfigManagerPatch
	{
		[HarmonyPatch(typeof(EntityConfigManager), nameof(EntityConfigManager.LoadGeneratedEntities))]
		public class EntityConfigManager_LoadGeneratedEntities_Patch
		{
			public static void Prefix()
			{
				Mod.genericEntitiesLoader.ApplyToActiveTemplates(loader => loader.LoadContent());
				Mod.itemsLoader.ApplyToActiveTemplates(loader => loader.LoadContent());
				Mod.decorPlantsLoader.ApplyToActiveTemplates(loader => loader.LoadContent());
				Mod.artifactsLoader.ApplyToActiveTemplates(loader => loader.LoadContent());
				Mod.harvestableSpacePOIsLoader.ApplyToActiveTemplates(loader => loader.LoadContent());
			}
		}
	}
}
