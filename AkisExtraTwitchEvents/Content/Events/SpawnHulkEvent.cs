using ONITwitchLib;
using ONITwitchLib.Utils;
using System;
using System.Linq;
using TUNING;
using UnityEngine;

namespace Twitchery.Content.Events
{
	public class SpawnHulkEvent : ITwitchEvent
	{
		public const string ID = "Hulk";
		public Personality personality;

		public bool Condition(object data)
		{
			return !Components.LiveMinionIdentities.Any(minion => minion.personalityResourceId == TPersonalities.HULK);
		}

		public string GetID() => ID;

		public void Run(object data)
		{
			personality = Db.Get().Personalities.Get(TPersonalities.HULK);

			var pos = PosUtil.ClampedMouseCellWithRange(5);

			var go = SpawnHulk(Grid.CellToPos(pos));
			ToastManager.InstantiateToastWithGoTarget(STRINGS.AETE_EVENTS.HULK.TOAST, STRINGS.AETE_EVENTS.HULK.DESC, go);
		}

		public GameObject SpawnHulk(Vector3 position)
		{
			var minionStartingStats = new MinionStartingStats(personality, guaranteedTraitID: "AncientKnowledge");
			minionStartingStats.Traits.Add(Db.Get().traits.TryGet("Chatty"));
			minionStartingStats.voiceIdx = -2;

			minionStartingStats.StartingLevels["Strength"] += 20;

			var minionIdentity = Util.KInstantiate<MinionIdentity>(Assets.GetPrefab((Tag)MinionConfig.ID));
			Immigration.Instance.ApplyDefaultPersonalPriorities(minionIdentity.gameObject);
			minionIdentity.gameObject.SetActive(true);
			minionStartingStats.Apply(minionIdentity.gameObject);

			MinionResume component = minionIdentity.GetComponent<MinionResume>();

			for (int index = 0; index < 3; ++index)
				component.ForceAddSkillPoint();

			var spawnPos = position with
			{
				z = Grid.GetLayerZ(Grid.SceneLayer.Move)
			};

			minionIdentity.transform.SetPosition(spawnPos);

			return minionIdentity.gameObject;
		}
	}
}
