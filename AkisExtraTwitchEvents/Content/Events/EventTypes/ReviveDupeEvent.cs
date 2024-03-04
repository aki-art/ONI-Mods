using ONITwitchLib;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class ReviveDupeEvent() : TwitchEventBase(ID)
	{
		public const string ID = "ReviveDupe";

		public TargetIdentity target;

		public override bool Condition() => target.IsValid();

		public override int GetWeight() => TwitchEvents.Weights.VERY_RARE;

		public override Danger GetDanger() => Danger.Small;

		public override void OnDraw() => UpdateRevivalTarget();

		public override void Run()
		{
			GameObject revived = target.Revive();

			if (revived == null)
				ToastManager.InstantiateToast("Oops", "Something went wrong, Revive event cannot run.");
			else
			{
				new EmoteChore(
					revived.GetComponent<ChoreProvider>(),
					Db.Get().ChoreTypes.EmoteHighPriority,
					Db.Get().Emotes.Minion.Cheer);

				ToastManager.InstantiateToastWithGoTarget("", string.Format("{0} is back for more!", Util.StripTextFormatting(revived.GetProperName())), revived.gameObject);
			}

			UpdateRevivalTarget();
		}

		public void UpdateRevivalTarget()
		{
			var targets = ListPool<TargetIdentity, ReviveDupeEvent>.Allocate();

			foreach (var minion in Components.MinionIdentities.Items)
			{
				if (minion.HasTag(GameTags.Dead))
				{
					targets.Add(new TargetIdentity()
					{
						identity = minion
					});
				}
			}

			foreach (var grave in Mod.graves.Items)
			{
				if (grave.HasDupe())
				{
					targets.Add(new TargetIdentity()
					{
						storage = grave
					});
				}
			}

			if (targets.Count > 0)
			{
				var target = targets.GetRandom();

				var minionName = target.identity == null
					? target.storage.GetName()
					: target.identity.GetProperName();

				this.target = target;

				SetName(string.Format("Revive {0}", minionName));
			}
			else
			{
				target = default;
				SetName("Revive (not available)");
			}

			targets.Recycle();
		}

		public void OnDupeBuried(AETE_GraveStoneMinionStorage graveStorage, GameObject minion)
		{
			if (!target.IsValid())
				return;

			// TODO: proxy
			if (minion.TryGetComponent(out MinionIdentity identity))
			{
				if (target.identity == identity)
				{
					target.identity = null;
					target.storage = graveStorage;
				}
			}
		}

		public struct TargetIdentity
		{
			public AETE_GraveStoneMinionStorage storage;
			public MinionIdentity identity;

			public readonly GameObject Revive()
			{
				if (storage != null)
					return storage.Eject();

				if (identity != null)
				{
					var go = new GameObject("temporary carrier");
					var storage = go.AddComponent<MinionStorage>();
					var pos = identity.transform.position;
					storage.SerializeMinion(identity.gameObject);
					var result = storage.DeserializeMinion(storage.GetStoredMinionInfo()[0].id, pos);

					Object.Destroy(go);

					return result;
				}

				return null;
			}

			public readonly bool IsValid() => storage != null || identity != null;
		}
	}
}
