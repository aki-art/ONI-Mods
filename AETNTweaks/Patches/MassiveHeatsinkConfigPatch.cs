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
                // increase internal storage
                var storage = go.GetComponent<Storage>();
                storage.capacityKg = Mathf.Max(storage.capacityKg, 10f);

                // enable logic control
                go.AddOrGet<LogicOperationalController>();

                // add controller for pyrosite attachments
                var controller = go.AddOrGet<PyrositeController>();
                controller.extraConsumptionPerPyrosite = go.GetComponent<ConduitConsumer>().capacityKG;
                controller.enabled = false;
            }
        }
    }
}
