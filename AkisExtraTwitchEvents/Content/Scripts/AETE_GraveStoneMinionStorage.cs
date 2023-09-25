using FUtility;
using KSerialization;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class AETE_GraveStoneMinionStorage : KMonoBehaviour
	{
		[MyCmpGet] private MinionStorage minionStorage;
		[MyCmpGet] private Grave grave;
		[Serialize] public string minionName;

		public bool HasDupe() => minionStorage.serializedMinions != null && minionStorage.serializedMinions.Count > 0;

		public override void OnSpawn()
		{
			base.OnSpawn();
			Mod.graves.Add(this);
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			Mod.graves.Remove(this);
		}

		public string GetName() => minionName;

		public void OnDupeBuried(GameObject corpse)
		{
			Log.Debug("On DUpe buried");
			if(HasDupe())
				return;


			Log.Debug("storing dupe");

			minionName = corpse.name;
			minionStorage.SerializeMinion(corpse);

			Log.Debug(minionStorage.serializedMinions.Count);
		}

		public GameObject Revive()
		{
			if (!HasDupe())
			{
				Log.Debug("reviving null dupe");
				return null;
			}

			minionName = null;

			grave.smi.GoTo(grave.smi.sm.empty);

			return minionStorage.DeserializeMinion(minionStorage.serializedMinions[0].id, transform.position);
		}
	}
}
