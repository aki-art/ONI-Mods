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
				Mod.genericEntitiesLoader.ApplyToActiveLoaders(loader => loader.LoadContent());
				Mod.itemsLoader.ApplyToActiveLoaders(loader => loader.LoadContent());
				Mod.decorPlantsLoader.ApplyToActiveLoaders(loader => loader.LoadContent());
				Mod.singleHarvestPlantsLoader.ApplyToActiveLoaders(loader => loader.LoadContent());
				Mod.artifactsLoader.ApplyToActiveLoaders(loader => loader.LoadContent());
				Mod.harvestableSpacePOIsLoader.ApplyToActiveLoaders(loader => loader.LoadContent());
			}
		}
	}
}
