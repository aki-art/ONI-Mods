using FUtility;
using HarmonyLib;

namespace PrintingPodRecharge.Patches
{
    public class SandboxToolParameterMenuPatch
    {
        [HarmonyPatch(typeof(SandboxToolParameterMenu), "ConfigureEntitySelector")]
        public static class SandboxToolParameterMenu_ConfigureEntitySelector_Patch
        {
            public static void Postfix(SandboxToolParameterMenu __instance)
            {
                var bioInkFilter = SandboxUtil.CreateSimpleTagFilter(
                    __instance,
                    STRINGS.UI.SANBOXTOOLS.FILTERS.BIO_INKS,
                    ModAssets.Tags.bioInk,
                    Items.BioInkConfig.DEFAULT);

                SandboxUtil.AddFilters(__instance, bioInkFilter);
            }
        }
    }
}
