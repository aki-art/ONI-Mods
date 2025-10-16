using ONITwitchLib;
using System.Collections.Generic;
using System.Linq;
using Twitchery.Content.Defs;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	// this event is not added to the rotation by default
	// instead, it gets added as variations
	public class PolymorphEvent() : TwitchEventBase(ID)
	{
		public const string ID = "Polymorph";
		public const float REROLL_COOLDOWN = 5f;
		public float lastReroll = 0;
		public static readonly Tag[] forbiddenTags = [
			GameTags.Dead,
			TTags.midased,
			TTags.midasSafe,
			GameTags.Stored ];

		private MinionIdentity currentTarget;
		private string currentTargetName;

		public override void OnGameLoad()
		{
			base.OnGameLoad();
			OnDraw();
		}

		private void SetTarget(MinionIdentity identity)
		{
			currentTarget = identity;
			currentTargetName = identity == null ? null : Util.StripTextFormatting(identity.GetProperName());

			SetName(STRINGS.AETE_EVENTS.POLYMOPRH.TOAST.Replace("{Name}", currentTargetName ?? "N/A"));
		}

		public override bool Condition() => Components.LiveMinionIdentities.Any(IsDupePolyable);

		public override int GetWeight() => Consts.EventWeight.Common;

		public override string GetName() => STRINGS.AETE_EVENTS.POLYMOPRH.TOAST;

		public override Danger GetDanger() => Danger.Small;

		public string GetID() => ID;

		public override void Run()
		{
			var isOriginalTarget = true;

			if (currentTarget == null)
			{
				isOriginalTarget = GetIdentity(out currentTarget);
			}

			Polymorph(isOriginalTarget, currentTarget, currentTargetName, out var critter, out var toast);

			ToastManager.InstantiateToastWithGoTarget(STRINGS.AETE_EVENTS.POLYMOPRH.TOAST_ALT, toast, critter.gameObject);
			AudioUtil.PlaySound(ModAssets.Sounds.POLYMORHPH, ModAssets.GetSFXVolume() * 0.7f);
			Game.Instance.SpawnFX(ModAssets.Fx.pinkPoof, critter.transform.position, 0);

			SetTarget(null);
		}

		public static void Polymorph(bool isOriginalTarget, MinionIdentity identity, string minionName, out GameObject critter, out string toast)
		{
			var creaturePrefabId = PolymorphFloorCritterConfig.ID;

			critter = FUtility.Utils.Spawn(creaturePrefabId, identity.transform.position);
			var morph = TDb.polymorphs.GetRandom();

			toast = STRINGS.AETE_EVENTS.POLYMOPRH.DESC
				.Replace("{Dupe}", identity.GetProperName())
				.Replace("{Critter}", morph.Name);

			if (!isOriginalTarget)
			{
				toast = STRINGS.AETE_EVENTS.POLYMOPRH.DESC_NOTFOUND
					.Replace("{TargetDupe}", minionName ?? "N/A")
					.Replace("{Dupe}", identity.GetProperName())
					+ toast;
			}

			critter.GetComponent<AETE_PolymorphCritter>().SetMorph(identity, morph);
		}

		public override void OnDraw()
		{
			base.OnDraw();
			GetIdentity(out var newIdentity);
			SetTarget(newIdentity);
		}

		private bool GetIdentity(out MinionIdentity identity)
		{
			if (currentTarget != null && !currentTarget.HasTag(GameTags.Dead))
			{
				identity = currentTarget;
				return true;
			}

			var minions = GetTargetableMinions();

			if (minions.Count == 0)
			{
				ToastManager.InstantiateToast("Warning", "No duplicants alive, cannot execute event.");
				identity = null;
				return false;
			}

			identity = minions.GetRandom();
			return false;
		}

		private static List<MinionIdentity> GetTargetableMinions()
		{
			// prefer minions from the active world
			var minions = Components.LiveMinionIdentities.GetWorldItems(ClusterManager.Instance.activeWorldId);

			// if none pick any anywhere
			if (minions.Count == 0)
				minions = Components.LiveMinionIdentities.Items;

			// filter bionic or stored dupes
			minions.RemoveAll(m => !IsDupePolyable(m));

			return minions;
		}

		private static bool IsDupePolyable(MinionIdentity identity)
		{
			return identity.model == GameTags.Minions.Models.Standard && !identity.HasAnyTags(forbiddenTags);
		}
	}
}
