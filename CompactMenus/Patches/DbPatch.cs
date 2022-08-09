using HarmonyLib;

namespace CompactMenus.Patches
{
    public class DbPatch
    {
        [HarmonyPriority(Priority.Last)]
        [HarmonyPatch(typeof(Db), "Initialize")]
        public class Db_Initialize_Patch
        {
            public static void Postfix()
            {
                ModAssets.LoadAssets();
            }
        }
    }
}
