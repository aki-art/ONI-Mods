#if TWITCH
using FUtility;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

        private HashSet<RoomType> preferredRoomTypes;

        private List<GameObject> testingLines = new List<GameObject>();
        public static bool testingMode = false;

        void Start()
        {
            var roomTypes = Db.Get().RoomTypes;
            if (preferredRoomTypes == null)
            {
                preferredRoomTypes = new HashSet<RoomType>()
                {
                    roomTypes.MessHall,
                    roomTypes.GreatHall,
                    roomTypes.Barracks,
                    roomTypes.Bedroom,
                    roomTypes.PrivateBedroom,
                    roomTypes.Hospital,
                    roomTypes.Kitchen,
                    roomTypes.Latrine,
                    roomTypes.Laboratory,
                    roomTypes.MassageClinic,
                    roomTypes.RecRoom,
                    roomTypes.PlumbedBathroom
                };
            }

            var rooms = new List<Room>(Game.Instance.roomProber.rooms);
            // only the ones in valid worlds
            rooms.RemoveAll(r => r.roomType == roomTypes.Neutral);
            rooms = rooms.OrderBy(room => preferredRoomTypes.Contains(room.roomType) ? 0 : 1).ToList();

            foreach(var room in rooms)
            {
                Log.Debuglog("-  " + room.GetProperName());
            }

            foreach (var room in rooms)
            {
                if (IsRoomEligible(room))
                {
                    this.room = room;
                    if(tilesToUpgrade != null)
                    {
                        tilesToUpgrade.Clear();
                    }
                    tilesToUpgrade = GetFloor(room, 128);

                    var tiles = Assets.BuildingDefs;
                    tiles.Shuffle();
                    glassTileDef = tiles.First(t => t.BuildingComplete.HasTag("DecorPackA_StainedGlass"));

                    var elementName = glassTileDef.PrefabID
                        .Replace("DecorPackA_", "")
                        .Replace("StainedGlassTile", "");

                    var element = ElementLoader.FindElementByName(elementName) != null ? (Tag)elementName : SimHashes.Diamond.CreateTag();

                    Upgrade(element);
                    return;
                }
            }
        }

        private bool IsUpgradeableFloor(int cell)
        {
            if(Grid.ObjectLayers[(int)ObjectLayer.FoundationTile].TryGetValue(cell, out var go))
            {
                return go != null && go.PrefabID() == TileConfig.ID;
            }

            if(Grid.IsSolidCell(cell))
            {
                // dont yoink out natural tiles under plants
                // (plants are on the building layer, not the plants layer)
                if (Grid.ObjectLayers[(int)ObjectLayer.Building].TryGetValue(Grid.CellAbove(cell), out var goAbove))
                {
                    var isPlant = goAbove != null && (goAbove.GetComponent<Uprootable>() != null || goAbove.GetComponent<BasicForagePlantPlanted>() != null);
                    return !isPlant;
                }

                return true;
            }

            return false;
        }

        // TODO: this can be a lot more efficient
        private HashSet<int> GetRoomCells(Room room)
        {
            var result = new HashSet<int>();
            var testColor = Random.ColorHSV(0, 1, 1, 1, 1, 1);
            testColor.a = 0.5f;

            foreach(var line in testingLines)
            {
                Destroy(line);
            }

            for(int x = room.cavity.minX; x <= room.cavity.maxX; x++)
            {
                for(int y = room.cavity.minY; y <= room.cavity.maxY; y++)
                {
                    var cell = Grid.XYToCell(x, y);
                    if (Game.Instance.roomProber.GetCavityForCell(cell) == room.cavity)
                    {
                        result.Add(cell);
                        if(testingMode)
                        {
                            AddDebugMarker(cell, testColor);
                        }
                    }
                }
            }

            return result;
        }

        private void AddDebugMarker(int cell, Color color)
        {
            if(!testingMode)
            {
                return;
            }

            // TESTING ONLY
            var testGo = new GameObject("test");
            testGo.SetActive(true);

            var line = testGo.AddComponent<LineRenderer>();

            line.material = new Material(Shader.Find("Klei/Biome/Unlit Transparent"))
            {
                renderQueue = RenderQueues.Liquid
            };

            var cellPos = Grid.CellToPos(cell);
            cellPos.z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2);

            line.SetPositions(new[]
            {
                cellPos + new Vector3(0, 0.5f),
                cellPos + new Vector3(1, 0.5f),
            });

            line.positionCount = 2;
            line.startColor = line.endColor = color;
            line.startWidth = line.endWidth = 0.45f;

            testingLines.Add(testGo);
        }

        public HashSet<int> GetFloor(Room room, int maxTiles)
        {
            var roomCells = GetRoomCells(room); //GridUtil.FloodCollectCells(center, cell => IsInRoom(cell, room), 128);

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

            return tiles;
        }

        private bool IsRoomEligible(Room room)
        {
            if(room.roomType == Db.Get().RoomTypes.Neutral || room.roomType == Db.Get().RoomTypes.NatureReserve)
            {
                return false;
            }

            var tiles = GetFloor(room, 20);
            return tiles != null && tiles.Count >= 5;
        }

       void Upgrade(Tag element)
        {
            foreach (var cell in tilesToUpgrade)
            {
                if (Grid.ObjectLayers[(int)ObjectLayer.FoundationTile].TryGetValue(cell, out var go))
                {
                    if(go.TryGetComponent(out Deconstructable deconstructable))
                    {
                        deconstructable.ForceDestroyAndGetMaterials();
                        GameScheduler.Instance.ScheduleNextFrame("", _ => SpawnGlassTile(cell, element));
                    }
                }
                else
                {
                    WorldDamage.Instance.ApplyDamage(cell, 1f, -1);
                    SpawnGlassTile(cell, element);
                }
            }

            var message = room.roomType == Db.Get().RoomTypes.CreaturePen ?
                (string)STRINGS.TWITCH.FLOOR_UPGRADE.TOAST_STABLE
                : STRINGS.TWITCH.FLOOR_UPGRADE.TOAST_GENERIC.Replace("{RoomType}", room.roomType.Name);

            ONITwitchLib.ToastManager.InstantiateToastWithPosTarget(STRINGS.TWITCH.FLOOR_UPGRADE.NAME, message, room.cavity.GetCenter());
        }

        private void SpawnGlassTile(int cell, Tag element)
        {
            var elems = new List<Tag>(glassTileDef.DefaultElements())
            {
                [1] = element
            };

            glassTileDef.Build(cell, Orientation.Neutral, null, elems, 300f, false, GameClock.Instance.GetTime() + 1);
            World.Instance.blockTileRenderer.Rebuild(ObjectLayer.FoundationTile, cell);
        }
    }
}
#endif