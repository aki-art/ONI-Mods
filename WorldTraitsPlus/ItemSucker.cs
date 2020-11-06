using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WorldTraitsPlus
{
    public class ItemSucker : KMonoBehaviour, ISim33ms
    {
        public int range = 5;
        public float power = 2f;

        List<GameObject> items = new List<GameObject>();
        private Extents pickupableExtents;

        protected override void OnSpawn()
        {
            pickupableExtents = new Extents(Grid.PosToCell(this), range);
        }

        public void Sim33ms(float dt)
        {
            CollectItems();
            UpdateFallers();
        }

        private void UpdateFallers()
        {
            foreach(GameObject go in items)
            {
                if(go == null || go.transform == null) return;
                Vector3 vec =  transform.position - go.transform.position;
                vec *= power;

                if (GameComps.Fallers.Has(go))
                    GameComps.Fallers.Remove(go);

                GameComps.Fallers.Add(go, vec);
            }
        }

        private void CollectItems()
        {
            items.Clear();
            foreach (ScenePartitionerEntry entry in GatherEntries())
            {
                items.Add((entry.obj as Pickupable).gameObject);
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
