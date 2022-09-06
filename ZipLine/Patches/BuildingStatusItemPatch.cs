using Database;
using HarmonyLib;

namespace ZipLine.Patches.DatabasePatches
{
    public class BuildingStatusItemsPatch
    {
        [HarmonyPatch(typeof(BuildingStatusItems), "CreateStatusItems")]
        public static class Database_BuildingStatusItems_CreateStatusItems_Patch
        {
            public static void Postfix()
            {
                ModDb.StatusItems.Register();
            }
        }
    }
}
