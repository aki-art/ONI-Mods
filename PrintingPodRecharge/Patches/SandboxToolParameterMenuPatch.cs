using FUtility;
using HarmonyLib;
using PrintingPodRecharge.Content.Items;
using PrintingPodRecharge.Content.Items.BookI;
using PrintingPodRecharge.Content.Items.Dice;
using System.Collections.Generic;
using static SandboxToolParameterMenu.SelectorValue;

namespace PrintingPodRecharge.Patches
{
	public class SandboxToolParameterMenuPatch
	{
		private static readonly HashSet<Tag> items = new HashSet<Tag>()
		{
			BioInkConfig.DEFAULT,
			BioInkConfig.FOOD,
			BioInkConfig.METALLIC,
			BioInkConfig.SEEDED,
			BioInkConfig.SHAKER,
			BioInkConfig.VACILLATING,
			BioInkConfig.TWITCH,
			BioInkConfig.MEDICINAL,
			CatDrawingConfig.ID,
			SelfImprovablesConfig.BOOK_VOL1,
			SelfImprovablesConfig.BOOK_VOL2,
			SelfImprovablesConfig.MANGA,
			SelfImprovablesConfig.D8,
			HeatCubeConfig.ID,
			D6Config.ID
		};

		[HarmonyPatch(typeof(SandboxToolParameterMenu), "ConfigureEntitySelector")]
		public static class SandboxToolParameterMenu_ConfigureEntitySelector_Patch
		{
			public static void Postfix(SandboxToolParameterMenu __instance)
			{
				var sprite = Def.GetUISprite(Assets.GetPrefab(BioInkConfig.DEFAULT));
				var bioInkFilter = new SearchFilter(STRINGS.UI.SANBOXTOOLS.FILTERS.BIO_INKS, obj => obj is KPrefabID id && items.Contains(id.PrefabTag), null, sprite);

				SandboxUtil.AddFilters(__instance, bioInkFilter);
			}
		}
	}
}
