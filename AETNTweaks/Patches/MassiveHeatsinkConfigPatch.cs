using AETNTweaks.Components;
using HarmonyLib;
using UnityEngine;

namespace AETNTweaks.Patches
{
    // MassiveHeatSink = AETN
    public class MassiveHeatsinkConfigPatch
    {

        [HarmonyPatch(typeof(MassiveHeatSinkConfig), "CreateBuildingDef")]
        public class MassiveHeatSinkConfig_CreateBuildingDef_Patch
        {
            public static void Postfix(ref BuildingDef __result)
            {
                __result.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(1, 1));
            }
        }
        [HarmonyPatch(typeof(MassiveHeatSinkConfig), "DoPostConfigureComplete")]
        public class MassiveHeatSinkConfig_DoPostConfigureComplete_Patch
        {
            public static void Postfix(GameObject go)
            {
                go.AddOrGet<LogicOperationalController>();

                var pulseController = go.AddOrGet<PulseController>();
                pulseController.pulseFrequency = Mod.Settings.PulseFrequency;
                pulseController.range = 8;
                pulseController.enabled = false; // controlled by MassiveHeatSink statemachine (MassiveHeatSinkPatch)

                var vis = go.AddOrGet<PulseVisualizer>();
                vis.fxAnimName = "snore_fx_kanim";
                vis.offset = new Vector3(0.5f, 3);
            }
        }
    }
}
