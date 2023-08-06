/*using HarmonyLib;

namespace Twitchery.Patches
{
	public class EntityConfigManagerPatch
	{
		[HarmonyPatch(typeof(EntityConfigManager), "LoadGeneratedEntities")]
		public class EntityConfigManager_LoadGeneratedEntities_Patch
		{
			public static void Postfix()
			{
				var acorn = Assets.GetPrefab(ForestTreeConfig.SEED_ID);

				if (acorn.TryGetComponent(out Edible _))
					return;

				EntityTemplates.ExtendEntityToFood(
					acorn,
					new EdiblesManager.FoodInfo(
						ForestTreeConfig.SEED_ID,
						"",
						0.0f,
						0,
						283.15f,
						308.15f,
						float.PositiveInfinity,
						false));
			}
		}
	}
}
*/