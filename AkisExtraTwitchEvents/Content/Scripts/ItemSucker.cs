using System.Collections.Generic;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class ItemSucker : KMonoBehaviour, ISim33ms
	{
		public int range = 5;
		public float power = 4f;

		private readonly List<KBatchedAnimController> items = [];
		private Extents pickupableExtents;

		public bool suck;

		public void SetRange(int range)
		{
			pickupableExtents = new Extents(Grid.PosToCell(this), range);
		}

		public void Sim33ms(float dt)
		{
			if (suck)
			{
				CollectItems();
				UpdateFallers();
			}
		}

		private void UpdateFallers()
		{
			foreach (var kbac in items)
			{
				if (kbac == null || kbac.transform == null || kbac.IsNullOrDestroyed())
					return;

				kbac.Rotation += 10f;
				kbac.animScale *= 0.95f;

				if (kbac.animScale < 0.0005f)
				{
					Util.KDestroyGameObject(kbac.gameObject);
				}
				else
				{
					Vector3 vec = transform.position - kbac.transform.position;
					vec *= power;

					var go = kbac.gameObject;

					if (GameComps.Fallers.Has(go))
						GameComps.Fallers.Remove(go);

					GameComps.Fallers.Add(go, vec);
				}
			}
		}

		private void CollectItems()
		{
			items.Clear();

			foreach (ScenePartitionerEntry entry in GatherEntries())
			{
				var pickupable = (entry.obj as Pickupable);

				if (!pickupable.handleFallerComponents || pickupable.HasTag(GameTags.Stored))
					continue;

				if (pickupable.TryGetComponent(out MinionIdentity _))
					continue;

				// these are usually better left alone
				if (pickupable.HasTag(ONITwitchLib.ExtraTags.OniTwitchSurpriseBoxForceDisabled))
					continue;

				var go = pickupable.gameObject;

				items.Add(go.GetComponent<KBatchedAnimController>());
			}
		}

		private ListPool<ScenePartitionerEntry, ItemSucker>.PooledList GatherEntries()
		{
			var pooledList = ListPool<ScenePartitionerEntry, ItemSucker>.Allocate();
			GameScenePartitioner.Instance.GatherEntries(pickupableExtents, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
			return pooledList;
		}

	}
}
