using HarmonyLib;
using System.Collections.Generic;
using Twitchery.Content.Defs;
using Twitchery.Content.Defs.Calamities;
using static SandboxToolParameterMenu.SelectorValue;

namespace Twitchery.Patches
{
	public class SandboxToolParameterMenuPatch
	{
		private static readonly HashSet<Tag> items =
		[
			GiantRadishConfig.ID,
			PizzaBoxConfig.ID,
			//LavaSourceBlockConfig.ID,
			//PocketDimensionThingConfig.ID,
			SinkHoleSpawnerConfig.ID,
			PimpleConfig.ID,
			MugCometConfig.ID,
			SandDropCometConfig.ID,
			SandstormSpawnerConfig.ID,
			DeathLaserConfig.ID,
			BigWormConfig.ID,
			SmallWormConfig.ID,
			GenericEggCometConfig.ID,
			PandorasBoxConfig.ID,
			$"GeyserGeneric_{TGeyserConfigs.MOLTEN_GLASS_VOLCANO}",
			$"GeyserGeneric_{TGeyserConfigs.GOOP_GEYSER}",
			//$"GeyserGeneric_{TGeyserConfigs.NUCLEAR_WASTE_GEYSER}",
#if WEREVOLE
			WereVoleCureConfig.ID,
#endif
#if SUPERPIP
			TentaclePortalConfig.ID
#endif
		];

		[HarmonyPatch(typeof(SandboxToolParameterMenu), "ConfigureEntitySelector")]
		public static class SandboxToolParameterMenu_ConfigureEntitySelector_Patch
		{
			public static void Postfix(SandboxToolParameterMenu __instance)
			{
				var sprite = Def.GetUISprite(Assets.GetPrefab(GiantRadishConfig.ID));
				var filter = new SearchFilter("Extra Twitch Events", obj => obj is KPrefabID id && items.Contains(id.PrefabTag), null, sprite);

				SandboxUtil.AddFilters(__instance, filter);
				SandboxUtil.UpdateOptions(__instance);
			}
		}
	}
}
