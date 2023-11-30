using ONITwitchLib;
using ONITwitchLib.Utils;
using System.Linq;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class SpawnHulkEvent : ITwitchEvent
	{
		public const string ID = "Hulk";
		public Personality personality;

		public int GetWeight() => TwitchEvents.Weights.COMMON;

		public bool Condition(object data)
		{
			return
				Components.LiveMinionIdentities.Count <= 35 &&
				!Components.LiveMinionIdentities.Any(minion => minion.personalityResourceId == TPersonalities.HULK);
		}

		public string GetID() => ID;

		public void Run(object data)
		{
			personality = Db.Get().Personalities.Get(TPersonalities.HULK);

			var telepad = GameUtil.GetActiveTelepad();

			var pos = telepad == null
				? Grid.CellToPos(PosUtil.RandomCellNearMouse())
				: telepad.transform.position + Vector3.up;

			var go = SpawnHulk(pos);
			ToastManager.InstantiateToastWithGoTarget(STRINGS.AETE_EVENTS.HULK.TOAST, STRINGS.AETE_EVENTS.HULK.DESC, go);
		}

		public GameObject SpawnHulk(Vector3 position)
		{
			var minionStartingStats = new MinionStartingStats(personality)
			{
				voiceIdx = -2
			};

			var minionIdentity = Util.KInstantiate<MinionIdentity>(Assets.GetPrefab((Tag)MinionConfig.ID));
			Immigration.Instance.ApplyDefaultPersonalPriorities(minionIdentity.gameObject);
			minionIdentity.gameObject.SetActive(true);
			minionStartingStats.Apply(minionIdentity.gameObject);

			var identity = minionIdentity.GetComponent<MinionResume>();

			for (int index = 0; index < 3; ++index)
				identity.ForceAddSkillPoint();

			var spawnPos = position with
			{
				z = Grid.GetLayerZ(Grid.SceneLayer.Move)
			};

			minionIdentity.transform.SetPosition(spawnPos);

			if (minionIdentity.TryGetComponent(out Health health))
			{
				health.amountInstance.SetValue(health.amountInstance.GetMax());
			}

			return minionIdentity.gameObject;
		}
	}
}
