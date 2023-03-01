using Database;
using GravitasBigStorage.Content;
using HarmonyLib;

namespace GravitasBigStorage.Patches
{
    public class BuildingFacadesPatch
    {
        [HarmonyPatch(typeof(BuildingFacades), MethodType.Constructor, typeof(ResourceSet))]
        public class BuildingFacades_Ctor_Patch
        {
            public static void Postfix(BuildingFacades __instance)
            {
                __instance.Add(
                    "GravitasBigStorage_Container_Alien",
                    STRINGS.BUILDINGS.PREFABS.GRAVITASBIGSTORAGE_CONTAINER.FACADES.ALIEN.NAME,
                    STRINGS.BUILDINGS.PREFABS.GRAVITASBIGSTORAGE_CONTAINER.FACADES.ALIEN.DESC,
                    PermitRarity.Universal,
                    GravitasBigStorageConfig.ID,
                    "gravitasbigstorage_container_alien_kanim");

                __instance.Add(
                    "GravitasBigStorage_Container_Padded",
                    STRINGS.BUILDINGS.PREFABS.GRAVITASBIGSTORAGE_CONTAINER.FACADES.PADDED.NAME,
                    STRINGS.BUILDINGS.PREFABS.GRAVITASBIGSTORAGE_CONTAINER.FACADES.PADDED.DESC,
                    PermitRarity.Universal,
                    GravitasBigStorageConfig.ID,
                    "gravitasbigstorage_container_padded_kanim");

                __instance.Add(
                    "GravitasBigStorage_Container_Red",
                    STRINGS.BUILDINGS.PREFABS.GRAVITASBIGSTORAGE_CONTAINER.FACADES.RED.NAME,
                    STRINGS.BUILDINGS.PREFABS.GRAVITASBIGSTORAGE_CONTAINER.FACADES.RED.DESC,
                    PermitRarity.Universal,
                    GravitasBigStorageConfig.ID,
                    "gravitasbigstorage_container_red_kanim");

                __instance.Add(
                    "GravitasBigStorage_Container_Retro",
                    STRINGS.BUILDINGS.PREFABS.GRAVITASBIGSTORAGE_CONTAINER.FACADES.RETRO.NAME,
                    STRINGS.BUILDINGS.PREFABS.GRAVITASBIGSTORAGE_CONTAINER.FACADES.RETRO.DESC,
                    PermitRarity.Universal,
                    GravitasBigStorageConfig.ID,
                    "gravitasbigstorage_container_retro_kanim");

                __instance.Add(
                    "GravitasBigStorage_Container_Rusty",
                    STRINGS.BUILDINGS.PREFABS.GRAVITASBIGSTORAGE_CONTAINER.FACADES.RUSTY.NAME,
                    STRINGS.BUILDINGS.PREFABS.GRAVITASBIGSTORAGE_CONTAINER.FACADES.RUSTY.DESC,
                    PermitRarity.Universal,
                    GravitasBigStorageConfig.ID,
                    "gravitasbigstorage_container_rusty_kanim");

                __instance.Add(
                    "GravitasBigStorage_Container_Starry",
                    STRINGS.BUILDINGS.PREFABS.GRAVITASBIGSTORAGE_CONTAINER.FACADES.STARRY.NAME,
                    STRINGS.BUILDINGS.PREFABS.GRAVITASBIGSTORAGE_CONTAINER.FACADES.STARRY.DESC,
                    PermitRarity.Universal,
                    GravitasBigStorageConfig.ID,
                    "gravitasbigstorage_container_starry_kanim");
            }
        }
    }
}
