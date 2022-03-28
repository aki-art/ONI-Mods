using AETNTweaks.Cmps;
using HarmonyLib;

namespace AETNTweaks.Patches
{
    internal class MassiveHeatsinkPatch
    {

        [HarmonyPatch(typeof(MassiveHeatSink), "OnSpawn")]
        public class MassiveHeatSink_OnSpawn_Patch
        {
            public static void Prefix(MassiveHeatSink __instance)
            {
                Mod.AETNs.Add(__instance);
            }
        }

        [HarmonyPatch(typeof(MassiveHeatSink.States), "InitializeStates")]
        public class MassiveHeatSink_States_InitializeStates_Patch
        {
            public static void Postfix(MassiveHeatSink.States __instance)
            {
                // not using ToggleComponent because ay other mod adding the original component to something else would break
                __instance.active
                    .Enter(smi =>
                    {
                        if (smi.GetComponent<PyrositeController>() is KMonoBehaviour component)
                        {
                            component.enabled = true;
                        };
                    })
                    .Exit(smi =>
                    {
                        if (smi.GetComponent<PyrositeController>() is KMonoBehaviour component)
                        {
                            component.enabled = false;
                        };
                    });
            }
        }
    }
}
