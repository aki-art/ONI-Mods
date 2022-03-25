using HarmonyLib;

namespace AETNTweaks.Patches
{
    internal class DBPatch
    {
        [HarmonyPatch(typeof(Db), "Initialize")]
        public static class Db_Initialize_Patch
        {
            public static void Prefix()
            {
                ModAssets.LateLoadAssets();
            }
        }
    }
}
