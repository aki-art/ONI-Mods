using Harmony;
using FUtility;
using System.Collections.Generic;
using System;
using InteriorDecorationVolI.Buildings;
using UnityEngine;

namespace InteriorDecorationVolI
{
    class Patches
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                var buildings = new List<Type>()
                {
                    typeof(Buildings.GlassSculpture.GlassSculptureConfig),
                    //typeof(Buildings.Aquarium.AquariumConfig)
                    typeof(Buildings.MoodLamp.MoodLampConfig)
                };

                FUtility.Buildings.RegisterBuildings(buildings);

            }
        }
    }
}
