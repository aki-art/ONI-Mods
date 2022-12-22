#if TWITCH
using FUtility;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static STRINGS.UI.NEWBUILDCATEGORIES;

namespace PrintingPodRecharge.Integration.TwitchIntegration
{
    public class FloorUpgrader : KMonoBehaviour, ISim1000ms
    {
        public bool finished;
        private Room room;
        public const int MIN_TILE_COUNT = 5;
        private List<int> tilesToUpgrade;
        private BuildingDef glassTileDef1;
        private BuildingDef glassTileDef2;
        public bool running;
        public Tag element1;
        public Tag element2;
        public bool checkerBoard;

        private HashSet<RoomType> preferredRoomTypes;

        private List<GameObject> testingLines = new List<GameObject>();
        public static bool testingMode = false;

        protected override void OnSpawn()
        {
            checkerBoard = Random.value < 0.5f;

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

            foreach (var room in rooms)
            {
                Log.Debuglog("-  " + room.GetProperName());
            }

            foreach (var room in rooms)
            {
                if (IsRoomEligible(room))
                {
                    this.room = room;
                    if (tilesToUpgrade != null)
                    {
                        tilesToUpgrade.Clear();
                    }
                    tilesToUpgrade = GetFloor(room, 128);

                    var buildingDefs = new List<BuildingDef>(Assets.BuildingDefs).Where(t => t.BuildingComplete.HasTag("DecorPackA_StainedGlass")).ToList();

                    if (buildingDefs == null || buildingDefs.Count < 2)
                    {
                        Log.Warning("Decor Pack I is installed but no stained glass tiles are registered.");
                        return;
                    }

                    buildingDefs.Shuffle();

                    glassTileDef1 = buildingDefs[0];
                    element1 = GetElementFromGlassTileDef(glassTileDef1);

                    if (checkerBoard)
                    {
                        glassTileDef2 = buildingDefs[1];
                        element2 = GetElementFromGlassTileDef(glassTileDef2);
                    }

                    Upgrade();

                    return;
                }
            }
        }

        private Tag GetElementFromGlassTileDef(BuildingDef def)
        {
            var elementName = def.PrefabID
                .Replace("DecorPackA_", "")
                .Replace("StainedGlassTile", "");

            return ElementLoader.FindElementByName(elementName) != null ? (Tag)elementName : SimHashes.Diamond.CreateTag();
        }

        private bool IsUpgradeableFloor(int cell)
        {
            if (Grid.ObjectLayers[(int)ObjectLayer.FoundationTile].TryGetValue(cell, out var go))
            {
                return go != null && go.PrefabID() == TileConfig.ID;
            }

            if (Grid.IsSolidCell(cell))
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

            foreach (var line in testingLines)
            {
                Destroy(line);
            }

            for (int x = room.cavity.minX; x <= room.cavity.maxX; x++)
            {
                for (int y = room.cavity.minY; y <= room.cavity.maxY; y++)
                {
                    var cell = Grid.XYToCell(x, y);
                    if (Game.Instance.roomProber.GetCavityForCell(cell) == room.cavity)
                    {
                        result.Add(cell);
                        if (testingMode)
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
            if (!testingMode)
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

        public List<int> GetFloor(Room room, int maxTiles)
        {
            var roomCells = GetRoomCells(room); //GridUtil.FloodCollectCells(center, cell => IsInRoom(cell, room), 128);

            if (roomCells.Count == 0)
            {
                return null;
            }

            var tiles = new List<int>();

            foreach (var cell in roomCells)
            {
                var cellBelow = Grid.CellBelow(cell);
                if (IsUpgradeableFloor(cellBelow))
                {
                    tiles.Add(cellBelow);

                    if (tiles.Count >= maxTiles)
                    {
                        return tiles;
                    }
                }
            }

            return tiles;
        }

        private bool IsRoomEligible(Room room)
        {
            if (room.roomType == Db.Get().RoomTypes.Neutral || room.roomType == Db.Get().RoomTypes.NatureReserve)
            {
                return false;
            }

            var tiles = GetFloor(room, 20);
            return tiles != null && tiles.Count >= 5;
        }

        void Upgrade()
        {
            tilesToUpgrade.Shuffle();
            running = true;

            var message = room.roomType == Db.Get().RoomTypes.CreaturePen ?
                (string)STRINGS.TWITCH.FLOOR_UPGRADE.TOAST_STABLE
                : STRINGS.TWITCH.FLOOR_UPGRADE.TOAST_GENERIC.Replace("{RoomType}", room.roomType.Name);

            ONITwitchLib.ToastManager.InstantiateToastWithPosTarget(STRINGS.TWITCH.FLOOR_UPGRADE.NAME, message, room.cavity.GetCenter());
        }

        private void SpawnGlassTile(int cell)
        {
            var element = element1;
            var def = glassTileDef1;

            if (checkerBoard)
            {
                Grid.CellToXY(cell, out int x, out int y);
                if (x % 2 != y % 2)
                {
                    element = element2;
                    def = glassTileDef2;
                }
            }

            var elems = new List<Tag>(glassTileDef1.DefaultElements())
            {
                [1] = element
            };

            def.Build(cell, Orientation.Neutral, null, elems, 300f, false, GameClock.Instance.GetTime() + 1);
            World.Instance.blockTileRenderer.Rebuild(ObjectLayer.FoundationTile, cell);

            SpawnSparkles(Grid.CellToPosCCC(cell, Grid.SceneLayer.FXFront));
        }

        private void SpawnSparkles(Vector3 position)
        {
            var sparkles = Instantiate(ModAssets.Prefabs.sparklesParticles);
            sparkles.transform.position = position;
            sparkles.SetActive(true);
            sparkles.GetComponent<ParticleSystem>().Play();
        }

        public void Sim1000ms(float _)
        {
            if (!running)
            {
                return;
            }

            var tileCount = Random.Range(0, 3);

            for (var i = 0; i < tileCount; i++)
            {
                if (tilesToUpgrade != null && tilesToUpgrade.Count > 0)
                {
                    var cell = tilesToUpgrade.First();
                    UpgradeSingleTile(cell);
                    tilesToUpgrade.Remove(cell);
                }
                else
                {
                    Util.KDestroyGameObject(gameObject);
                }
            }
        }

        private void UpgradeSingleTile(int cell)
        {
            if (Grid.ObjectLayers[(int)ObjectLayer.FoundationTile].TryGetValue(cell, out var go))
            {
                if (go.TryGetComponent(out Deconstructable deconstructable))
                {
                    deconstructable.ForceDestroyAndGetMaterials();
                    GameScheduler.Instance.ScheduleNextFrame("", _ => SpawnGlassTile(cell));
                }
            }
            else
            {
                WorldDamage.Instance.ApplyDamage(cell, 1f, -1);
                SpawnGlassTile(cell);
            }
        }
    }
}
#endif