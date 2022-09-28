﻿/*using FUtility.Components;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CrittersDropBones.Integration.TwitchIntegration.Commands
{
    public class MessyMessHallCommand
    {
        public const string ID = "cdb_MessyMessHall";

        private HashSet<int> visitedCells = new HashSet<int>();
        private HashSet<HashedString> messHalls = new HashSet<HashedString>()
        {
            new HashedString("MessHall"),
            new HashedString("GreatHall")
        };

        public static bool Condition()
        {
            return true;
        }

        public static Dictionary<string, object> GetDefaultConfig()
        {
            return new Dictionary<string, object>()
            {
                { "SomeData", "Value" }
            };
        }

        public static void Run(int danger, Dictionary<string, object> data)
        {
            new MessyMessHallCommand().Run();
        }

        public void Run()
        {
            var rooms = Game.Instance.roomProber.rooms;
            foreach (var room in rooms)
            {
                if (IsMessHall(room))
                {
                    var id = room.roomType.IdHash;
                    var startCell = room.buildings.ElementAt(0).NaturalBuildingCell();

                    GameUtil.FloodFillConditional(startCell, c => IsConnectedToRoom(c, id), visitedCells, null);
                    foreach (var cell in visitedCells)
                    {
                        if (Random.value < 0.2f)
                        {
                            CreateSpawner(cell);
                        }
                    }

                    visitedCells.Clear();
                }
            }
        }

        private bool IsMessHall(Room room)
        {
            return messHalls.Contains(room.roomType.IdHash) && room.buildings.Count > 0;
        }

        private bool IsConnectedToRoom(int cell, HashedString roomId)
        {
            return Game.Instance.roomProber.GetCavityForCell(cell) is CavityInfo cavity && roomId == cavity.room.roomType.IdHash;
        }

        private void CreateSpawner(int cell)
        {
            var spawnerGo = new GameObject("spawner");
            spawnerGo.transform.position = Grid.CellToPosCCC(cell, Grid.SceneLayer.Creatures);

            var spawner = spawnerGo.AddComponent<PrefabSpawner>();
            spawner.minCount = 1;
            spawner.maxCount = 10;
            spawner.options = new List<(float, Tag)>
            {
                (1f, BasicForagePlantConfig.ID),
                (1f, Items.FishBoneConfig.ID),
                //(0.5f, Items.LargeBoneConfig.ID),
                (1f, Items.BoneConfig.ID),
                (0.3f, GlomConfig.ID),
                (5f, RotPileConfig.ID),
                (0.4f, SimHashes.ToxicSand.CreateTag()),
                (0.5f, SimHashes.DirtyWater.CreateTag()),
                (0.5f, SimHashes.ContaminatedOxygen.CreateTag())
            };
        }

    }
}
*/