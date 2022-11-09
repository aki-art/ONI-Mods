using Database;
using HarmonyLib;
using PrintingPodRecharge.Content;

namespace PrintingPodRecharge.Patches
{
    public class AssignableSlotsPatch
    {
        [HarmonyPatch(typeof(AssignableSlots), MethodType.Constructor)]
        public class AssignableSlots_Ctor_Patch
        {
            public static bool Prepare()
            {
                return Mod.IsTwitchIntegrationHere;
            }

            public static void Postfix(AssignableSlots __instance)
            {
                PAssignableSlots.Register(__instance);
            }
        }
    }
}
