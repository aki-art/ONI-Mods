using Bestagons.Content.Defs;
using Bestagons.Content.Map;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bestagons.Content.Scripts
{
    public class Bestagons_HexagonGrid : KMonoBehaviour
    {
        public Dictionary<HexCoord, PurchasableHex> hexLookup;
        public HexCoord startHex;

        // template size
        public const int HEX_BOUNDS_WIDTH = 19;
        public const int HEX_BOUNDS_HEIGHT = 21;
        // template stacks vertically at every nth of this
        public const float STACKING_HEIGHT = 15;

        public const float VERTICAL_SCALE = 10f;
        public const float HORIZONTAL_SCALE = 10.3923045f; // precalculated, so eventually stops being accurate,
                                                           // but not an issue for these small maps
                                                           // not the same as a real hexagonal shape, because of the tiled restriction
        public Vector2 offset = new(0.5f, 0.5f);
        private Vector2 scaleVec = new(VERTICAL_SCALE, HORIZONTAL_SCALE);

        [MyCmpReq]
        public WorldContainer worldContainer;

        public override void OnSpawn()
        {
            base.OnSpawn();
            hexLookup = new();

            if (Bestagons_Mod.Instance.GetWorldState(this.GetMyWorldId()) == Bestagons_Mod.WorldState.UnInitialized)
            {
                Generate();
                CoverArea();
            }


            Log.Debug("hex grid created for world " + this.GetMyWorld()?.worldName);
        }

        private void CoverArea()
        {
            Bestagons_Mod.Instance.HideWorld(worldContainer.id);
        }

        public void Generate()
        {
            Log.Info("= Generating Hex tiles =");

            if(ClusterManager.Instance == null)
                Log.Warning("ClusterManager.Instance is null");

            var world = ClusterManager.Instance.GetWorld(worldContainer.id);

            if (world == null)
                Log.Warning("world is null");

            int xCount = Mathf.FloorToInt(world.Width / HEX_BOUNDS_WIDTH) - 1;
            int yCount = Mathf.FloorToInt(world.Height / STACKING_HEIGHT) - 1;

            for (int r = 0; r <= yCount; r++)
            {
                int rOffset = Mathf.FloorToInt(r / 2f);
                for (int q = 0 - rOffset; q <= xCount - rOffset; q++)
                {
                    var coord = new HexCoord(q, r);
                    var hex = CreateNewHex(coord);
                    hexLookup[coord] = hex;
                }
            }

            new HexMapGenerator(ref hexLookup, new HexCoord(xCount / 2, yCount / 2), "BestagonsMedium", this, world);

            Bestagons_Mod.Instance.SetWorldState(this.GetMyWorldId(), Bestagons_Mod.WorldState.Generated);
            Log.Info($"= Generated {hexLookup.Count} hexes =");
        }


        public HexCoord GetHexAtPos(Vector2 pos) => HexCoord.AtPosition(pos / scaleVec);

        private PurchasableHex CreateNewHex(HexCoord hex)
        {
            var prefab = Assets.GetPrefab(PurchasableHexConfig.ID);

            var position = hex.Position();
            position *= new Vector2(HORIZONTAL_SCALE, VERTICAL_SCALE);
            position += offset;

            var go = GameUtil.KInstantiate(prefab, position, Grid.SceneLayer.SceneMAX);
            go.SetActive(true);

            if(go.TryGetComponent(out PurchasableHex purchasableHex))
            {
                purchasableHex.location = hex;
            }

            return purchasableHex;
        }
    }
}
