using HarmonyLib;

namespace Beached.Patches
{
    public class CreatureStatusItems
    {
        [HarmonyPatch(typeof(Database.CreatureStatusItems), "CreateStatusItems")]
        public static class Database_CreatureStatusItems_CreateStatusItems_Patch
        {
            public static void Postfix()
            {
                ModAssets.StatusItems.Register();
            }
        }
    }
}
