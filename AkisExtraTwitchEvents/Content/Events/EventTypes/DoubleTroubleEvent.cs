using FUtility;
using Klei.AI;
using System.Collections.Generic;
using TUNING;
using Twitchery.Content.Scripts;

namespace Twitchery.Content.Events.EventTypes
{
	public class DoubleTroubleEvent : ITwitchEvent
	{
		public const string ID = "DoubleTrouble";
		public const string MAX_DUPES_KEY = "MaxDupeCount";

		public int GetWeight() => TwitchEvents.Weights.COMMON;

		public bool Condition(object data) => Mod.Settings.MaxDupes >= (int)(Components.LiveMinionIdentities.Count * 1.5f);

		public string GetID() => ID;

		public void Run(object data)
		{
			if (AkisTwitchEvents.Instance == null)
			{
				Log.Warning("AkisTwitchEvents.Instance is null.");
				return;
			}

			var dupeCount = Components.LiveMinionIdentities.Items.Count;
			foreach (var minion in Components.LiveMinionIdentities.Items)
			{
				CreateCopy(minion);

				if (++dupeCount >= Mod.Settings.MaxDupes)
					break;
			}

			ONITwitchLib.ToastManager.InstantiateToast(
				STRINGS.AETE_EVENTS.DOUBLE_TROUBLE.TITLE,
				 STRINGS.AETE_EVENTS.DOUBLE_TROUBLE.DESC);
		}

		private static void CreateCopy(MinionIdentity srcIdentity)
		{
			var personality = Db.Get().Personalities.TryGet(srcIdentity.personalityResourceId);

			if (personality == null)
			{
				Log.Debug("No personality");
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

			if (dstIdentity.gameObject.TryGetComponent(out AETE_MinionStorage storage))
				storage.MakeItDouble();

			CopyConsumablePermissions(srcIdentity, dstIdentity);

			Game.Instance.SpawnFX(SpawnFXHashes.BuildingFreeze, dstIdentity.transform.position, 0);
		}

		private static void CopyConsumablePermissions(MinionIdentity sourceIdentity, MinionIdentity destinationIdentity)
		{
			sourceIdentity.TryGetComponent(out ConsumableConsumer srcConsumer);
			destinationIdentity.TryGetComponent(out ConsumableConsumer dstConsumer);

			if (srcConsumer.forbiddenTagSet != null)
				dstConsumer.forbiddenTagSet = new HashSet<Tag>(srcConsumer.forbiddenTagSet);
		}
	}
}
