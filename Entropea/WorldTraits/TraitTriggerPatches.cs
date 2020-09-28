using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Entropea.WorldTraits
{
    class TraitTriggerPatches
    {
        [HarmonyPatch(typeof(Game))]
        [HarmonyPatch("OnSpawn")]
        public static class World_OnSpawn_Patch
        {
            public static void Postfix()
            {
                //GameScheduler.Instance.Schedule("testEarthQuake", 10f, StartTestEarthQuake);
            }

            private static void StartTestEarthQuake(object obj)
            {
                var earthQuaqeController = new GameObject();
                var testEQ = earthQuaqeController.AddComponent<EarthQuake>();
                testEQ.Begin();
            }
        }
    }
}
