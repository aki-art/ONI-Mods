using FUtility;
using GravitasBigStorage.Content;
using HarmonyLib;

namespace GravitasBigStorage.Patches
{
    public class GeneratedBuildingsPatch
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                ModUtil.AddBuildingToPlanScreen(
                    Consts.BUILD_CATEGORY.BASE,
                    GravitasBigStorageConfig.ID,
                    Consts.SUB_BUILD_CATEGORY.Base.STORAGE,
                    StorageLockerConfig.ID);

            }
        }
    }
}
