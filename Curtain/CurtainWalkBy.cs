using System.Linq;

namespace Curtain
{
    public partial class Curtain
    {
        private HandleVector<int>.Handle pickupablesChangedEntry;
        private Extents pickupableExtents;
        private bool passedDirty;
        private bool passingLeft;

        private void StartPartitioner()
        {
            pickupableExtents = Extents.OneCell(building.PlacementCells[0]);

            pickupablesChangedEntry = GameScenePartitioner.Instance.Add(
               "Curtain.PickupablesChanged",
               gameObject,
               pickupableExtents,
               GameScenePartitioner.Instance.pickupablesChangedLayer, OnPickupablesChanged);

            passedDirty = true;
        }

        private void OnPickupablesChanged(object data)
        {
            Pickupable p = data as Pickupable;
            if (p && IsDupe(p))
                passedDirty = true;
        }

        internal void CheckDupePassing()
        {
            if (!passedDirty) return;
            var pooledList = GatherEntries();

            foreach (var entry in pooledList.Select(e => e.obj as Pickupable))
            {
                if (IsDupe(entry))
                {
                    UpdateMovement(entry.GetComponent<Navigator>());
                    return;
                }
            }

            passedDirty = false;
        }

        private void UpdateMovement(Navigator navigator)
        {
            if (navigator.IsMoving())
            {
                passingLeft = navigator.GetNextTransition().x > 0;
                Trigger((int)GameHashes.WalkBy, this);
            }

            passedDirty = false;
        }

        private ListPool<ScenePartitionerEntry, Curtain>.PooledList GatherEntries()
        {
            var pooledList = ListPool<ScenePartitionerEntry, Curtain>.Allocate();
            GameScenePartitioner.Instance.GatherEntries(pickupableExtents, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
            return pooledList;
        }

        private bool IsDupe(Pickupable pickupable) => pickupable.KPrefabID.HasTag(GameTags.DupeBrain);
    }
}