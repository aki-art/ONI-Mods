using HarmonyLib;

namespace CrittersDropBones.Patches
{
    public class BuildingStatusItemsPatch
    {
        [HarmonyPatch(typeof(Database.BuildingStatusItems), "CreateStatusItems")]
        public static class Database_BuildingStatusItems_CreateStatusItems_Patch
        {
            public static void Postfix()
            {
                ModAssets.StatusItems.Register();
            }
        }
    }
}
