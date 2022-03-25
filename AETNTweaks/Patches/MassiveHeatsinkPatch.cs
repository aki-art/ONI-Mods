using AETNTweaks.Components;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AETNTweaks.Patches
{
    internal class MassiveHeatsinkPatch
    {

        [HarmonyPatch(typeof(MassiveHeatSink.States), "InitializeStates")]
        public class MassiveHeatSink_States_InitializeStates_Patch
        {
            public static void Postfix(MassiveHeatSink.States __instance)
            {
                // not using ToggleComponent because ay other mod adding the original component to something else would break
                __instance.active
                    .Enter(smi =>
                    {
                        if (smi.GetComponent<PulseController>() is KMonoBehaviour component)
                        {
                            component.enabled = true;
                        };
                    })
                    .Exit(smi =>
                    {
                        if (smi.GetComponent<PulseController>() is KMonoBehaviour component)
                        {
                            component.enabled = false;
                        };
                    });
            }
        }
    }
}
