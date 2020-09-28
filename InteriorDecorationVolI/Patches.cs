using Harmony;
using FUtility;
using System.Collections.Generic;
using System;

namespace InteriorDecorationv1
{
    class Patches
    {
        [HarmonyPatch(typeof(Localization), "Initialize")]
        class StringLocalisationPatch
        {
            public static void Postfix()
            {
                Loc.Translate(typeof(STRINGS));
            }
        }

        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                var buildings = new List<Type>()
                {
                    typeof(Buildings.GlassSculpture.GlassSculptureConfig),
                    typeof(Buildings.Aquarium.AquariumConfig),
                    typeof(Buildings.MoodLamp.MoodLampConfig)
                };

                FUtility.Buildings.RegisterBuildings(buildings);

            }
        }
    }
}
