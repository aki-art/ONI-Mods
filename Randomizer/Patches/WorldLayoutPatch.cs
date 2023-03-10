using Delaunay.Geo;
using HarmonyLib;
using ProcGen;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Randomizer.Patches
{
    internal class WorldLayoutPatch
    {

        [HarmonyPatch(typeof(WorldLayout), "GenerateOverworld")]
        public class TargetType_GenerateOverworld_Patch
        {
            public static void Prefix(WorldLayout __instance)
            {
                Log.Debuglog("worldlayout generateoverworld");
                Log.Debuglog(__instance.worldGen.Settings.world.filePath);
                Log.Debuglog(__instance.worldGen.Settings.world.startSubworldName);
                Log.Debuglog($"{__instance.mapWidth} x {__instance.mapHeight}");
                Log.Debuglog($"OverworldDensityMin {__instance.worldGen.Settings.GetFloatSetting("OverworldDensityMin")}");
                Log.Debuglog($"OverworldDensityMax {__instance.worldGen.Settings.GetFloatSetting("OverworldDensityMax")}");
                Log.Debuglog($"OverworldMinNodes {__instance.worldGen.Settings.GetIntSetting("OverworldMinNodes")}");
                Log.Debuglog($"OverworldMaxNodes {__instance.worldGen.Settings.GetIntSetting("OverworldMaxNodes")}");

                //var test = __instance.worldGen.Settings.GetSubWorld(__instance.worldGen.Settings.world.startSubworldName);
                //Log.Debuglog(test?.name);
                //Assert.IsNotNull(test);
                foreach (var sw in __instance.worldGen.Settings.world.subworldFiles)
                {
                    Log.Debuglog("subworld: " + sw.name);
                }

                var position = new Vector2((float)__instance.mapWidth * __instance.worldGen.Settings.world.startingBasePositionHorizontal.GetRandomValueWithinRange(__instance.myRandom), (float)__instance.mapHeight * __instance.worldGen.Settings.world.startingBasePositionVertical.GetRandomValueWithinRange(__instance.myRandom));
                var poly = new Polygon(new UnityEngine.Rect(0.0f, 0.0f, (float)__instance.mapWidth, (float)__instance.mapHeight));
                var randomPoints = PointGenerator.GetRandomPoints(poly, 
                    16,
                    __instance.worldGen.Settings.GetFloatSetting("OverworldAvoidRadius"),
                    new List<Vector2>() { position },
                    __instance.worldGen.Settings.GetEnumSetting<PointGenerator.SampleBehaviour>("OverworldSampleBehaviour"),
                    false, 
                    __instance.myRandom, 
                    false);

                Log.Debuglog("generated simul points: " + randomPoints.Count);
            }
        }
    }
}
