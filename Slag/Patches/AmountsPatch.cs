using HarmonyLib;
using Slag.Content;

namespace Slag.Patches
{
    public class AmountsPatch
    {
        [HarmonyPatch(typeof(Database.Amounts), "Load")]
        public static class Amounts_Load_Patch
        {
            public static void Postfix(Database.Amounts __instance)
            {
                SAmounts.Register(__instance);
            }
        }
    }
}
