using FUtility.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CrittersDropBones.Integration.TwitchIntegration
{
    public class MessHallMesser : KMonoBehaviour
    {
        public HashSet<int> targetCells = new HashSet<int>();

        private void CreateSpawner(int cell)
        {
            GameObject spawnerGo = new GameObject("spawner");
            spawnerGo.transform.position = Grid.CellToPosCCC(cell, Grid.SceneLayer.Creatures);
            var spawner = spawnerGo.AddComponent<PrefabSpawner>();
            spawner.minCount = 1;
            spawner.maxCount = 10;
            spawner.options = new List<(float, Tag)>
            {
                (1f, BasicForagePlantConfig.ID),
                (1f, Items.FishBoneConfig.ID),
                //(0.5f, Items.LargeBoneConfig.ID),
                (5f, Items.BoneConfig.ID),
                (0.3f, GlomConfig.ID),
                (5f, RotPileConfig.ID),
                (0.4f, SimHashes.ToxicSand.CreateTag()),
                (0.5f, SimHashes.DirtyWater.CreateTag()),
                (0.5f, SimHashes.ContaminatedOxygen.CreateTag())
            };
        }

        private void BreakBuilding()
        {

        }
    }
}
