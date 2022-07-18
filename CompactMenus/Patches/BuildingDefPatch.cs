using HarmonyLib;
using STRINGS;

namespace CompactMenus.Patches
{
    public class BuildingDefPatch
    {
        [HarmonyPatch(typeof(BuildingDef), "IsAvailable")]
        public class BuildingDef_IsAvailable_Patch
        {
            public static void Postfix(BuildingDef __instance, ref bool __result)
            {
                if(__result)
                {
                    var name = UI.StripLinkFormatting(__instance.Name).ToLowerInvariant();
                    __result = Mod.buildMenuSearch.IsNullOrWhiteSpace() || name.Contains(Mod.buildMenuSearch);
                }
            }
        }
    }
}
