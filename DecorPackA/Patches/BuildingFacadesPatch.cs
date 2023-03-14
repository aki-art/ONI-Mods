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
                AddPotFacade(
                    __instance,
                    "DecorPackA_FlowerVaseHangingFancy_Colorful",
                    STRINGS.BUILDINGS.PREFABS.FLOWERVASEHANGINGFANCY.FACADES.DECORPACKA_COLORFUL.NAME,
                    STRINGS.BUILDINGS.PREFABS.FLOWERVASEHANGINGFANCY.FACADES.DECORPACKA_COLORFUL.DESC,
                    "decorpacka_hangingvase_colorful_kanim");

                AddPotFacade(
                    __instance,
                    "DecorPackA_FlowerVaseHangingFancy_BlueYellow",
                    STRINGS.BUILDINGS.PREFABS.FLOWERVASEHANGINGFANCY.FACADES.DECORPACKA_BLUEYELLOW.NAME,
                    STRINGS.BUILDINGS.PREFABS.FLOWERVASEHANGINGFANCY.FACADES.DECORPACKA_BLUEYELLOW.DESC,
                    "decorpacka_hangingvase_blueyellow_kanim");

                AddPotFacade(
                    __instance,
                    "DecorPackA_FlowerVaseHangingFancy_ShoveVole",
                    STRINGS.BUILDINGS.PREFABS.FLOWERVASEHANGINGFANCY.FACADES.DECORPACKA_SHOVEVOLE.NAME,
                    STRINGS.BUILDINGS.PREFABS.FLOWERVASEHANGINGFANCY.FACADES.DECORPACKA_SHOVEVOLE.DESC,
                    "decorpacka_hangingvase_shovevoleb_kanim");

                AddPotFacade(
                    __instance,
                    "DecorPackA_FlowerVaseHangingFancy_Honey",
                    STRINGS.BUILDINGS.PREFABS.FLOWERVASEHANGINGFANCY.FACADES.DECORPACKA_HONEY.NAME,
                    STRINGS.BUILDINGS.PREFABS.FLOWERVASEHANGINGFANCY.FACADES.DECORPACKA_HONEY.DESC,
                    "decorpacka_hangingvase_honey_kanim");

                AddPotFacade(
                    __instance,
                    "DecorPackA_FlowerVaseHangingFancy_Uranium",
                    STRINGS.BUILDINGS.PREFABS.FLOWERVASEHANGINGFANCY.FACADES.DECORPACKA_URANIUM.NAME,
                    STRINGS.BUILDINGS.PREFABS.FLOWERVASEHANGINGFANCY.FACADES.DECORPACKA_URANIUM.DESC,
                    "decorpacka_hangingvase_uranium_kanim");
            }

            private static void AddPotFacade(BuildingFacades __instance, string id, string name, string desc, string anim)
            {
                __instance.Add(
                    id,
                    name,
                    desc,
                    PermitRarity.Universal,
                    FlowerVaseHangingFancyConfig.ID,
                    anim);

                ModDb.myFacades.Add(id);
            }
        }
    }
}
