using FUtility;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using static SandboxToolParameterMenu.SelectorValue;

namespace MayISit.Patches
{
    public class SandboxToolParameterMenuPatch
    {
        private static readonly HashSet<Tag> tags = new HashSet<Tag>()
        {
            "PropFacilityChair",
            "PropFacilityChairFlip",
            "PropFacilityCouch",
            "PropFacilityTable"
        };

        [HarmonyPatch(typeof(SandboxToolParameterMenu), "ConfigureEntitySelector")]
        public static class SandboxToolParameterMenu_ConfigureEntitySelector_Patch
        {
            public static void Postfix(SandboxToolParameterMenu __instance)
            {
                var sprite = Def.GetUISprite(Assets.GetPrefab("PropFacilityCouch"));
                var parent = __instance.entitySelector.filters.First(x => x.Name == global::STRINGS.UI.SANDBOXTOOLS.FILTERS.ENTITIES.SPECIAL);

                var filter = new SearchFilter("POIs", obj => obj is KPrefabID id && tags.Contains(id.PrefabTag), parent, sprite);

                SandboxUtil.AddFilters(__instance, filter);
            }
        }
    }
}
