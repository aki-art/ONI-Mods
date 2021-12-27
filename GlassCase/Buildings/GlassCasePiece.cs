using FUtility;
using KSerialization;
using System;
using System.Collections.Generic;

namespace GlassCase.Buildings
{
    public abstract class GlassCasePiece : KMonoBehaviour
    {
        public int ID => this.NaturalBuildingCell();

        [Serialize]
        public int masterID;

        protected int[] neighbouringCells = new int[4];

        [Serialize]
        public bool hideFromNetwork = false;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            int cell = this.NaturalBuildingCell();
            neighbouringCells = new int[4] { Grid.CellAbove(cell), Grid.CellRight(cell), Grid.CellBelow(cell), Grid.CellLeft(cell) }; // TODO: validity check
            GetComponent<KSelectable>().AddStatusItem(ModAssets.disconnectedStatus);
        }

        protected void ForAllNeighbors(Action<GlassCasePiece> action)
        {
            foreach (int cellToCheck in neighbouringCells)
            {
                if (Mod.FindGlassCaseElementOnCell(cellToCheck, out GlassCasePiece gc))
                {
                    action.Invoke(gc);
                }
            }
        }

        protected abstract void OnConnect(GlassCaseValve master);

        public void Connect(GlassCaseValve master, bool notifyNeighbors = true)
        {
            masterID = master.ID;
            Log.Debuglog($"Connected {ID} to {masterID}.");

            if (notifyNeighbors)
            {
                ForAllNeighbors(gc =>
                {
                    if (gc.IsOrphan()) gc.Connect(master);
                });
            }

            KSelectable kSelectable = GetComponent<KSelectable>();
            if (kSelectable.HasStatusItem(ModAssets.disconnectedStatus)) {
                kSelectable.AddStatusItem(ModAssets.connectedStatus);
                kSelectable.RemoveStatusItem(ModAssets.disconnectedStatus);
            }

            OnConnect(master);
        }

        protected override void OnCleanUp()
        {
            Disconnect();
            base.OnCleanUp();
        }

        protected abstract void OnDisconnect();

        public void Disconnect()
        {
            GetComponent<KSelectable>().AddStatusItem(ModAssets.connectedStatus);
            GetComponent<KSelectable>().RemoveStatusItem(ModAssets.disconnectedStatus);
            OnDisconnect();
        }

        public bool IsOrphan() => masterID != -1;
    }
}
