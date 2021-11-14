using HarmonyLib;

namespace DuctTapePipes.Patches
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
