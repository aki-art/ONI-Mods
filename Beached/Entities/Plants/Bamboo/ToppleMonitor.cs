using UnityEngine;

namespace Beached.Entities.Plants.Bamboo
{
    // Manages a plant being able to grow on solid ground, or on itself.
    public class ToppleMonitor : UprootedMonitor
    {
        [SerializeField]
        public Tag validFoundationTag;

        [SerializeField]
        public ObjectLayer objectLayer;

        public bool CanGrowOn(int cell)
        {
            if (Grid.Solid[cell])
            {
                return true;
            }

            if (Grid.ObjectLayers[(int)objectLayer].TryGetValue(cell, out var go))
            {
                return go.HasTag(validFoundationTag);
            }

            return false;
        }

        protected override void OnCleanUp()
        {
            CheckToppleableAbove();
            base.OnCleanUp();
        }

        private void CheckToppleableAbove()
        {
            var cell = Grid.CellAbove(Grid.PosToCell(this));


            while (CheckToppleable(cell))
            {
                cell = Grid.CellAbove(cell);
            }
        }

        private static bool CheckToppleable(int cellAbove)
        {
            if (Grid.ObjectLayers[(int)ObjectLayer.Building].TryGetValue(cellAbove, out var go))
            {
                if (go.TryGetComponent(out ToppleMonitor toppleMonitor))
                {
                    if (!toppleMonitor.IsUprooted)
                    {
                        toppleMonitor.OnGroundChanged(null);
                    }
                    return true;
                }
            }

            return false;
        }

        // redirected from UprootableMonitorPatch, return is for skipping the original method
        public bool IsSuitableFoundation(int cell, out bool __result, ToppleMonitor toppleMonitor)
        {
            var canGrowOn = true;

            foreach (var offset in toppleMonitor.monitorCells)
            {
                if (!Grid.IsCellOffsetValid(cell, offset))
                {
                    __result = false;
                    return false;
                }

                var offsetCell = Grid.OffsetCell(cell, offset);
                canGrowOn = toppleMonitor.CanGrowOn(offsetCell);

                if (!canGrowOn)
                {
                    break;
                }
            }

            __result = canGrowOn;
            return false;
        }
    }
}
