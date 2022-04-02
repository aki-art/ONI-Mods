using FUtility;
using System.Collections.Generic;

namespace MoreMarbleSculptures
{
    public class ArtHelper
    {
        // Use slab as default anim, because i replace the whole kanim later
        public static Artable.Stage CreateExcellentStage(string id, string name, int decor)
        {
            return new Artable.Stage(id, name, "slab", decor, true, Artable.Status.Great);
        }

        public static Artable.Stage CreateOkayStage(string id, string name, int decor)
        {
            return new Artable.Stage(id, name, "slab", decor, false, Artable.Status.Okay);
        }

        public static Artable.Stage CreatePoorStage(string id, string name, int decor)
        {
            return new Artable.Stage(id, name, "slab", decor, false, Artable.Status.Ugly);
        }

        public static void GetDefaultDecors(List<Artable.Stage> stages, int fallbackUgly, int fallbackMediocre, int fallbackGreat, out int ugly, out int mediocre, out int great)
        {
            ugly = GetDefaultDecor(Artable.Status.Great, stages, fallbackUgly);
            mediocre = GetDefaultDecor(Artable.Status.Okay, stages, fallbackMediocre);
            great = ArtHelper.GetDefaultDecor(Artable.Status.Ugly, stages, fallbackGreat);
        }

        private static int GetDefaultDecor(Artable.Status status, List<Artable.Stage> stages, int defaultIfNotFound = 5)
        {
            // in case some other mod is trying to override these values, read it from their existing values
            var stage = stages.Find(s => s.statusItem == status);

            return stage == null ? defaultIfNotFound : stage.decor;
        }

        public static void MoveStages(Artable artable, Dictionary<string, Artable.Status> targetStates, string greatName, string okayName, string uglyName, int uglyDecor, int okayDecor, int greatDecor)
        {
            if(artable == null || artable.stages == null)
            {
                Log.Warning("Invalid artable.");
            }

            if(targetStates == null || targetStates.Count == 0)
            {
                return;
            }

            foreach (var stage in artable.stages)
            {
                if (targetStates.TryGetValue(stage.id, out Artable.Status status))
                {
                    stage.statusItem = status;

                    switch (status)
                    {
                        case Artable.Status.Ugly:
                            stage.name = uglyName;
                            stage.decor = uglyDecor;
                            stage.cheerOnComplete = false;
                            break;
                        case Artable.Status.Okay:
                            stage.name = okayName;
                            stage.decor = okayDecor;
                            stage.cheerOnComplete = false;
                            break;
                        case Artable.Status.Great:
                            stage.name = greatName;
                            stage.decor = greatDecor;
                            stage.cheerOnComplete = true;
                            break;
                    }
                }
            }
        }
    }
}
