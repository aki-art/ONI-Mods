using FUtility;
using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class AETE_GraveStoneMinionStorage : KMonoBehaviour
	{
		public static HashSet<Guid> storedMinionGUIDs = new();

		[MyCmpGet] private MinionStorage minionStorage;
		[MyCmpGet] private Grave grave;

		[Serialize] public string minionName;

		public bool HasDupe() => minionStorage.serializedMinions != null && minionStorage.serializedMinions.Count > 0;

		public override void OnSpawn()
		{
			base.OnSpawn();
			Mod.graves.Add(this);

			if (ClusterManager.Instance.GetWorld(this.GetMyWorldId()) == null)
			{
				Util.KDestroyGameObject(this.gameObject);
				Log.Warning($"Corpse world {minionName} is missing.");
				return;
			}

			AddStoredMinions();
		}

		private void AddStoredMinions()
		{
			foreach (var storedMinion in minionStorage.serializedMinions)
				storedMinionGUIDs.Add(storedMinion.id);
		}

		private void RemoveStoredMinions()
		{
			foreach (var storedMinion in minionStorage.serializedMinions)
				storedMinionGUIDs.Remove(storedMinion.id);
		}


		public override void OnCleanUp()
		{
			base.OnCleanUp();
			Mod.graves.Remove(this);
		}

		public string GetName() => minionName;

		public void OnDupeBuried(GameObject corpse)
		{
			if (HasDupe())
				return;

			minionName = corpse.name;
			minionStorage.SerializeMinion(corpse);

			AddStoredMinions();
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


			RemoveStoredMinions();

			var revived = minionStorage.DeserializeMinion(minionStorage.serializedMinions[0].id, transform.position);


			return revived;
		}
	}
}
