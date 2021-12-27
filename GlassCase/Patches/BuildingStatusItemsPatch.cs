using HarmonyLib;

namespace GlassCase.Patches
{
    [HarmonyPatch(typeof(Database.BuildingStatusItems), "CreateStatusItems")]
    public static class Database_BuildingStatusItems_CreateStatusItems_Patch
    {
        public static void Postfix()
        {
            ModAssets.CreateStatusItem();
        }
    }
}
