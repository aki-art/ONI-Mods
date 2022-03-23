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
                    new Artable.Stage("dragon", MARBLESCULPTURE.EXCELLENTQUALITYNAME, "slab", greatDecor, true, Artable.Status.Great),
                    new Artable.Stage("talos", MARBLESCULPTURE.EXCELLENTQUALITYNAME, "slab", greatDecor, true, Artable.Status.Great),
                    new Artable.Stage("pacucorn", MARBLESCULPTURE.EXCELLENTQUALITYNAME, "slab", greatDecor, true, Artable.Status.Great),
                    new Artable.Stage("smugpip", MARBLESCULPTURE.EXCELLENTQUALITYNAME, "slab", greatDecor, true, Artable.Status.Great)
                    ,
                    new Artable.Stage("smile", MARBLESCULPTURE.POORQUALITYNAME, "slab", uglyDecor, true, Artable.Status.Ugly),
                };

                var paintable = go.AddOrGet<Paintable>();
                paintable.offset = Vector3.zero;
                paintable.overLays = new HashSet<string>() 
                { 
                    "Good1" 
                };
                paintable.overlayAnim = "mms_marble_overlay";
            }

            public static void Postfix(GameObject go)
            {
                if(Mod.Settings.MoveSculptures == null || Mod.Settings.MoveSculptures.Count == 0)
                {
                    return;
                }

                if(go.TryGetComponent(out Sculpture sculpture))
                {
                    var stages = sculpture.stages;

                    greatDecor = GetDefaultDecor(Artable.Status.Great, stages);
                    okayDecor = GetDefaultDecor(Artable.Status.Okay, stages);
                    uglyDecor = GetDefaultDecor(Artable.Status.Ugly, stages);

                    MoveStages(sculpture);
                    return;
                }

                Log.Warning("Marble Sculpture has no Sculpture component.");
            }

            private static void MoveStages(Sculpture sculpture)
            {
                foreach (var stage in sculpture.stages)
                {
                    if (Mod.Settings.MoveSculptures.TryGetValue(stage.id, out Artable.Status status))
                    {
                        stage.statusItem = status;

                        switch (status)
                        {
                            case Artable.Status.Ugly:
                                stage.name = MARBLESCULPTURE.POORQUALITYNAME;
                                stage.decor = uglyDecor;
                                stage.cheerOnComplete = false;
                                break;
                            case Artable.Status.Okay:
                                stage.name = MARBLESCULPTURE.AVERAGEQUALITYNAME;
                                stage.decor = okayDecor;
                                stage.cheerOnComplete = false;
                                break;
                            case Artable.Status.Great:
                                stage.name = MARBLESCULPTURE.EXCELLENTQUALITYNAME;
                                stage.decor = greatDecor;
                                stage.cheerOnComplete = true;
                                break;
                        }
                    }
                }
            }

            private static int GetDefaultDecor(Artable.Status status, List<Artable.Stage> stages)
            {
                // in case some other mod is trying to override these values, read it from their existing values
                var stage = stages.Find(s => s.statusItem == status);
                // default vanilla values happen to be 3 times the status tier, so that's convenient
                return stage is null ? (int)status * 5 : stage.decor; 

            }
        }
    }
}
