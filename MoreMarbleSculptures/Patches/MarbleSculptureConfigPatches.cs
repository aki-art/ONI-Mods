using HarmonyLib;
using MoreMarbleSculptures.FUtilityArt.Components;
using UnityEngine;

namespace MoreMarbleSculptures.Patches
{
    public class MarbleSculptureConfigPatches
    {
        [HarmonyPatch(typeof(MarbleSculptureConfig), "DoPostConfigureComplete")]
        public class MarbleSculptureConfig_DoPostConfigureComplete_Patch
        {
            public static void Postfix(GameObject go)
            {
                go.AddComponent<ArtOverride>();
                go.AddComponent<ArtOverrideRestorer>().fallback = "Default";
                go.GetComponent<KPrefabID>().prefabSpawnFn += g => Mod.artRestorers.Add(g.GetComponent<ArtOverrideRestorer>());
            }
        }
    }
}
