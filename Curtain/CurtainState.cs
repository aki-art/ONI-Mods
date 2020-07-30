using Harmony;

namespace Curtain
{
    public partial class Curtain
    {
        private bool meltCheck;

        private void UpdateState()
        {
            switch (CurrentState)
            {
                case ControlState.Open:
                    Open();
                    break;
                case ControlState.Locked:
                    Lock();
                    break;
                case ControlState.Auto:
                default:
                    Close();
                    break;
            }

            UpdatePassable();
        }

        private void SetDefaultCellFlags()
        {
            foreach (int cell in building.PlacementCells)
            {
                Grid.FakeFloor[cell] = IsTopCell(cell);
                Grid.HasDoor[cell] = true;
                Grid.RenderedByWorld[cell] = false;
                SimMessages.ClearCellProperties(cell, (byte)Sim.Cell.Properties.Unbreakable);
            }
        }

        private void UpdatePassable()
        {
            bool passable = CurrentState != ControlState.Locked;
            bool permeable = CurrentState == ControlState.Open;

            foreach (int cell in building.PlacementCells)
                SetCellPassable(cell, passable, permeable);
        }

        private static void CleanSim(int cell)
        {
            SimMessages.ClearCellProperties(cell, 12);
            Grid.RenderedByWorld[cell] = RenderedByWorld(cell);
            Grid.FakeFloor[cell] = false;
            Grid.HasDoor[cell] = false;
            Grid.HasAccessDoor[cell] = false;

            if (Grid.Element[cell].IsSolid)
                SimMessages.ReplaceAndDisplaceElement(cell, SimHashes.Vacuum, CellEventLogger.Instance.DoorOpen, 0);
        }

        private void DisplaceElement()
        {
            var pe = GetComponent<PrimaryElement>();
            float mass = pe.Mass / 2;

            foreach (int cell in building.PlacementCells)
            {
                var handle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(OnSimDoorClosed));
                SimMessages.ReplaceAndDisplaceElement(cell, pe.ElementID, CellEventLogger.Instance.DoorClose, mass, pe.Temperature, byte.MaxValue, 0, handle.index);
                SimMessages.SetCellProperties(cell, (byte)Sim.Cell.Properties.Transparent);
                World.Instance.groundRenderer.MarkDirty(cell);
            }
        }

        private void Dig(int cell)
        {
            var item = new Game.CallbackInfo(OnSimDoorOpened);
            var cb = Game.Instance.callbackManager.Add(item);
            SimMessages.Dig(cell, cb.index, true);
        }

        private static void SetCellPassable(int cell, bool passable, bool permeable)
        {
            Game.Instance.SetDupePassableSolid(cell, passable, !permeable);
            Grid.DupeImpassable[cell] = !passable;
            Grid.DupePassable[cell] = passable;
            Pathfinding.Instance.AddDirtyNavGridCell(cell);
        }

        private void UpdateController()
        {
            controller.sm.isOpen.Set(CurrentState == ControlState.Open, controller);
            controller.sm.isClosed.Set(CurrentState == ControlState.Auto, controller);
            controller.sm.isLocked.Set(CurrentState == ControlState.Locked, controller);
        }

        private void OnSimDoorOpened()
        {
            ByPassTemp(false);
            meltCheck = false;
        }

        private void OnSimDoorClosed()
        {
            ByPassTemp(true);
            meltCheck = true;
        }

        private void ByPassTemp(bool bypass)
        {
            var structureTemperatures = GameComps.StructureTemperatures;
            var handle = structureTemperatures.GetHandle(gameObject);

            if (bypass)
                structureTemperatures.Bypass(handle);
            else
                structureTemperatures.UnBypass(handle);
        }

        private static bool RenderedByWorld(int cell)
        {
            var substance = Traverse.Create(Grid.Element[cell].substance);
            return substance.Field("renderedByWorld").GetValue<bool>();
        }

        private bool IsTopCell(int cell)
        {
            return !(building.GetExtents().Contains(Grid.CellToXY(Grid.CellAbove(cell))));
        }
    }
}
