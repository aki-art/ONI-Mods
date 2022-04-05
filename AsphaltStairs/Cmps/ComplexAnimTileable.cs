using Stairs;
using UnityEngine;

namespace AsphaltStairs.Cmps
{
    // Note 2020: Similar to what Stairs uses, but convoluted and not reusable
    // Note 2022: I have no idea what this lovecraftian spaghetti is, but it works and I have no time or patience to refactor it for now
    [SkipSaveFileSerialization]
    public class ComplexAnimTileable : KMonoBehaviour
    {
        [SerializeField]
        public ObjectLayer objectLayer;

        [SerializeField]
        public Tag tag;

        [MyCmpReq]
        private OccupyArea occupyArea;

        [MyCmpReq]
        private Rotatable rotatable;

        private HandleVector<int>.Handle partitionerEntry;
        private Extents extents;

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

        protected override void OnSpawn()
        {
            extents = occupyArea.GetExtents();

            var watchExtents = new Extents(extents.x - 1, extents.y - 1, extents.width + 2, extents.height + 2);
            partitionerEntry = GameScenePartitioner.Instance.Add("ComplexAnimTileable.OnSpawn", gameObject, watchExtents, GameScenePartitioner.Instance.objectLayers[(int)objectLayer], OnNeighbourCellsUpdated);
            UpdateEndCaps();
        }

        protected override void OnCleanUp()
        {
            GameScenePartitioner.Instance.Free(ref partitionerEntry);
            base.OnCleanUp();
        }

        private void UpdateEndCaps()
        {
            var cell = Grid.PosToCell(this);

            var showtopPlatformRightSymbolAlt = true;
            var showtopPlatformRightCapSymbol = true;
            var showtopPlatformLeftCapSymbol = true;
            var showbottomCapSymbol = true;
            var showstairsBottomCapSymbol = true;
            var showplatformBridgeSymbol = false;
            var showbeamCapLeftSymbol = false;

            var showsingleBeamSymbol = true;
            var showsingleBeamBottomCapSymbol = true;

            var isStair = false;

            Grid.CellToXY(cell, out var x, out var y);

            var left_offset = new CellOffset(extents.x - x - 1, 0);
            var right_offset = new CellOffset(extents.x - x + extents.width, 0);
            var above_offset = new CellOffset(0, extents.y - y + extents.height);
            var below_offset = new CellOffset(0, extents.y - y - 1);

            var rotatable = GetComponent<Rotatable>();
            if (rotatable)
            {
                left_offset = rotatable.GetRotatedCellOffset(left_offset);
                right_offset = rotatable.GetRotatedCellOffset(right_offset);
                above_offset = rotatable.GetRotatedCellOffset(above_offset);
                below_offset = rotatable.GetRotatedCellOffset(below_offset);
            }

            var cell_left = Grid.OffsetCell(cell, left_offset);
            var cell_right = Grid.OffsetCell(cell, right_offset);
            var cell_above = Grid.OffsetCell(cell, above_offset);
            var cell_below = Grid.OffsetCell(cell, below_offset);

            var cell_below_left = Grid.OffsetCell(cell_below, left_offset);

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
                if (isStair)
                {
                    MyGrid.Masks[cell] |= MyGrid.Flags.Walkable;
                }
                else
                {
                    MyGrid.Masks[cell] &= (MyGrid.Flags)251;
                }
            }

            var controllers = GetComponentsInChildren<KBatchedAnimController>();

            foreach (var controller in controllers)
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

        private bool HasTileableNeighbour(int neighborCell)
        {
            var go = Grid.Objects[neighborCell, (int)objectLayer];
            return go != null && go.HasTag(tag);
        }

        private void OnNeighbourCellsUpdated(object data)
        {
            if (this != null && gameObject != null && partitionerEntry.IsValid())
            {
                UpdateEndCaps();
            }
        }

        private static bool IsWall(int cell)
        {
            return (Grid.BuildMasks[cell] & (Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation)) != 0 || Grid.HasDoor[cell];
        }

        private bool IsFlipped(int cell)
        {
            var otherItem = Grid.Objects[cell, (int)objectLayer];

            if (otherItem != null && otherItem.TryGetComponent(out Rotatable otherRotatable))
            {
                return rotatable.GetVisualizerFlipX() != otherRotatable.GetVisualizerFlipX();
            }

            return false;
        }
    }
}
