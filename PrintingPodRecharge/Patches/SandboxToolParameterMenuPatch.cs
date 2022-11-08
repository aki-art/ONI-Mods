using FUtility;
using HarmonyLib;
using PrintingPodRecharge.Items;
using System.Collections.Generic;
using static SandboxToolParameterMenu.SelectorValue;

namespace PrintingPodRecharge.Patches
{
    public class SandboxToolParameterMenuPatch
    {
        private static HashSet<Tag> items = new HashSet<Tag>()
        {
            BioInkConfig.DEFAULT,
            BioInkConfig.FOOD,
            BioInkConfig.METALLIC,
            BioInkConfig.SEEDED,
            BioInkConfig.SHAKER,
            BioInkConfig.VACILLATING,
            BioInkConfig.TWITCH,
            CatDrawingConfig.ID,
            BookOfSelfImprovementConfig.ID,
            //EmptyBottleConfig.ID
        };

        [HarmonyPatch(typeof(SandboxToolParameterMenu), "ConfigureEntitySelector")]
        public static class SandboxToolParameterMenu_ConfigureEntitySelector_Patch
        {
            public static void Postfix(SandboxToolParameterMenu __instance)
            {
                var sprite = Def.GetUISprite(global::Assets.GetPrefab(Items.BioInkConfig.DEFAULT));

                var bioInkFilter = new SearchFilter(STRINGS.UI.SANBOXTOOLS.FILTERS.BIO_INKS, obj => obj is KPrefabID id && items.Contains(id.PrefabTag), null, sprite);

                SandboxUtil.AddFilters(__instance, bioInkFilter);
            }
        }
    }
}
