using HarmonyLib;

namespace TwelveCyclesOfChristmas.Patches
{
    class DbPatch
    {
        [HarmonyPatch(typeof(Db), "Initialize")]
        public static class Db_Initialize_Patch
        {
            public static void Prefix()
            {
                ModAssets.LoadAssets();
            }
        }
    }
}
