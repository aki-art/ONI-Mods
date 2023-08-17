using ONITwitchLib;
using System;
using Twitchery.Content.Defs;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events
{
	// this event is not added to the rotation by default
	// instead, it gets added as variations
	public class PolymorphEvent : ITwitchEvent
	{
		public const string ID = "Polymorph";
		public const float REROLL_COOLDOWN = 5f;
		public float lastReroll = 0;

		public bool Condition(object _) => Components.LiveMinionIdentities.Count > 0; // check if not all are turned yet

		public int GetWeight() => TwitchEvents.Weights.COMMON;

		public string GetID() => ID;

		public void Run(object data)
		{
			var isOriginalTarget = GetIdentity(out var identity);
			var creaturePrefabId = PolymorphFloorCritterConfig.ID;

			var critter = FUtility.Utils.Spawn(creaturePrefabId, identity.transform.position);
			var morph = TDb.polymorphs.GetRandom();

			var toast = STRINGS.AETE_EVENTS.POLYMOPRH.DESC
				.Replace("{Dupe}", identity.GetProperName())
				.Replace("{Critter}", morph.Name);

			if (!isOriginalTarget)
			{
				toast = STRINGS.AETE_EVENTS.POLYMOPRH.DESC_NOTFOUND
					.Replace("{TargetDupe}", AkisTwitchEvents.polyTargetName)
					.Replace("{Dupe}", identity.GetProperName())
					+ toast;
			}

			critter.GetComponent<AETE_PolymorphCritter>().SetMorph(identity, morph);

			ToastManager.InstantiateToastWithGoTarget(STRINGS.AETE_EVENTS.POLYMOPRH.TOAST_ALT, toast, critter.gameObject);
			AudioUtil.PlaySound(ModAssets.Sounds.POLYMORHPH, ModAssets.GetSFXVolume() * 0.7f);
			Game.Instance.SpawnFX(ModAssets.Fx.pinkPoof, critter.transform.position, 0);
		}

		private bool GetIdentity(out MinionIdentity identity)
		{
			if (AkisTwitchEvents.polymorphTarget != null && !AkisTwitchEvents.polymorphTarget.HasTag(GameTags.Dead))
			{
				identity = AkisTwitchEvents.polymorphTarget;
				return true;
			}

			var minions = Components.LiveMinionIdentities.GetWorldItems(ClusterManager.Instance.activeWorldId);

			if (minions.Count == 0)
				minions = Components.LiveMinionIdentities.Items;

			if (minions.Count == 0)
			{
				ToastManager.InstantiateToast("Warning", "No duplicants alive, cannot execute event.");
				identity = null;
				return false;
			}

			identity = minions.GetRandom();
			return false;
		}
	}
}
