using Klei.AI;
using ONITwitchLib;
using System.Linq;
using Twitchery.Content.Scripts;

namespace Twitchery.Content.Events.EventTypes
{
	public class SuperDupeEvent() : TwitchEventBase(ID)
	{
		public const string ID = "SuperDupe";
		private MinionIdentity currentTarget;
		private string currentTargetName;

		public override Danger GetDanger() => Danger.None;

		public override int GetWeight() => Consts.EventWeight.Uncommon;

		public override bool Condition()
		{
			if (AkisTwitchEvents.Instance.lastSuperDupeEventCycle + 10 > GameClock.Instance.GetTimeInCycles())
				return false;

			if (currentTarget == null || !IsDupeSuperable(currentTarget))
				OnDraw();

			return currentTarget != null;
		}

		public override void OnDraw()
		{
			base.OnDraw();
			GetIdentity(out var newIdentity, false);
			SetTarget(newIdentity);
		}

		public override void OnGameLoad()
		{
			base.OnGameLoad();
			OnDraw();

			AkisTwitchEvents.Instance.onDupeSuperEnded += () =>
			{
				if (currentTarget == null)
					OnDraw();
			};
		}

		private void SetTarget(MinionIdentity identity)
		{
			currentTarget = identity;

			// dont want color tag here because this is sent to the twitch chat
			currentTargetName = identity == null ? null : Util.StripTextFormatting(identity.GetProperName());

			SetName(STRINGS.AETE_EVENTS.SUPERDUPE.TOAST.Replace("{Name}", currentTargetName ?? "N/A"));
		}

		public override void Run()
		{
			if (Components.LiveMinionIdentities.Count == 0)
				return;

			var isOriginalTarget = true;
			var previousname = currentTargetName ?? "N/A";

			if (currentTarget == null)
				isOriginalTarget = GetIdentity(out currentTarget, true);

			if (currentTarget == null)
				return;

			currentTarget.GetComponent<Effects>().Add(TEffects.SUPERDUPE, true);

			if (!isOriginalTarget)
			{
				ToastManager.InstantiateToastWithGoTarget(
					STRINGS.AETE_EVENTS.SUPERDUPE.TOAST.Replace("{Name}", currentTargetName ?? "N/A"),
					STRINGS.AETE_EVENTS.SUPERDUPE.DESC_NOTFOUND
						.Replace("{Previous}", previousname)
						.Replace("{Name}", currentTarget.GetProperName()),
					currentTarget.gameObject);
			}
			else
				ToastManager.InstantiateToastWithGoTarget(
					STRINGS.AETE_EVENTS.SUPERDUPE.TOAST,
					STRINGS.AETE_EVENTS.SUPERDUPE.DESC.Replace("{Name}", currentTarget.GetProperName()),
					currentTarget.gameObject);

			SetTarget(null);

			AkisTwitchEvents.Instance.lastSuperDupeEventCycle = GameClock.Instance.GetTimeInCycles();
		}

		private bool IsDupeSuperable(MinionIdentity identity)
		{
			return !identity.IsNullOrDestroyed()
				&& !identity.HasTag(GameTags.Dead)
				&& !identity.GetComponent<Effects>().HasEffect(TEffects.SUPERDUPE);
		}

		private bool GetIdentity(out MinionIdentity identity, bool pushWarning)
		{
			if (currentTarget != null && !currentTarget.HasTag(GameTags.Dead))
			{
				identity = currentTarget;
				return true;
			}

			var targets = Components.LiveMinionIdentities.items
				.Where(IsDupeSuperable);

			if (targets.Count() == 0)
			{
				if (pushWarning)
					ToastManager.InstantiateToast("Warning", "No Superable duplicants around, cannot execute event.");

				identity = null;
				return false;
			}

			identity = targets.GetRandom();
			return false;
		}
	}
}
