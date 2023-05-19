using FUtility;
using HarmonyLib;
using Klei.AI;
using System;
using System.Collections.Generic;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events
{
    public class DoubleTroubleEvent : ITwitchEvent
    {
        public const string ID = "DoubleTrouble";

        public bool Condition(object data) => true;

        public string GetID() => ID;

        public void Run(object data)
        {
            if (AkisTwitchEvents.Instance == null)
            {
                Log.Warning("AkisTwitchEvents.Instance is null.");
                return;
            }

            foreach (MinionIdentity minion in Components.LiveMinionIdentities)
            {
                CreateCopy(minion);
            }

            AkisTwitchEvents.Instance.dupedDupePurgeTime = GameClock.Instance.GetTimeInCycles() + 0.3f; // TODO: configurable cycle

            ONITwitchLib.ToastManager.InstantiateToast("Double Trouble", "Duped your dupes!");
        }

        private void CreateCopy(MinionIdentity original)
        {
            var prefab = Assets.GetPrefab(MinionConfig.ID);
            var newMinion = Util.KInstantiate(prefab, null, prefab.name);

            Immigration.Instance.ApplyDefaultPersonalPriorities(newMinion);

            newMinion.transform.SetLocalPosition(original.transform.position);
            newMinion.SetActive(true);
/*
            var stats = new MinionStartingStats(false);
            stats.Apply(newMinion);
*/
            GameScheduler.Instance.ScheduleNextFrame("", _ =>
            {
                var newIdentity = newMinion.GetComponent<MinionIdentity>();
                CopyMinion(original, newIdentity);
                newIdentity.gameObject.AddComponent<AETE_DuplicatedDupe>().remainingLifeTimeSeconds = 30f;
            });

        }

        private static void CopyMinion(MinionIdentity sourceIdentity, MinionIdentity destinationIdentity)
        {
            Log.Assert("source", sourceIdentity);
            Log.Assert("dest", destinationIdentity);

            destinationIdentity.SetName(sourceIdentity.name);
            destinationIdentity.nameStringKey = sourceIdentity.nameStringKey;
            destinationIdentity.personalityResourceId = sourceIdentity.personalityResourceId;
            destinationIdentity.gender = sourceIdentity.gender;
            destinationIdentity.genderStringKey = sourceIdentity.genderStringKey;
            destinationIdentity.arrivalTime = sourceIdentity.arrivalTime;
            destinationIdentity.voiceIdx = sourceIdentity.voiceIdx;

            CopyBodyData(sourceIdentity, destinationIdentity);

            if (sourceIdentity.TryGetComponent(out Traits srcTraits) && srcTraits.TraitIds != null)
                destinationIdentity.GetComponent<Traits>().SetTraitIds(new List<string>(srcTraits.TraitIds));

            /*
            if (sourceIdentity.TryGetComponent(out Accessorizer srcAccessorizer) && srcAccessorizer != null)
                destinationIdentity.GetComponent<Accessorizer>().SetAccessories(srcAccessorizer.accessories);

            if (sourceIdentity.TryGetComponent(out WearableAccessorizer srcWearableAccessorizer))
            {
                destinationIdentity.GetComponent<WearableAccessorizer>()
                    .RestoreWearables(srcWearableAccessorizer.wearables, srcWearableAccessorizer.clothingItems);
            }

            if (sourceIdentity.TryGetComponent(out ConsumableConsumer consumableConsumer))
            {
                ConsumableConsumer component1 = destinationIdentity.GetComponent<ConsumableConsumer>();
                if (consumableConsumer.forbiddenTagSet != null)
                    component1.forbiddenTagSet = new HashSet<Tag>(consumableConsumer.forbiddenTagSet);
            }

            if (sourceIdentity.TryGetComponent(out MinionResume srcMinionResume) && srcMinionResume.MasteryBySkillID != null)
            {
                var dstMinionResume = destinationIdentity.GetComponent<MinionResume>();
                dstMinionResume.RestoreResume(srcMinionResume.MasteryBySkillID, srcMinionResume.AptitudeBySkillGroup, srcMinionResume.GrantedSkillIDs, srcMinionResume.TotalExperienceGained);
                dstMinionResume.SetHats(srcMinionResume.currentHat, srcMinionResume.targetHat);
            }

            if (sourceIdentity.TryGetComponent(out ChoreConsumer srcChoreConsumer) && srcChoreConsumer.choreGroupPriorities != null)
                destinationIdentity.GetComponent<ChoreConsumer>().SetChoreGroupPriorities(srcChoreConsumer.choreGroupPriorities);

            if (sourceIdentity.TryGetComponent(out AttributeLevels srcAttributeLevels) && srcAttributeLevels.levels != null)
            {
                var dstAttributeLevels = destinationIdentity.GetComponent<AttributeLevels>();
                dstAttributeLevels.SaveLoadLevels = srcAttributeLevels.saveLoadLevels;
                dstAttributeLevels.OnDeserialized();
            }

            if (sourceIdentity.TryGetComponent(out Effects srcEffects) && srcEffects.saveLoadImmunities != null)
            {
                var dstEffects = destinationIdentity.GetComponent<Effects>();
                var effects = Db.Get().effects;

                foreach (var saveLoadImmunity in srcEffects.saveLoadImmunities)
                {
                    if (effects.Exists(saveLoadImmunity.effectID))
                    {
                        var effect = effects.Get(saveLoadImmunity.effectID);
                        dstEffects.AddImmunity(effect, saveLoadImmunity.giverID, saveLoadImmunity.saved);
                    }
                }

                if (srcEffects.saveLoadEffects != null)
                {
                    foreach (Effects.SaveLoadEffect saveLoadEffect in srcEffects.saveLoadEffects)
                    {
                        if (effects.Exists(saveLoadEffect.id))
                        {
                            var effect = effects.Get(saveLoadEffect.id);
                            var effectInstance = dstEffects.Add(effect, saveLoadEffect.saved);

                            if (effectInstance != null)
                                effectInstance.timeRemaining = saveLoadEffect.timeRemaining;
                        }
                    }
                }
            }

            */

            destinationIdentity.GetComponent<Accessorizer>().ApplyAccessories();
            /*
            destinationIdentity.assignableProxy = new Ref<MinionAssignablesProxy>();
            destinationIdentity.assignableProxy.Set(sourceIdentity.assignableProxy.Get());
            destinationIdentity.assignableProxy.Get().SetTarget(destinationIdentity, destinationIdentity.gameObject);

            var equipment = destinationIdentity.GetEquipment();

            foreach (var slot in equipment.Slots)
            {
                var assignable = slot.assignable as Equippable;
                if (assignable != null)
                    equipment.Equip(assignable);
            }

            var srcSchedulable = sourceIdentity.GetComponent<Schedulable>();
            var schedule = srcSchedulable.GetSchedule();

            if (schedule == null)
                return;

            schedule.Unassign(srcSchedulable);

            var dstSchedulable = destinationIdentity.GetComponent<Schedulable>();
            schedule.Assign(dstSchedulable);
            */
        }

        private static void CopyBodyData(MinionIdentity sourceIdentity, MinionIdentity destinationIdentity)
        {
            var srcBodyData = sourceIdentity.GetComponent<Accessorizer>().bodyData;
            var newBodyData = new KCompBuilder.BodyData();

            Traverse.CopyFields(Traverse.Create(srcBodyData), Traverse.Create(newBodyData));
            destinationIdentity.GetComponent<Accessorizer>().bodyData = newBodyData;
        }
    }
}
