using HarmonyLib;

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
                    __result = Mod.buildMenuSearch.IsNullOrWhiteSpace() || __instance.Name.ToLowerInvariant().Contains(Mod.buildMenuSearch);
                }
            }
        }
    }
}
