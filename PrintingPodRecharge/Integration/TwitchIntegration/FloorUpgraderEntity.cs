#if TWITCH
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ONITwitchLib;
using ONITwitchLib.Utils;
using System;
using FUtility;
using System.Linq;

namespace PrintingPodRecharge.Integration.TwitchIntegration
{
    public class FloorUpgrader : MonoBehaviour
    {
        public bool finished;
        public float secondsBetweenPlacements = 0.2f;
        private Room room;
        public const int MIN_TILE_COUNT = 5;
        private HashSet<int> tilesToUpgrade;
        private BuildingDef glassTileDef;

        void Start()
        {
            var rooms = Game.Instance.roomProber.rooms;

            var tiles = Assets.BuildingDefs;
            tiles.Shuffle();
            glassTileDef = tiles.First(t => t.BuildingComplete.HasTag("DecorPackA_StainedGlass"));

            Log.Debuglog("start floor upgrade");

            foreach (var room in rooms)
            {
                if(IsRoomEligible(room))
                {
                    Log.Debuglog("found a good room");
                    this.room = room;
                    tilesToUpgrade = GetFloor(room, 128);
                    //StartCoroutine(UpgradeCoroutine());
                    Upgrade();
                }
            }
        }

        private bool IsUpgradeableFloor(int cell)
        {
            if(Grid.ObjectLayers[(int)ObjectLayer.FoundationTile].TryGetValue(cell, out var go))
            {
                return go.PrefabID() == TileConfig.ID;
            }

            return Grid.IsSolidCell(cell);
        }

        public HashSet<int> GetFloor(Room room, int maxTiles)
        {
            Log.Debuglog("getting floor");
            var center = Grid.PosToCell(room.cavity.GetCenter());
            var roomCells = GridUtil.FloodCollectCells(center, cell => IsInRoom(cell, room), 128);

            Log.Debuglog($"room has {roomCells.Count} cells");

            if (roomCells.Count == 0)
            {
                return null;
            }

            var tiles = new HashSet<int>();

            foreach(var cell in roomCells)
            {
                var cellBelow = Grid.CellBelow(cell);
                if (IsUpgradeableFloor(cellBelow))
                {
                    tiles.Add(cellBelow);

                    if(tiles.Count >= maxTiles)
                    {
                        return tiles;
                    }
                }
            }
            Log.Debuglog($"tiles has {tiles.Count}");

            return tiles;
        }

        private bool IsInRoom(int cell, Room room)
        {
            return Game.Instance.roomProber.GetCavityForCell(cell) == room.cavity;
        }

        private bool IsRoomEligible(Room room)
        {
            if(room.roomType == Db.Get().RoomTypes.Neutral)
            {
                return false;
            }

            var tiles = GetFloor(room, MIN_TILE_COUNT);
            return tiles != null && tiles.Count >= MIN_TILE_COUNT;
        }

       void Upgrade()
        {
            foreach (var cell in tilesToUpgrade)
            {
                if (Grid.ObjectLayers[(int)ObjectLayer.FoundationTile].TryGetValue(cell, out var go))
                {
                    if (go.TryGetComponent(out SimCellOccupier simCellOccupier))
                    {
                        simCellOccupier.DestroySelf(() =>
                        {
                            Destroy(go);
                            SpawnGlassTile(cell);
                        });
                    }
                }
                else
                {
                    SpawnGlassTile(cell);
                }
            }
        }

        private void SpawnGlassTile(int cell)
        {
            glassTileDef.Build(cell, Orientation.Neutral, null, glassTileDef.DefaultElements(), 300f, false, GameClock.Instance.GetTime() + 1);
            World.Instance.blockTileRenderer.Rebuild(ObjectLayer.FoundationTile, cell);
        }

        IEnumerator UpgradeCoroutine()
        {
            foreach (var cell in tilesToUpgrade)
            {
                if (Grid.ObjectLayers[(int)ObjectLayer.FoundationTile].TryGetValue(cell, out var go))
                {
                    if(go.TryGetComponent(out SimCellOccupier simCellOccupier))
                    {
                        simCellOccupier.DestroySelf(() => SpawnGlassTile(cell));
                    }
                }
                else
                {
                    SpawnGlassTile(cell);
                }

                yield return new WaitForSeconds(secondsBetweenPlacements);
            }

            Destroy(gameObject);
        }

    }
}
#endif