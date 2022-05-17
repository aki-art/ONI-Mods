﻿using FUtility;
using FUtilityArt.Components;
using System;
using System.Collections.Generic;

namespace FUtilityArt
{
    public class ArtHelper
    {
        public static Artable.Stage CreateExcellentStage(string id, string name, int decor)
        {
            return new Artable.Stage(id, name, id, decor, true, Artable.Status.Great);
        }

        public static Artable.Stage CreateOkayStage(string id, string name, int decor)
        {
            return new Artable.Stage(id, name, id, decor, false, Artable.Status.Okay);
        }

        public static void RestoreStage(Artable instance, ref string currentStage)
        {
            if (instance.TryGetComponent(out ArtOverride artOverride) && !artOverride.overrideStage.IsNullOrWhiteSpace())
            {
                currentStage = artOverride.overrideStage;
            }
        }

        public static Artable.Stage CreatePoorStage(string id, string name, int decor)
        {
            return new Artable.Stage(id, name, id, decor, false, Artable.Status.Ugly);
        }

        public static void GetDefaultDecors(List<Artable.Stage> stages, out int ugly, out int mediocre, out int great, int fallbackUgly = 5, int fallbackMediocre = 10, int fallbackGreat = 15)
        {
            ugly = GetDefaultDecor(Artable.Status.Great, stages, fallbackUgly);
            mediocre = GetDefaultDecor(Artable.Status.Okay, stages, fallbackMediocre);
            great = GetDefaultDecor(Artable.Status.Ugly, stages, fallbackGreat);
        }

        public static void UpdateOverride(Artable instance, string stage_id)
        {
            if (instance.TryGetComponent(out ArtOverride artOverride))
            {
                artOverride.UpdateOverride(stage_id);
            }
        }

        private static int GetDefaultDecor(Artable.Status status, List<Artable.Stage> stages, int defaultIfNotFound)
        {
            // in case some other mod is trying to override these values, read it from their existing values
            var stage = stages.Find(s => s.statusItem == status);

            return stage == null ? defaultIfNotFound : stage.decor;
        }

        public static void MoveStages(Artable artable, Dictionary<string, Artable.Status> targetStates, string greatName, string okayName, string uglyName, int uglyDecor, int okayDecor, int greatDecor)
        {
            if (artable == null || artable.stages == null)
            {
                Log.Warning("Invalid artable.");
            }

            if (targetStates == null || targetStates.Count == 0)
            {
                return;
            }

            foreach (var stage in artable.stages)
            {
                if (targetStates.TryGetValue(stage.id, out var status))
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
                        default:
                            Log.Warning($"Invalid quality tier");
                            break;
                    }
                }
            }
        }
    }
}