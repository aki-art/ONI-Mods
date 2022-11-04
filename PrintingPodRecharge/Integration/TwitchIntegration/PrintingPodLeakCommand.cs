using FUtility.Components;
using System.Collections.Generic;
using UnityEngine;

namespace PrintingPodRecharge.Integration.TwitchIntegration
{
    public class PrintingPodLeakCommand
    {
        public const string ID = "PrindingPodLeak";

        public static bool Condition()
        {
            return true;
        }

        public static void Run()
        {
            foreach (Telepad telepad in Components.Telepads)
            {
                CreateSpawner(telepad);
            }
        }

        private static void CreateSpawner(Telepad pad)
        {
            var spawnerGo = new GameObject("spawner");
            spawnerGo.transform.position = pad.transform.position + new Vector3(0.5f, 1.5f);

            var spawner = spawnerGo.AddComponent<PrefabSpawner>();
            spawner.minCount = 3;
            spawner.maxCount = 6;
            spawner.yeet = true;
            spawner.yeetMax = 5;
            spawner.yeetOnlyUp = true;
            spawner.fxHash = SpawnFXHashes.BuildingLeakLiquid;
            spawner.soundFx = GlobalAssets.GetSound("Liquid_footstep");
            spawner.volume = 3f;

            spawner.options = new List<(float, Tag)>
            {
                (2f, Items.BioInkConfig.DEFAULT),
                (2f, Items.BioInkConfig.GERMINATED),
                (2f, Items.BioInkConfig.METALLIC),
                (2f, Items.BioInkConfig.SEEDED),
                (2f, Items.BioInkConfig.VACILLATING),
                (2f, Items.BioInkConfig.FOOD),
                (2f, Items.BioInkConfig.SHAKER)
            };
        }
    }
}
