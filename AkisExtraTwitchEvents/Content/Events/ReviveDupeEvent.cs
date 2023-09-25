using FUtility;
using ONITwitchLib;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events
{
	internal class ReviveDupeEvent : ITwitchEvent
	{
		public const string ID = "ReviveDupe";

		public bool Condition(object data) => AkisTwitchEvents.HasRevivableDupeTarget();

		public string GetID() => ID;

		public int GetWeight() => TwitchEvents.Weights.UNCOMMON;

		public void Run(object data)
		{
			var target = AkisTwitchEvents.revivalEvent.target;
			GameObject revived = null;

			if (target.storage != null)
			{
				Log.Debug("had stored dupe");
				revived = target.storage.Revive();
			}
			else if (target.identity != null)
			{
				var go = new GameObject("temporary carrier");
				var storage = go.AddComponent<MinionStorage>();
				var pos = target.identity.transform.position;
				storage.SerializeMinion(target.identity.gameObject);
				revived = storage.DeserializeMinion(storage.GetStoredMinionInfo()[0].id, pos);

				Object.Destroy(go);
			}

			if (revived == null)
				ToastManager.InstantiateToast("Oops", "Something went wrong, Revive event cannot run.");
			else
			{
				new EmoteChore(
					revived.GetComponent<ChoreProvider>(), 
					Db.Get().ChoreTypes.EmoteHighPriority, 
					Db.Get().Emotes.Minion.Cheer);

				ToastManager.InstantiateToastWithGoTarget("", string.Format("{0} is back for more!", revived.GetProperName()), revived.gameObject);
			}

			AkisTwitchEvents.Instance.UpdateRevivalTarget();
		}
	}
}
