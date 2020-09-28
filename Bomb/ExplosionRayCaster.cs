using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bomb
{
    public class ExplosionRayCaster
    {
        public static Dictionary<int, Cell> occlusionMap;
        const float LIQUID_DAMPENING = 0.25f;

        public static void Test(List<Vector2I> cells)
        {
            occlusionMap = new Dictionary<int, Cell>();
            foreach(var cell in cells)
            {
                occlusionMap[Grid.PosToCell(cell)] = new Cell(cell);
            }
        }

        public struct Cell
        {
            public float occlusion;
            public readonly Vector2I position;
            public readonly int cell;
            public readonly bool isFoundationTile;
            private readonly Element element;
            private readonly GameObject gameObject;
            private readonly bool bunkerTile;
            
            public Cell(Vector2I position)
            {
                this.position = position;
                cell = Grid.PosToCell(position);
                occlusion = 0;
                gameObject = Grid.Objects[cell, (int)ObjectLayer.FoundationTile];
                isFoundationTile = false;
                bunkerTile = false;

                if (gameObject != null)
                {
                    bunkerTile = gameObject.HasTag(GameTags.Bunker);
                    var sco = gameObject.GetComponent<SimCellOccupier>();
                    if(sco != null && !sco.doReplaceElement)
                    {
                        isFoundationTile = true;
                    }
                }
                

                element = isFoundationTile ? gameObject.GetComponent<PrimaryElement>().Element : Grid.Element[cell];
                occlusion = GetOcclusion();
            }

            private float GetOcclusion()
            {
                // if in liquid
                if (!isFoundationTile && Grid.IsSubstantialLiquid(cell))
                    return LIQUID_DAMPENING;

                // if there is nothing in the cell
                
                if (element.id == SimHashes.Vacuum || !Grid.IsSolidCell(cell))
                    return 0f;

                // if the cell is impenetrable
                if (element.id == SimHashes.Unobtanium || element.hardness == 0 || bunkerTile)
                    return 1f;

                // cast is neccessary or it will round to int
                return GetHealth() * ((float)element.hardness / (float)byte.MaxValue);
            }

            private float GetHealth()
            {
                if(isFoundationTile)
                {
                    BuildingHP bhp = gameObject.GetComponent<BuildingHP>();
                    return bhp.HitPoints / bhp.MaxHitPoints;
                }
                else
                {
                    return Math.Max(1f, Grid.Mass[cell] / element.defaultValues.mass);
                }
            }

            public float Damage(List<PrimaryElement> debrisAccumulator)
            {
                return 0;
            }

        }
    }
}
