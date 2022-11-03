﻿using System;

namespace SnowSculptures.Content.Buildings
{
    public class GlassCase : KMonoBehaviour
    {
        [MyCmpReq]
        private Building building;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            UpdateSealables(s => s.Seal());
        }

        protected override void OnCleanUp()
        {
            UpdateSealables(s => s.Unseal());
            base.OnCleanUp();
        }

        private void UpdateSealables(Action<GlassCaseSealable> fn)
        {
            var pooledList = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
            GameScenePartitioner.Instance.GatherEntries(building.GetExtents(), GameScenePartitioner.Instance.completeBuildings, pooledList);

            foreach (var scenePartitionerEntry in pooledList)
            {
                if (scenePartitionerEntry.obj is KMonoBehaviour kMonoBehaviour && kMonoBehaviour.TryGetComponent(out GlassCaseSealable sealable))
                {
                    fn(sealable);
                }
            }
        }
    }
}
