using FUtility;
using System.Collections.Generic;
using UnityEngine;

namespace GlassCase.Buildings
{
    public class GlassCase : GlassCasePiece
    {
        [MyCmpGet]
        OccupyArea occupyArea;

        [MyCmpAdd]
        PressureBreakable pressureBreakable;

        [SerializeField]
        public float insulation = 0.25f;

        public GlassCaseValve master;

        private HandleVector<int>.Handle pickupablesChangedEntry;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            int cell = this.NaturalBuildingCell();
            Mod.glassCaseElements.Add(cell, this);
            SimMessages.SetInsulation(cell, insulation);

            pickupablesChangedEntry = GameScenePartitioner.Instance.Add("GlassCase.PickupablesChanged", gameObject, cell, GameScenePartitioner.Instance.pickupablesChangedLayer, OnPickupablesChanged);

            if(masterID != -1 && Mod.glassCaseElements.TryGetValue(masterID, out GlassCasePiece glassCasePiece) && glassCasePiece is GlassCaseValve valve)
            {
                Connect(valve);
            }
            else
            {
                TryConnectToNeighbors(cell);
            }


            string start = "MeteorDamage_Building_";
            var sounds = HarmonyLib.Traverse.Create(typeof(GlobalAssets)).Field<Dictionary<string, string>>("SoundTable").Value;

            foreach (var sound in sounds)
            {
                if (sound.Key.StartsWith(start))
                {
                    Log.Debuglog("Sound", sound.Key);
                }
            }
        }

        protected void TryConnectToNeighbors(int cell)
        {
            Log.Debuglog(ID, "trying to find connections");
            foreach (int cellToCheck in neighbouringCells)
            {
                if (Mod.FindGlassCaseElementOnCell(cellToCheck, out GlassCasePiece gc) && !gc.IsOrphan())
                {
                    master.Refresh();
                    return;
                }
            }
        }

        protected override void OnCleanUp()
        {
            hideFromNetwork = true;
            master.Refresh();
            GameScenePartitioner.Instance.Free(ref pickupablesChangedEntry);
            int cell = this.NaturalBuildingCell();
            Mod.glassCaseElements.Remove(cell);
            SimMessages.SetInsulation(cell, 1f);
            base.OnCleanUp();
        }

        Tag[] notAcceptedTags = new Tag[] {
            GameTags.Creature,
            GameTags.CreatureBrain,
            GameTags.DupeBrain };

        // Stop food from rotting
        private void OnPickupablesChanged(object data)
        {
            int cell = this.NaturalBuildingCell();
            ListPool<ScenePartitionerEntry, LogicMassSensor>.PooledList pooledList = ListPool<ScenePartitionerEntry, LogicMassSensor>.Allocate();
            GameScenePartitioner.Instance.GatherEntries(Grid.CellToXY(cell).x, Grid.CellToXY(cell).y, 1, 1, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
            for (int i = 0; i < pooledList.Count; i++)
            {
                Pickupable pickupable = pooledList[i].obj as Pickupable;
                if (!(pickupable == null) && !pickupable.wasAbsorbed)
                {
                    //if(pickupable.GetDef<Rottable.Def>() is object)
                    if(!pickupable.HasAnyTags(notAcceptedTags))
                    {
                        pickupable.gameObject.AddComponent<Encased>();
                    }
                }
            }

            pooledList.Recycle();
        }

        protected override void OnConnect(GlassCaseValve master)
        {
            SimMessages.SetInsulation(this.NaturalBuildingCell(), master.insulation);
            this.master = master;
        }

        protected override void OnDisconnect()
        {
            masterID = -1;
            master = null;
            SimMessages.SetInsulation(this.NaturalBuildingCell(), insulation);
        }
    }
}
