using HarmonyLib;
using MayISit.Content.Scripts;
using UnityEngine;

namespace MayISit.Patches
{
    public class PropFacilityCouchConfigPatch
    {
        [HarmonyPatch(typeof(PropFacilityCouchConfig), "OnSpawn")]
        public class PropFacilityCouchConfig_OnSpawn_Patch
        {
            public static void Postfix(GameObject inst)
            {
                inst.AddOrGet<Seat>();
            }
        }
    }
}
