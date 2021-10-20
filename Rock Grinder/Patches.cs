using FUtility;
using Harmony;

namespace RockGrinder
{
    class Patches
    {
        public static class Mod_OnLoad
        {
            public static void OnLoad() => Log.PrintVersion();
        }


        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Postfix() => Buildings.RegisterBuildings(typeof(RockGrinderConfig));
        }

        [HarmonyPatch(typeof(Localization), "Initialize")]
        class Localization_Initialize_Patch
        {
            public static void Postfix() => Loc.Translate(typeof(STRINGS));
        }
    }
}