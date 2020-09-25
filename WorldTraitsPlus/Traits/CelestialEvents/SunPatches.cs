/*using Harmony;
using UnityEngine;

namespace WorldTraitsPlus.Traits.CelestialEvents
{
    class SunPatches
    {
        [HarmonyPatch(typeof(TimeOfDay), "UpdateVisuals")]
        public static class TimeOfDay_UpdateVisuals_Patch
        {
            public static void Postfix(ref float ___scale)
            {
                if(WorldEventManager.Instance.activeEclipse != null && 
                    WorldEventManager.Instance.activeEclipse.hasStarted)
                {
                    ___scale = Mathf.Lerp(___scale, 0, Time.deltaTime * 0.2f);
                    Shader.SetGlobalVector("_TimeOfDay", new Vector4(___scale, 0, 0, 0));
                }
            }
        }


        [HarmonyPatch(typeof(TimeOfDay), "UpdateSunlightIntensity")]
        public static class TimeOfDay_UpdateSunlightIntensity_Patch
        {
            public static void Postfix(ref float __result)
            {
                if (WorldEventManager.Instance.activeEclipse != null &&
                    WorldEventManager.Instance.activeEclipse.hasStarted)
                {
                    Game.Instance.currentSunlightIntensity = 0;
                    __result = 0;
                }
            }
        }
    }
}
*/