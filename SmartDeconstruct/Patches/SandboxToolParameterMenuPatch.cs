using HarmonyLib;
using SmartDeconstruct.Content;
using System.Collections.Generic;
using static SandboxToolParameterMenu.SelectorValue;

namespace SmartDeconstruct.Patches
{
	public class SandboxToolParameterMenuPatch
	{
		private static readonly HashSet<Tag> items =
		[
			SmartDeconstructMarkerConfig.ID
		];

		[HarmonyPatch(typeof(SandboxToolParameterMenu), "ConfigureEntitySelector")]
		public static class SandboxToolParameterMenu_ConfigureEntitySelector_Patch
		{
			public static void Postfix(SandboxToolParameterMenu __instance)
			{
				var sprite = Def.GetUISprite(Assets.GetPrefab(BasicForagePlantPlantedConfig.ID));
				var filter = new SearchFilter("Smart Deconstruct", obj => obj is KPrefabID id && items.Contains(id.PrefabTag), SandboxUtil.AddOrGetModsMenu(__instance), sprite);

				SandboxUtil.AddFilters(__instance, filter);
				SandboxUtil.UpdateOptions(__instance);
			}
		}
	}
}
