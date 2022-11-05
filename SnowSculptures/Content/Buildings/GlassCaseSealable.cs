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

        protected override void OnSpawn()
        {
            base.OnSpawn();

            var cell = this.NaturalBuildingCell();

            /*            foreach (var offset in building.Def.PlacementOffsets)
                        {
                            var rotatedCellOffset = Rotatable.GetRotatedCellOffset(offset, building.Orientation);
                            var c = Grid.OffsetCell(cell, rotatedCellOffset);
                            if (Grid.ObjectLayers[(int)ObjectLayer.AttachableBuilding].TryGetValue(c, out var go) && go.TryGetComponent(out GlassCase glassCase))
                            {
                                underGlassCase = true;
                                break;
                            }
                        }*/

            CheckForCase();

            /*            if (underGlassCase)
                        {
                            Seal();
                        }*/
        }

        private void CheckForCase()
        {
            var pooledList = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
            GameScenePartitioner.Instance.GatherEntries(building.GetExtents(), GameScenePartitioner.Instance.completeBuildings, pooledList);

            foreach (var scenePartitionerEntry in pooledList)
            {
                if (scenePartitionerEntry.obj is KMonoBehaviour kMonoBehaviour && kMonoBehaviour.TryGetComponent(out GlassCase glassCase))
                {
                    Seal();
                    break;
                }
            }
        }

        public void Seal()
        {
            var handle = GameComps.StructureTemperatures.GetHandle(gameObject);
            if (handle.IsValid() && GameComps.StructureTemperatures.IsEnabled(handle))
            {
                GameComps.StructureTemperatures.Disable(handle);
            }

            statusItem = kSelectable.AddStatusItem(sealedStatus);
            kbac.Offset += animOffset;
            kbac.SetDirty();
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
        }

        protected override void OnCleanUp()
        {
            Unseal();
            base.OnCleanUp();
        }
    }
}
