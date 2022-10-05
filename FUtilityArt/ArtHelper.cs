using Database;
using FUtility;
using HarmonyLib;
using System.Collections.Generic;

namespace FUtilityArt
{
    public class ArtHelper
    {
        public const string READY = "AwaitingArting";
        public const string UGLY = "LookingUgly";
        public const string OKAY = "LookingOkay";
        public const string GREAT = "LookingGreat";

        public static void GetDefaultDecors(ArtableStages artableStages, string id, out int ugly, out int mediocre, out int great, int fallbackUgly = 5, int fallbackMediocre = 10, int fallbackGreat = 15)
        {
            var stages = artableStages.GetPrefabStages(id);

            ugly = GetDefaultDecor(GREAT, stages, fallbackUgly);
            mediocre = GetDefaultDecor(OKAY, stages, fallbackMediocre);
            great = GetDefaultDecor(UGLY, stages, fallbackGreat);
        }

        private static int GetDefaultDecor(string status, List<ArtableStage> stages, int defaultIfNotFound)
        {
            // in case some other mod is trying to override these values, read it from their existing values
            var stage = stages.Find(s => s.statusItem?.Id == status);
            return stage == null ? defaultIfNotFound : stage.decor;
        }

        public static void MoveStages(List<ArtableStage> stages, Dictionary<string, string> targetStates, string greatName, string okayName, string uglyName, int uglyDecor, int okayDecor, int greatDecor)
        {
            if (stages == null)
            {
                Log.Warning("Invalid artable.");
            }

            if (targetStates == null || targetStates.Count == 0)
            {
                Log.Debuglog("no targetstates");
                return;
            }

            var statusItems = Db.Get().ArtableStatuses;

            var idLookup = new Dictionary<string, string>()
            {
                { "Bad", "MarbleSculpture_Bad" },
                { "Average", "MarbleSculpture_Average" },
                { "Good1", "MarbleSculpture_Good1" },
                { "Good2", "MarbleSculpture_Good2" },
                { "Good3", "MarbleSculpture_Good3" },
            };


            var statusLookup = new Dictionary<string, string>()
            {
                { "Bad", "LookingUgly" },
                { "Average", "LookingOkay" },
                { "Great", "LookingGreat" },
            };

            foreach (var stage in stages)
            {
                // convert to new naming scheme
                var id = idLookup.TryGetValue(stage.Id, out var i) ? i : stage.Id;

                Log.Debuglog($"remapping stage id from {stage.id} to {id}");

                if (targetStates.TryGetValue(stage.id, out var status))
                {
                    // convert to new naming scheme
                    if (statusLookup.TryGetValue(status, out var statusId))
                    {
                        Log.Debuglog($"remapping status id from {status} to {statusId}");
                        status = statusId;
                    }

                    var statusItem = statusItems.TryGet(status);

                    if(statusItem == null)
                    {
                        Log.Warning($"{statusId} is not a valid sculpture quality.");
                        continue;
                    }

                    stage.statusItem = statusItem;
                    
                    switch (status)
                    {
                        case "Ugly":
                            stage.name = uglyName;
                            stage.decor = uglyDecor;
                            stage.cheerOnComplete = false;
                            break;
                        case "Okay":
                            stage.name = okayName;
                            stage.decor = okayDecor;
                            stage.cheerOnComplete = false;
                            break;
                        case "Great":
                            stage.name = greatName;
                            stage.decor = greatDecor;
                            stage.cheerOnComplete = true;
                            break;
                        default:
                            Log.Warning($"Invalid quality tier");
                            break;
                    }

                    Log.Debuglog($"rearranged sculpture {stage.id} to {stage.statusItem.Id}");
                }
            }
        }
    }
}
