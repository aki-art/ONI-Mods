using Beached.Components;
using HarmonyLib;

namespace Beached.Patches
{
    internal class RationaAIPatch
    {
        // Extend duplicant AI to care about salty oxygen
        [HarmonyPatch(typeof(RationalAi), "InitializeStates")]
        public class RationalAi_InitializeStates_Patch
        {
            public static void Postfix(RationalAi __instance)
            {
                __instance.alive
                    .ToggleStateMachine(smi => new FreshAirMonitor.Instance(smi.master));
            }
        }
    }
}
