using HarmonyLib;
using Slag.Cmps;
using Slag.Content.Entities;
using UnityEngine;

namespace Slag.Patches
{
    public class FlyingCreatureBaitConfigPatch
    {
        [HarmonyPatch(typeof(FlyingCreatureBaitConfig), "DoPostConfigureComplete")]
        public class FlyingCreatureBaitConfig_DoPostConfigureComplete_Patch
        {
            public static void Postfix(GameObject go)
            {
                var spawner = go.AddOrGet<MiteorSpawner>();
                spawner.prefabID = EggCometConfig.ID;
                spawner.duration = 25f;
                spawner.spread = 10;
                spawner.coolDown = Mod.Settings.MiteorEventCooldownInCycles;
            }
        }
    }
}
