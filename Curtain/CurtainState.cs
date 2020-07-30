using Harmony;

namespace Curtain
{
    public partial class Curtain
    {
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
            }
        }

        private void UpdatePassable()
        {
            bool passable = state.HasFlag(State.Passable);
            bool permeable = state.HasFlag(State.Permeable);

            foreach (int cell in building.PlacementCells)
            {
                Game.Instance.SetDupePassableSolid(cell, passable, !permeable);
                Grid.DupeImpassable[cell] = !passable;
                Grid.DupePassable[cell] = passable;
                Pathfinding.Instance.AddDirtyNavGridCell(cell);
            }
        }

        private static void CleanPassable(int cell)
        {
            Grid.HasDoor[cell] = false;
            Grid.HasAccessDoor[cell] = false;
            Game.Instance.SetDupePassableSolid(cell, false, Grid.Solid[cell]);
            Grid.CritterImpassable[cell] = false;
            Grid.DupeImpassable[cell] = false;

            Pathfinding.Instance.AddDirtyNavGridCell(cell);
        }

        private static void CleanSim(int cell)
        {
            SimMessages.ClearCellProperties(cell, 12);
            Grid.RenderedByWorld[cell] = RenderedByWorld(cell);
            Grid.FakeFloor[cell] = false;

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

                World.Instance.groundRenderer.MarkDirty(cell);
            }
        }

        private void Dig(int cell)
        {
            var item = new Game.CallbackInfo(OnSimDoorOpened);
            var cb = Game.Instance.callbackManager.Add(item);
            SimMessages.Dig(cell, cb.index, true);
        }


        private void OnSimDoorOpened() => ByPassTemp(false);

        private void OnSimDoorClosed() => ByPassTemp(true);

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
