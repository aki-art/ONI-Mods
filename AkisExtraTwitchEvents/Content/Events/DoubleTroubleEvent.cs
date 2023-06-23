using FUtility;
using Klei.AI;
using System;
using System.Collections.Generic;
using TUNING;
using Twitchery.Content.Scripts;
using UnityEngine;
using static ResearchTypes;

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

			foreach (var minion in Components.LiveMinionIdentities.Items)
				CreateCopy(minion);

			ONITwitchLib.ToastManager.InstantiateToast(
				STRINGS.AETE_EVENTS.DOUBLE_TROUBLE.TITLE,
				 STRINGS.AETE_EVENTS.DOUBLE_TROUBLE.DESC);
		}

		private static void CreateCopy(MinionIdentity srcIdentity)
		{
			var personality = Db.Get().Personalities.TryGet(srcIdentity.personalityResourceId);

			if (personality == null)
			{
				Log.Debuglog("No personality");
				return;
			}

			var minionStartingStats = new MinionStartingStats(personality);

			if (srcIdentity.TryGetComponent(out Traits traits))
				minionStartingStats.Traits = new List<Trait>(traits.TraitList);

			minionStartingStats.voiceIdx = srcIdentity.voiceIdx;
			minionStartingStats.StartingLevels = new();

			if (srcIdentity.TryGetComponent(out Attributes attributes))
			{
				foreach (string key in DUPLICANTSTATS.ALL_ATTRIBUTES)
				{
					var attribute = attributes.GetValue(key);
					minionStartingStats.StartingLevels[key] = (int)attribute;
				}
			}

			minionStartingStats.Name = srcIdentity.GetProperName();

			var dstIdentity = Util.KInstantiate<MinionIdentity>(Assets.GetPrefab(MinionConfig.ID));
			Immigration.Instance.ApplyDefaultPersonalPriorities(dstIdentity.gameObject);
			dstIdentity.gameObject.SetActive(true);


			minionStartingStats.Apply(dstIdentity.gameObject);
			dstIdentity.arrivalTime += srcIdentity.arrivalTime;
			dstIdentity.transform.position = srcIdentity.transform.position;
			dstIdentity.nameStringKey = srcIdentity.nameStringKey;
			dstIdentity.personalityResourceId = srcIdentity.personalityResourceId;

			CopyBioInks(srcIdentity, dstIdentity);

			if (dstIdentity.gameObject.TryGetComponent(out AETE_MinionStorage storage))
			{
				storage.MakeItDouble();
			}

			CopyConsumablePermissions(srcIdentity, dstIdentity);
			//CopyAccessories(srcIdentity, dstIdentity);
			//CopyResume(srcIdentity, dstIdentity);
			//CopySchedule(srcIdentity, dstIdentity);

			Game.Instance.SpawnFX(SpawnFXHashes.BuildingFreeze, dstIdentity.transform.position, 0);
		}

		private static void CopyBioInks(MinionIdentity srcIdentity, MinionIdentity dstIdentity)
		{
			var type = Type.GetType("PrintingPodRecharge.ModAPI, PrintingPodRecharge");

			if (type == null)
				return;

			type.GetMethod("CopyFromMinion")?.Invoke(null, new object[] { srcIdentity, dstIdentity });
		}

		private static void CopySchedule(MinionIdentity sourceIdentity, MinionIdentity destIdentity)
		{
			var srcSchedulable = sourceIdentity.GetComponent<Schedulable>();
			var schedule = srcSchedulable.GetSchedule();

			if (schedule == null)
				return;

			var dstSchedulable = destIdentity.GetComponent<Schedulable>();
			schedule.Assign(dstSchedulable);
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
	}
}
