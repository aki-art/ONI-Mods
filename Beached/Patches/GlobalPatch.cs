using Beached.Components;
using HarmonyLib;

namespace Beached.Patches
{
    internal class GlobalPatch
    {
        [HarmonyPatch(typeof(Global), "Awake")]
        public static class Global_Awake_Patch
        {
            public static void Postfix(Global __instance)
            {
                __instance.FindOrAddComponent<SubZoneTypes>();
            }
        }
    }
}
