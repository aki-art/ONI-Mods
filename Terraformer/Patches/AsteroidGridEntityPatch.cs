using HarmonyLib;

namespace Terraformer.Patches
{
    public class AsteroidGridEntityPatch
    {
        [HarmonyPatch(typeof(AsteroidGridEntity), "OnSpawn")]
        public class Localization_Initialize_Patch
        {
            public static void Postfix(AsteroidGridEntity __instance)
            {
                __instance.FindOrAdd<InstantDestroyablePlanet>();
            }
        }
    }
}
