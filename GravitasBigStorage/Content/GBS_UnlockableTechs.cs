using KSerialization;
using UnityEngine;

namespace GravitasBigStorage.Content
{
	public class GBS_UnlockableTechs : KMonoBehaviour
	{
		[SerializeField] public string unlockTechId;
		[Serialize] public bool unlockComplete;

		public override void OnSpawn()
		{
			base.OnSpawn();
			if (!unlockComplete && StoryManager.Instance.IsStoryComplete(Db.Get().Stories.LonelyMinion))
				Unlock();
		}

		private void Unlock()
		{
			var techItem = Db.Get().TechItems.TryGet(unlockTechId);
			if (techItem != null && !techItem.IsComplete())
				techItem.POIUnlocked();

			unlockComplete = true;
		}

		public void OnStoryComplete() => Unlock();
	}
}
