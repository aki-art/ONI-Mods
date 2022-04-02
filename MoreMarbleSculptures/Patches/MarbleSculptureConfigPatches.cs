using FUtility;
using HarmonyLib;
using MoreMarbleSculptures.Components;
using System.Collections.Generic;
using UnityEngine;
using static STRINGS.BUILDINGS.PREFABS;

namespace MoreMarbleSculptures.Patches
{
    public class MarbleSculptureConfigPatches
    {
        [HarmonyPatch(typeof(MarbleSculptureConfig), "DoPostConfigureComplete")]
        public class MarbleSculptureConfig_DoPostConfigureComplete_Patch
        {
            private static int uglyDecor;
            private static int okayDecor;
            private static int greatDecor;

            public static void Prefix(GameObject go)
            {
                var overrides = go.AddComponent<ArtOverride>();
                overrides.animFileName = "mms_marble_kanim";
                overrides.offset = new Vector3(0.5f, 0f);

                overrides.fallbacks = new string[]
                {
                    "Default",
                    "Bad",
                    "Average",
                    "Good1"
                };

                overrides.extraStages = new List<Artable.Stage>()
                {
                   ArtHelper.CreateExcellentStage("dragon", MARBLESCULPTURE.EXCELLENTQUALITYNAME, greatDecor),
                   ArtHelper.CreateExcellentStage("talos", MARBLESCULPTURE.EXCELLENTQUALITYNAME, greatDecor),
                   ArtHelper.CreateExcellentStage("pacucorn", MARBLESCULPTURE.EXCELLENTQUALITYNAME, greatDecor),
                   ArtHelper.CreateExcellentStage("smugpip", MARBLESCULPTURE.EXCELLENTQUALITYNAME, greatDecor),
                   ArtHelper.CreateExcellentStage("panda", MARBLESCULPTURE.EXCELLENTQUALITYNAME, greatDecor),
                   ArtHelper.CreateExcellentStage("dashmaster", MARBLESCULPTURE.EXCELLENTQUALITYNAME, greatDecor),

                   ArtHelper.CreatePoorStage("smile", MARBLESCULPTURE.POORQUALITYNAME, uglyDecor)
                };
            }

            public static void Postfix(GameObject go)
            {
                if(go.TryGetComponent(out Sculpture sculpture))
                {
                    ArtHelper.GetDefaultDecors(sculpture.stages, 5, 10, 15, out greatDecor, out okayDecor, out uglyDecor);

                    ArtHelper.MoveStages(
                        sculpture, 
                        Mod.Settings.MoveSculptures,
                        MARBLESCULPTURE.POORQUALITYNAME, 
                        MARBLESCULPTURE.AVERAGEQUALITYNAME, 
                        MARBLESCULPTURE.EXCELLENTQUALITYNAME,
                        uglyDecor, 
                        okayDecor, 
                        greatDecor);

                    return;
                }

                Log.Warning("Marble Sculpture has no Sculpture component.");
            }
        }
    }
}
