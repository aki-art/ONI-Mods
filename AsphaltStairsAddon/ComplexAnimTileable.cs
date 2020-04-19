using Stairs;
using System;
using UnityEngine;

namespace AsphaltStairsAddon
{
    // Similar to what Stairs uses, but convoluted and not reusable
    [SkipSaveFileSerialization]
    public class ComplexAnimTileable : KMonoBehaviour
    {
        private HandleVector<int>.Handle partitionerEntry;
        public ObjectLayer objectLayer = ObjectLayer.LadderTile;
        public Tag[] tags;
        private Extents extents;

        #region anim names
        private static readonly KAnimHashedString topPlatformRightSymbolAlt = new KAnimHashedString("top_platform_right_alt");
        private static readonly KAnimHashedString topPlatformRightCapSymbol = new KAnimHashedString("top_platform_right_cap");
        private static readonly KAnimHashedString topPlatformLeftCapSymbol = new KAnimHashedString("top_platform_left_cap");
        private static readonly KAnimHashedString bottomCapSymbol = new KAnimHashedString("bottom_cap");

        private static readonly KAnimHashedString stairSymbol = new KAnimHashedString("stairs");
        private static readonly KAnimHashedString underStairSymbol = new KAnimHashedString("under_stairs");
        private static readonly KAnimHashedString underStairExtensionSymbol = new KAnimHashedString("stairs_beam_extension");
        private static readonly KAnimHashedString stairsBottomCapSymbol = new KAnimHashedString("stairs_bottom_cap");
        private static readonly KAnimHashedString stairsExtensionBottomCapSymbol = new KAnimHashedString("stairs_extension_bottom_cap");
        private static readonly KAnimHashedString platformBridgeSymbol = new KAnimHashedString("top_platform_stair_mask");
        private static readonly KAnimHashedString beamCapLeftSymbol = new KAnimHashedString("cap_left");

        private static readonly KAnimHashedString longerLeftCapCapSymbol = new KAnimHashedString("long_left_cap");
        private static readonly KAnimHashedString singleBeamSymbol = new KAnimHashedString("beam");
        private static readonly KAnimHashedString singleBeamBottomCapSymbol = new KAnimHashedString("beam_bottom_cap");
        #endregion

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            if (tags == null || tags.Length == 0)
            {
                tags = new Tag[]
                {
                    GetComponent<KPrefabID>().PrefabTag
                };
            }
        }

        protected override void OnSpawn()
        {
            OccupyArea occupy_area = GetComponent<OccupyArea>();
            if (occupy_area != null)
            {
                extents = occupy_area.GetExtents();
            }
            else
            {
                Building building = GetComponent<Building>();
                extents = building.GetExtents();
            }
            Extents watch_extents = new Extents(extents.x - 1, extents.y - 1, extents.width + 2, extents.height + 2);
            partitionerEntry = GameScenePartitioner.Instance.Add("ComplexAnimTileable.OnSpawn", gameObject, watch_extents, GameScenePartitioner.Instance.objectLayers[(int)objectLayer], new Action<object>(OnNeighbourCellsUpdated));
            UpdateEndCaps();
        }

        protected override void OnCleanUp()
        {
            GameScenePartitioner.Instance.Free(ref partitionerEntry);
            base.OnCleanUp();
        }

        private void UpdateEndCaps()
        {
            int cell = Grid.PosToCell(this);

            bool showtopPlatformRightSymbolAlt = true;
            bool showtopPlatformRightCapSymbol = true;
            bool showtopPlatformLeftCapSymbol = true;
            bool showbottomCapSymbol = true;
            bool showstairsBottomCapSymbol = true;
            bool showplatformBridgeSymbol = false;
            bool showbeamCapLeftSymbol = false;

            bool showsingleBeamSymbol = true;
            bool showsingleBeamBottomCapSymbol = true;

            bool isStair = false;

            Grid.CellToXY(cell, out int x, out int y);

            CellOffset left_offset = new CellOffset(extents.x - x - 1, 0);
            CellOffset right_offset = new CellOffset(extents.x - x + extents.width, 0);
            CellOffset above_offset = new CellOffset(0, extents.y - y + extents.height);
            CellOffset below_offset = new CellOffset(0, extents.y - y - 1);

            Rotatable rotatable = GetComponent<Rotatable>();
            if (rotatable)
            {
                left_offset = rotatable.GetRotatedCellOffset(left_offset);
                right_offset = rotatable.GetRotatedCellOffset(right_offset);
                above_offset = rotatable.GetRotatedCellOffset(above_offset);
                below_offset = rotatable.GetRotatedCellOffset(below_offset);
            }

            int cell_left = Grid.OffsetCell(cell, left_offset);
            int cell_right = Grid.OffsetCell(cell, right_offset);
            int cell_above = Grid.OffsetCell(cell, above_offset);
            int cell_below = Grid.OffsetCell(cell, below_offset);

            int cell_below_left = Grid.OffsetCell(cell_below, left_offset);

            // stairs
            if (Grid.IsValidCell(cell_left) && Grid.IsValidCell(cell_above))
            {
                isStair = !HasTileableNeighbour(cell_left) && !HasTileableNeighbour(cell_above) && !IsWall(cell_left);
                showtopPlatformLeftCapSymbol = !isStair && !HasTileableNeighbour(cell_left);
                showsingleBeamSymbol = !isStair && !HasTileableNeighbour(cell_left);

                if (IsFlipped(cell_left) && !isStair)
                {
                    showbeamCapLeftSymbol = true;
                    showsingleBeamSymbol = true;
                    showplatformBridgeSymbol = true;
                }

                // above
                showtopPlatformRightSymbolAlt = !IsFlipped(cell_above) && HasTileableNeighbour(cell_above);
            }

            if (Grid.IsValidCell(cell_right))
            {
                showtopPlatformRightCapSymbol = !HasTileableNeighbour(cell_right);
            }

            if (Grid.IsValidCell(cell_below))
            {
                showbottomCapSymbol = !HasTileableNeighbour(cell_below);
                showstairsBottomCapSymbol = isStair && (!HasTileableNeighbour(cell_below) || !HasTileableNeighbour(cell_below_left));
                showsingleBeamBottomCapSymbol = showsingleBeamSymbol && showbottomCapSymbol;
            }


            if (MyGrid.IsStair(cell))
            {
                if (isStair) { MyGrid.Masks[cell] |= MyGrid.Flags.Walkable; }
                else { MyGrid.Masks[cell] &= (MyGrid.Flags)251; }
            }

            KBatchedAnimController[] controllers = GetComponentsInChildren<KBatchedAnimController>();

            foreach (KBatchedAnimController controller in controllers)
            {
                controller.SetSymbolVisiblity(topPlatformRightSymbolAlt, showtopPlatformRightSymbolAlt);
                controller.SetSymbolVisiblity(topPlatformRightCapSymbol, showtopPlatformRightCapSymbol);
                controller.SetSymbolVisiblity(topPlatformLeftCapSymbol, showtopPlatformLeftCapSymbol);

                controller.SetSymbolVisiblity(beamCapLeftSymbol, showbeamCapLeftSymbol);
                controller.SetSymbolVisiblity(bottomCapSymbol, showbottomCapSymbol);

                controller.SetSymbolVisiblity(stairsBottomCapSymbol, showstairsBottomCapSymbol);
                controller.SetSymbolVisiblity(stairsExtensionBottomCapSymbol, showstairsBottomCapSymbol);
                controller.SetSymbolVisiblity(platformBridgeSymbol, showplatformBridgeSymbol);
                controller.SetSymbolVisiblity(stairSymbol, isStair);
                controller.SetSymbolVisiblity(underStairSymbol, isStair);
                controller.SetSymbolVisiblity(underStairExtensionSymbol, isStair);

                controller.SetSymbolVisiblity(singleBeamBottomCapSymbol, showsingleBeamBottomCapSymbol);
                controller.SetSymbolVisiblity(singleBeamSymbol, showsingleBeamSymbol);

                controller.SetSymbolVisiblity(longerLeftCapCapSymbol, showsingleBeamSymbol);
            }
        }

        private bool HasTileableNeighbour(int neighbour_cell)
        {
            bool has_tileable_neighbour = false;
            GameObject go = Grid.Objects[neighbour_cell, (int)objectLayer];
            if (go != null)
            {
                KPrefabID kpid = go.GetComponent<KPrefabID>();
                if (kpid != null && kpid.HasAnyTags(tags))
                {
                    has_tileable_neighbour = true;
                }
            }
            return has_tileable_neighbour;
        }

        private void OnNeighbourCellsUpdated(object data)
        {
            if (!(this == null || gameObject == null))
            {
                if (partitionerEntry.IsValid())
                {
                    UpdateEndCaps();
                }
            }
        }
        private static bool IsWall(int cell)
        {
            return (Grid.BuildMasks[cell] & (Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation)) != 0 || Grid.HasDoor[cell];
        }

        private bool IsFlipped(int cell)
        {
            Rotatable rotatable = GetComponent<Rotatable>();
            if (!rotatable) return false;

            var otherItem = Grid.Objects[cell, (int)objectLayer];
            if (otherItem == null)
                return false;

            var otherRotatable = otherItem.GetComponent<Rotatable>();
            if (otherRotatable == null)
                return false;

            if (rotatable.GetVisualizerFlipX() != otherRotatable.GetVisualizerFlipX())
                return true;

            return false;
        }
    }
}
