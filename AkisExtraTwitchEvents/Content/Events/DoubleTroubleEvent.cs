using FUtility;
using HarmonyLib;
using Klei.AI;
using System;
using System.Collections.Generic;
using TUNING;
using Twitchery.Content.Scripts;
using UnityEngine;
using YamlDotNet.Core.Tokens;

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

/*		private void CreateCopy(MinionIdentity original)
		{
			var prefab = Assets.GetPrefab(MinionConfig.ID);
			var newMinion = Util.KInstantiate(prefab, null, prefab.name);

			Immigration.Instance.ApplyDefaultPersonalPriorities(newMinion);

			newMinion.transform.SetLocalPosition(original.transform.position);
			newMinion.SetActive(true);
			*//*
						var stats = new MinionStartingStats(false);
						stats.Apply(newMinion);
			*//*
			GameScheduler.Instance.ScheduleNextFrame("", _ =>
			{
				var newIdentity = newMinion.GetComponent<MinionIdentity>();
				CopyMinion(original, newIdentity);
				newIdentity.gameObject.AddComponent<AETE_DuplicatedDupe>().remainingLifeTimeSeconds = 30f;
			});

		}
*/
		private static void CreateCopy(MinionIdentity srcIdentity)
		{
			var personality = Db.Get().Personalities.TryGet(srcIdentity.personalityResourceId);

			if (personality == null)
				return;

			var minionStartingStats = new MinionStartingStats(personality);

			if (srcIdentity.TryGetComponent(out Traits traits))
				minionStartingStats.Traits = new List<Trait>(traits.TraitList);

			minionStartingStats.voiceIdx = srcIdentity.voiceIdx;

			if (srcIdentity.TryGetComponent(out Attributes attributes))
			{
				foreach (string key in DUPLICANTSTATS.ALL_ATTRIBUTES)
				{
					var attribute = attributes.GetValue(key);
					minionStartingStats.StartingLevels[key] = (int)attribute;
				}
			}

			minionStartingStats.Name = srcIdentity.GetComponent<UserNameable>().savedName;

			var dstIdentity = Util.KInstantiate<MinionIdentity>(Assets.GetPrefab(MinionConfig.ID));
			Immigration.Instance.ApplyDefaultPersonalPriorities(dstIdentity.gameObject);
			dstIdentity.gameObject.SetActive(true);
			minionStartingStats.Apply(dstIdentity.gameObject);
			dstIdentity.arrivalTime += srcIdentity.arrivalTime;

			dstIdentity.gameObject.AddComponent<AETE_DuplicatedDupe>().remainingLifeTimeSeconds = 30f;

			dstIdentity.transform.position = srcIdentity.transform.position;

			CopyBioInks(srcIdentity, dstIdentity);
		}

		private static void CopyBioInks(MinionIdentity srcIdentity, MinionIdentity dstIdentity)
		{
		}

		private static void CopyMinion(MinionIdentity sourceIdentity, MinionIdentity destinationIdentity)
		{
			Log.Debuglog("copying minion");

			destinationIdentity.SetName(sourceIdentity.name);
			destinationIdentity.nameStringKey = sourceIdentity.nameStringKey;
			destinationIdentity.personalityResourceId = sourceIdentity.personalityResourceId;
			destinationIdentity.gender = sourceIdentity.gender;
			destinationIdentity.genderStringKey = sourceIdentity.genderStringKey;
			destinationIdentity.arrivalTime = sourceIdentity.arrivalTime;
			destinationIdentity.voiceIdx = sourceIdentity.voiceIdx;

			CopyBodyData(sourceIdentity, destinationIdentity);
			CopyTraits(sourceIdentity, destinationIdentity);
			CopyAccessories(sourceIdentity, destinationIdentity);
			CopyConsumablePermissions(sourceIdentity, destinationIdentity);
			CopyResume(sourceIdentity, destinationIdentity);

			/*
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

		private static void CopyResume(MinionIdentity sourceIdentity, MinionIdentity destinationIdentity)
		{
			sourceIdentity.TryGetComponent(out MinionResume srcMinionResume);
			destinationIdentity.TryGetComponent(out MinionResume dstMinionResume);

			if (srcMinionResume == null || dstMinionResume == null)
				return;

			dstMinionResume.MasteryBySkillID = new(srcMinionResume.MasteryBySkillID);
			dstMinionResume.GrantedSkillIDs = srcMinionResume.MasteryBySkillID == null ? new() : new(srcMinionResume.GrantedSkillIDs);
			dstMinionResume.AptitudeBySkillGroup = new(srcMinionResume.AptitudeBySkillGroup);
			dstMinionResume.totalExperienceGained = srcMinionResume.totalExperienceGained;
			dstMinionResume.SetHats(srcMinionResume.currentHat, srcMinionResume.targetHat);
		}

		private static void CopyTraits(MinionIdentity sourceIdentity, MinionIdentity destinationIdentity)
		{
			sourceIdentity.TryGetComponent(out Traits srcTraits);
			destinationIdentity.TryGetComponent(out Traits dstTraits);

			if (srcTraits == null || dstTraits == null)
				return;

			dstTraits.SetTraitIds(new List<string>(srcTraits.TraitIds));
		}

		private static void CopyConsumablePermissions(MinionIdentity sourceIdentity, MinionIdentity destinationIdentity)
		{
			sourceIdentity.TryGetComponent(out ConsumableConsumer srcConsumer);
			destinationIdentity.TryGetComponent(out ConsumableConsumer dstConsumer);

			if (srcConsumer.forbiddenTagSet != null)
				dstConsumer.forbiddenTagSet = new HashSet<Tag>(srcConsumer.forbiddenTagSet);
		}

		private static void CopyAccessories(MinionIdentity sourceIdentity, MinionIdentity destinationIdentity)
		{
			sourceIdentity.TryGetComponent(out Accessorizer srcAccessorizer);
			destinationIdentity.TryGetComponent(out Accessorizer dstAccessorizer);

			if (srcAccessorizer == null || dstAccessorizer == null)
				return;

			foreach (var accessoryRef in srcAccessorizer.GetAccessories())
			{
				var accessory = accessoryRef.Get();
				if (accessory == null) continue;
				dstAccessorizer.AddAccessory(accessory);
			}

			dstAccessorizer.ApplyAccessories();
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
