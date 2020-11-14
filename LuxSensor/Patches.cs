using FUtility;
using Harmony;

namespace LuxSensor
{
    class Patches
    {
        public static class Mod_OnLoad
        {
            public static void OnLoad() => Log.PrintVersion();
        }

        [HarmonyPatch(typeof(Localization), "Initialize")]
        class StringLocalisationPatch
        {
            public static void Postfix() => Loc.Translate(typeof(STRINGS));
        }

        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Postfix() => Buildings.RegisterBuildings(typeof(LogicLightSensorConfig));
        }
    }
}