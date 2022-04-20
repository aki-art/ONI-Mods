using FUtility.Components;
using System.Collections.Generic;
using UnityEngine;

namespace PrintingPodRecharge.Integration.TwitchIntegration
{
    public class PrintingPodLeakCommand
    {
        public const string ID = "PringingPodLeak"; 
        
        public static bool Condition() => true;

        public static void Run()
        {
            foreach(Telepad telepad in Components.Telepads)
            {
                CreateSpawner(telepad);
            }
        }

        private static void CreateSpawner(Telepad pad)
        {
            var spawnerGo = new GameObject("spawner");
            spawnerGo.transform.position = pad.transform.position + new Vector3(0.5f, 1.5f);

            var spawner = spawnerGo.AddComponent<PrefabSpawner>();
            spawner.minCount = 4;
            spawner.maxCount = 10;
            spawner.yeet = true;
            spawner.yeetMax = 5;
            spawner.yeetOnlyUp = true;
            spawner.fxHash = SpawnFXHashes.BuildingLeakLiquid;
            spawner.soundFx = GlobalAssets.GetSound("Liquid_footstep");
            spawner.volume = 3f;

            spawner.options = new List<(float, Tag)>
            {
                (1f, Items.BioInkConfig.DEFAULT),
                (1f, Items.BioInkConfig.GERMINATED),
                (1f, Items.BioInkConfig.METALLIC),
                (1f, Items.BioInkConfig.SEEDED),
                (1f, Items.BioInkConfig.VACILLATING),
                (1f, Items.BioInkConfig.FOOD)
            };
        }
    }
}
