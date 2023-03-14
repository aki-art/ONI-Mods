using Database;
using HarmonyLib;

namespace DecorPackA.Patches
{
    public class BuildingFacadesPatch
    {
        [HarmonyPatch(typeof(BuildingFacades), MethodType.Constructor, typeof(ResourceSet))]
        public class BuildingFacades_Ctor_Patch
        {
            public static void Postfix(BuildingFacades __instance)
            {
                __instance.Add(
                    "DecorPackA_FlowerVaseHangingFancy_Colorful",
                    STRINGS.BUILDINGS.PREFABS.FLOWERVASEHANGINGFANCY.FACADES.DECORPACKA_COLORFUL.NAME,
                    STRINGS.BUILDINGS.PREFABS.FLOWERVASEHANGINGFANCY.FACADES.DECORPACKA_COLORFUL.DESC,
                    PermitRarity.Universal,
                    FlowerVaseHangingFancyConfig.ID,
                    "decorpacka_hangingvase_colorful_kanim");
            }
        }
    }
}
