using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ProcGen.SubWorld;

namespace WorldTraitsPlus.WorldEvents
{
    class SinkHole : WorldEvent, ISim200ms
    {
        private int radius = 30;
        private int centerCell;
        List<int> sunkenCells;
        GameObject fx;
        protected override void OnSpawn()
        {
            base.OnSpawn();
            Debug.Log("spawned sinkhole");
            radius = 7;
            duration = 10;
            centerCell = Grid.PosToCell(this);
            if (affectedCells == null || affectedCells.Count == 0)
                affectedCells = SeismicGrid.GetCircle(this, radius, 0);

        }

        public override void Begin()
        {
            base.Begin();
            fx = Util.KInstantiate(ModAssets.sinkholeFX, transform.GetPosition());
            //fx.transform.rotation = Quaternion.Euler(90, 30, 0);
            //fx.transform.transform.position += new Vector3(0, 0, -300);
            fx.transform.transform.localScale = new Vector3(0.1f, 0.1f);
            fx.transform.SetParent(transform);
            fx.SetActive(true);
        }

        public void Sim200ms(float dt)
        {
            elapsedTime += dt;

            if(elapsedTime >= duration)
            {
                End();
                return;
            }

            if (!hasStarted) return;

            int targetRadius = Mathf.CeilToInt(radius * progress);
            List<int> destroyedCells = new List<int>();

            for (int i = 0; i < Random.Range(1, 4); i++)
            {
                int targetCell = SeismicGrid.GetRandomCellInCircle(centerCell, targetRadius, new List<int>(affectedCells.Keys).ToList());
                SimMessages.Dig(targetCell);
                destroyedCells.Add(targetCell);
                /*                float damage = DamageTile(targetCell, 999);
                                if(damage == 0 && World.Instance.zoneRenderData.GetSubWorldZoneType(targetCell) != ZoneType.Space)
                                {
                                }*/
            }

            if(destroyedCells.Count > 0)
            {
                WorldEventManager.Instance.SetBackgroundWall(destroyedCells, ZoneType.Space);
            }
        }
    }
}
