using FUtility;
using HarmonyLib;
using Slag.Content.Critters.Slagmite;
using Slag.Content.Entities;
using Slag.Content.Items;
using System.Collections.Generic;
using System.Linq;
using static SandboxToolParameterMenu.SelectorValue;

namespace Slag.Patches
{
    public class SandboxToolParameterMenuPatch
    {
        private static readonly HashSet<Tag> tags = new HashSet<Tag>()
        {
            SlagWoolConfig.ID,
            //BabySlagmiteConfig.ID,
            CottonCandyConfig.ID,
            EggCometConfig.ID,
            SlagmiteConfig.ID
        };

        [HarmonyPatch(typeof(SandboxToolParameterMenu), "ConfigureEntitySelector")]
        public static class SandboxToolParameterMenu_ConfigureEntitySelector_Patch
        {
            public static void Postfix(SandboxToolParameterMenu __instance)
            {
                var sprite = Def.GetUISprite(Assets.GetPrefab(SlagmiteConfig.ID));
                var parent = __instance.entitySelector.filters.First(x => x.Name == global::STRINGS.UI.SANDBOXTOOLS.FILTERS.ENTITIES.SPECIAL);

                var filter = new SearchFilter("Slag", obj => obj is KPrefabID id && tags.Contains(id.PrefabTag), parent, sprite);

                SandboxUtil.AddFilters(__instance, filter);
            }
        }
    }
}
