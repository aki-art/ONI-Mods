using FUtility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
    public class MidasToucher : KMonoBehaviour, ISim200ms
    {
        [SerializeField]
        public float lifeTime;

        [SerializeField]
        public float radius;

        private float elapsedLifeTime = 0;

        private BuildingDef metalTile;
        private BuildingDef meshTile;
        private BuildingDef stainedGlassTile;

        private HashSet<int> alreadyVisitedCells;

        private static Dictionary<SimHashes, SimHashes> elementLookup = new()
        {
            { SimHashes.Water, SimHashes.DirtyWater},
            { SimHashes.Oxygen, SimHashes.ContaminatedOxygen},
            { SimHashes.SaltWater, SimHashes.DirtyWater},
            { SimHashes.Ice, SimHashes.DirtyIce},
            { SimHashes.Magma, SimHashes.MoltenGold},
        };

        public override void OnPrefabInit()
        {
            base.OnPrefabInit();
            metalTile = Assets.GetBuildingDef(MetalTileConfig.ID);
            stainedGlassTile = Assets.GetBuildingDef("DecorPackA_GoldStainedGlassTile");
            alreadyVisitedCells = new HashSet<int>();
        }

        public void Sim200ms(float dt)
        {
            elapsedLifeTime += dt;

            if (elapsedLifeTime > lifeTime)
            {
                Util.KDestroyGameObject(gameObject);
                return;
            }

            var position = Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos());
            var cells = GetTilesInRadius(position, radius);
            var worldIdx = this.GetMyWorldId();

            foreach (var offset in cells)
            {
                var cell = Grid.PosToCell(offset);
                if (Grid.IsValidCellInWorld(cell, worldIdx))
                {
                    TurnToGold(cell);
                }
            }
        }

        private void TurnToGold(int cell)
        {
            if (alreadyVisitedCells.Contains(cell))
            {
                return;
            }

            alreadyVisitedCells.Add(cell);

            var element = Grid.Element[cell];

            if (element == null)
            {
                return;
            }

            if (elementLookup.TryGetValue(element.id, out var newElement))
            {
                SimMessages.ReplaceElement(
                    cell,
                    newElement,
                    CellEventLogger.Instance.DebugTool,
                    Grid.Mass[cell],
                    Grid.Temperature[cell],
                    Grid.DiseaseIdx[cell],
                    Grid.DiseaseCount[cell]);
            }

            // ground
            if (element.IsSolid && element.id != SimHashes.Gold)
            {
                SimMessages.ReplaceElement(
                    cell,
                    SimHashes.Gold,
                    CellEventLogger.Instance.DebugTool,
                    Grid.Mass[cell],
                    Grid.Temperature[cell],
                    Grid.DiseaseIdx[cell],
                    Grid.DiseaseCount[cell]);
            }

            // buildings
            /*            var objects = new List<GameObject>
                        {
                            Grid.Objects[cell, (int)ObjectLayer.Backwall],
                            Grid.Objects[cell, (int)ObjectLayer.Wire],
                            Grid.Objects[cell, (int)ObjectLayer.Building],
                            Grid.Objects[cell, (int)ObjectLayer.GasConduit],
                            Grid.Objects[cell, (int)ObjectLayer.LiquidConduit],
                            Grid.Objects[cell, (int)ObjectLayer.SolidConduit],
                            Grid.Objects[cell, (int)ObjectLayer.FoundationTile],
                            Grid.Objects[cell, (int)ObjectLayer.LogicWire]
                        };*/

            var layers = new[]
            {
               (int)ObjectLayer.Backwall,
               (int)ObjectLayer.Wire,
               (int)ObjectLayer.Building,
               (int)ObjectLayer.GasConduit,
               (int)ObjectLayer.LiquidConduit,
               (int)ObjectLayer.SolidConduit,
               (int)ObjectLayer.FoundationTile,
               (int)ObjectLayer.LogicWire
            };

            foreach (var layer in layers)
            {
                if (Grid.ObjectLayers[layer].TryGetValue(cell, out var go))
                {
                    if (go.TryGetComponent(out Building building))
                    {
                        if (go.TryGetComponent(out PrimaryElement primaryElement) && go.TryGetComponent(out Deconstructable deconstructale))
                        {
                            if (primaryElement.Element.id != SimHashes.Gold)
                            {
                                primaryElement.SetElement(SimHashes.Gold);

                                if (deconstructale.constructionElements != null)
                                {
                                    deconstructale.constructionElements[0] = SimHashes.Gold.CreateTag();
                                }
                            }
                        }

                        if (building.Def?.BlockTileAtlas != null)
                        {
                            if (stainedGlassTile != null && go.HasTag("DecorPackA_StainedGlass") && building.PrefabID() != stainedGlassTile.PrefabID)
                            {
                                stainedGlassTile.TryReplaceTile(go, go.transform.position, Orientation.Neutral, new List<Tag>
                            {
                                SimHashes.Diamond.CreateTag(),
                                SimHashes.Gold.CreateTag()
                            });
                            }

                            World.Instance.blockTileRenderer.Rebuild(ObjectLayer.FoundationTile, cell);
                        }
                    }
                }
            }
        }

        private List<Vector2I> GetTilesInRadius(Vector3 position, float radius)
        {
            return ProcGen.Util.GetFilledCircle(position, radius);
        }
    }
}
