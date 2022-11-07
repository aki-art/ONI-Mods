using System;
using UnityEngine;

namespace SnowSculptures.Content.Buildings
{
    public class GlassCaseSealable : KMonoBehaviour
    {
        [MyCmpReq]
        private Building building;

        [MyCmpReq]
        private KSelectable kSelectable;

        [MyCmpReq]
        private KBatchedAnimController kbac;

        private Guid statusItem;

        public static Vector3 animOffset = new Vector3(0, 0.3f);

        public static StatusItem sealedStatus = new StatusItem("SnowSculptures_SealedStatusItem", "BUILDINGS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
        public static StatusItem somehowSealedStatus = new StatusItem("SnowSculptures_SomehowSealedStatusItem", "BUILDINGS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);

        public bool IsCased => glassCase != null;

        public GlassCase glassCase;

        protected override void OnSpawn()
        {
            base.OnSpawn();

            CheckForCase();
        }

        private void CheckForCase()
        {
            var pooledList = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
            GameScenePartitioner.Instance.GatherEntries(building.GetExtents(), GameScenePartitioner.Instance.completeBuildings, pooledList);

            foreach (var scenePartitionerEntry in pooledList)
            {
                if (scenePartitionerEntry.obj is KMonoBehaviour kMonoBehaviour && kMonoBehaviour.TryGetComponent(out GlassCase glassCase))
                {
                    Seal(glassCase);
                    break;
                }
            }
        }

        public void Seal(GlassCase glassCase)
        {
            var handle = GameComps.StructureTemperatures.GetHandle(gameObject);
            if (handle.IsValid() && GameComps.StructureTemperatures.IsEnabled(handle))
            {
                GameComps.StructureTemperatures.Disable(handle);
            }

            statusItem = kSelectable.AddStatusItem(sealedStatus);
            kbac.Offset += animOffset;
            kbac.SetDirty();

            if (gameObject.TryGetComponent(out SnowPile pile))
            {
                pile.PutInCase(glassCase);
            }

            this.glassCase = glassCase;
        }

        public void Unseal()
        {
            var handle = GameComps.StructureTemperatures.GetHandle(gameObject);
            if (handle.IsValid() && !GameComps.StructureTemperatures.IsEnabled(handle))
            {
                GameComps.StructureTemperatures.Enable(handle);
            }

            kSelectable.RemoveStatusItem(statusItem);
            kbac.Offset -= animOffset;

            if (gameObject.TryGetComponent(out SnowPile pile))
            {
                pile.TakeOutFromCase(glassCase);
            }

            glassCase = null;
        }

        protected override void OnCleanUp()
        {
            Unseal();
            base.OnCleanUp();
        }
    }
}
