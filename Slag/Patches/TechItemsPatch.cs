using HarmonyLib;

namespace Slag.Patches
{
    internal class TechItemsPatch
    {

        [HarmonyPatch(typeof(Database.TechItems), "Init")]
        public class TechItems_Init_Patch
        {
            public static void Postfix()
            {
            }
        }
    }
}
