using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Asphalt.Bitumen
{
    public class OilRefineryPatch
    {
        [HarmonyPatch(typeof(OilRefineryConfig), "ConfigureBuildingTemplate")]
        public static class OilRefineryConfig_ConfigureBuildingTemplate_Patch
        {
            public static void Postfix(GameObject go)
            {
                DropBitumen(go.AddComponent<ElementDropper>());
                MakeBitumen(go.GetComponent<ElementConverter>());

                go.GetComponent<KPrefabID>().prefabSpawnFn += CheckForHalt;
            }

            private static void MakeBitumen(ElementConverter elementConverter)
            {
                elementConverter.outputElements = elementConverter.outputElements.Append(
                    new ElementConverter.OutputElement(
                        kgPerSecond: 5f,
                        element: SimHashes.Bitumen,
                        minOutputTemperature: 348.15f,
                        storeOutput: true,
                        outputElementOffsety: 1f));
            }

            private static void DropBitumen(ElementDropper elementDropper)
            {
                elementDropper.emitMass = 100f;
                elementDropper.emitTag = SimHashes.Bitumen.CreateTag();
            }

            private static void CheckForHalt(GameObject go)
            {
                if (ModSettings.Uninstalling)
                    StopProduction(go.GetComponent<ElementConverter>());
            }

            private static void StopProduction(ElementConverter elementConverter)
            {
                elementConverter.outputElements = elementConverter.outputElements
                    .Where(e => e.elementHash != SimHashes.Bitumen)
                    .ToArray();
            }
        }
    }              
}