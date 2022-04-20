using HarmonyLib;
using TUNING;
using UnityEngine;

namespace PrintingPodRecharge.Patches
{
    public class GammaMushConfigPatch
    {
        [HarmonyPatch(typeof(GammaMushConfig), "OnPrefabInit")]
        public class GammaMushConfig_OnPrefabInit_Patch
        {
            public static void Postfix(GameObject inst)
            {
                var rad = inst.AddOrGet<RadiationEmitter>();
                rad.emitRads = 3f;
                rad.emitType = RadiationEmitter.RadiationEmitterType.Constant;
                rad.emitRate = 0.5f;
                rad.emitRadiusX = 2;
                rad.emitRadiusY = 2;

                var light = inst.AddOrGet<Light2D>();
                light.Color = Color.green * 2f;
                light.Range = 2f;
                light.Angle = 0f;
                light.Direction = LIGHT2D.DEFAULT_DIRECTION;
                light.Offset = new Vector2(0, 0f);
                light.shape = LightShape.Circle;
                light.Lux = 100;
            }
        }

        [HarmonyPatch(typeof(GammaMushConfig), "OnSpawn")]
        public class GammaMushConfig_OnSpawn_Patch
        {
            public static void Postfix(GameObject inst)
            {
                inst.GetComponent<KBatchedAnimController>().TintColour = new Color(0.85f, 1f, 0.8f);
            }
        }
    }
}
