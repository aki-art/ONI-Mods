using STRINGS;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PollingOfONIUITest.Acid
{
    public class AcidCorroder : KMonoBehaviour, ISim1000ms, ISim33ms
    {
        public static AcidCorroder Instance { get; private set; }
        public HashSet<int> acidCells;
        public HashSet<int> lookedAtcolumns;
        public static int maxCount = 64;
        static readonly float damagePerSecond = 15f;
        static readonly float maxHardness = 200f;
        static readonly float syncSeconds = 10f;
        
        protected override void OnPrefabInit()
        {
            acidCells = new HashSet<int>();
            Instance = this;
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            ScanAll();
            StartCoroutine(SyncAcid());
        }

        IEnumerator SyncAcid()
        {
            while(true)
            {
                ScanAll();
                yield return new WaitForSeconds(syncSeconds);
            }
        }

        private void ScanAll()
        {
            int count = acidCells.Count;

            var cells = ListPool<int, AcidCorroder>.Allocate();
            cells.AddRange(acidCells);

            for (int i = 0; i < Grid.CellCount; i++)
            {
                if (Grid.Element[i].id == Acid.AcidSimHash)
                {
                    cells.Add(i);
                }
            }

            acidCells = new HashSet<int>(cells);
            cells.Recycle();
            Debug.Log($"Synced {acidCells.Count - count} cells.");
        }

        public void Sim1000ms(float dt)
        {
            foreach(int i in acidCells)
            {
                if(Grid.Element[i].id == Acid.AcidSimHash)
                {
                    int target = Grid.CellBelow(i);
                    if(ShouldCorrode(target))
                    {
                        DamageCell(target);
                    }
                }
            }
        }

        private void DamageCell(int target)
        {
            WorldDamage.Instance.ApplyDamage(
            target,
            damagePerSecond,
            target,
            BUILDINGS.DAMAGESOURCES.COMET,
            UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.COMET);

            UpdateNeighbours();
        }

        private bool ShouldCorrode(int cell)
        {
            Element element = Grid.Element[cell];
            return Grid.IsValidCell(cell) && Grid.IsSolidCell(cell) && element.id != SimHashes.Unobtanium && element.hardness <= maxHardness;
        }

        public void Sim33ms(float dt)
        {
            UpdateNeighbours();
        }

        private void UpdateNeighbours()
        {
            var cells = ListPool<int, AcidCorroder>.Allocate();
            cells.AddRange(acidCells);

            foreach (int cell in cells)
            {
                RefreshNeighbourState(cell, Direction.Left);
                RefreshNeighbourState(cell, Direction.Right);
                RefreshNeighbourState(cell, Direction.Up);
                RefreshNeighbourState(cell, Direction.Down);
                RefreshNeighbourState(cell, Direction.None);
            }

            acidCells = new HashSet<int>(cells);
            cells.Recycle();
        }

        private void RefreshNeighbourState(int cell, Direction direction)
        {
            int target = direction == Direction.None ? cell : Grid.GetCellInDirection(cell, direction);
            if (IsAcid(target))
                acidCells.Add(target);
            else 
                acidCells.Remove(target);
        }

        private bool IsAcid(int cell) => Grid.Element[cell].id == Acid.AcidSimHash;
    }
}
