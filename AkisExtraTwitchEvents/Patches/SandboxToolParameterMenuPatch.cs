using FUtility;
using HarmonyLib;
using static SandboxToolParameterMenu.SelectorValue;
using System.Collections.Generic;
using Twitchery.Content.Defs;

namespace Twitchery.Patches
{
    public class SandboxToolParameterMenuPatch
    {
        private static readonly HashSet<Tag> items = new()
        {
            GiantRadishConfig.ID,
            PizzaBoxConfig.ID
        };

        [HarmonyPatch(typeof(SandboxToolParameterMenu), "ConfigureEntitySelector")]
        public static class SandboxToolParameterMenu_ConfigureEntitySelector_Patch
        {
            public static void Postfix(SandboxToolParameterMenu __instance)
            {
                var sprite = Def.GetUISprite(Assets.GetPrefab(GiantRadishConfig.ID));

                var filter = new SearchFilter("Extra Twitch Events", obj => obj is KPrefabID id && items.Contains(id.PrefabTag), null, sprite);

                SandboxUtil.AddFilters(__instance, filter);
            }
        }
    }
}
