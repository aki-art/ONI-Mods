using HarmonyLib;

namespace ZipLine.Patches
{
    public class DbPatch
    {
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
