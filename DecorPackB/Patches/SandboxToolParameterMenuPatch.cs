using DecorPackB.Content.Defs.Items;
using HarmonyLib;
using System.Collections.Generic;

namespace DecorPackB.Patches
{
	public class SandboxToolParameterMenuPatch
	{
		private static readonly HashSet<Tag> items = new()
		{
			FossilNoduleConfig.ID,
			GiantFossilFragmentConfigs.BRONTO,
			GiantFossilFragmentConfigs.LIVAYATAN,
			GiantFossilFragmentConfigs.TREX,
			GiantFossilFragmentConfigs.TRICERATOPS,
		};

		[HarmonyPatch(typeof(SandboxToolParameterMenu), "ConfigureEntitySelector")]
		public static class SandboxToolParameterMenu_ConfigureEntitySelector_Patch
		{
			public static void Postfix(SandboxToolParameterMenu __instance)
			{
				var menu = SandboxUtil.AddModMenu(
					__instance,
					"Decor Pack II",
					Def.GetUISprite(Assets.GetPrefab(FossilNoduleConfig.ID)),
					obj => obj is KPrefabID id && items.Contains(id.PrefabTag));

				SandboxUtil.UpdateOptions(__instance);
			}
		}
	}
}
