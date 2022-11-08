using FMOD.Studio;
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
            var telepad = GameUtil.GetActiveTelepad();

            var chance = 0.25f;//data.TryGetValue("LeekChance", out object val) ? (float)val : 0.25f;
            var typo = Random.value < chance;

            if(typo)
            {
                CreateLeekSpawners(telepad.GetComponent<Telepad>());
                ToastHelper.ToastToTarget("Leeky Printing Pod!", "* The author is profusely apologizing for the typo. *", telepad);
            }
            else
            {
                CreateSpawner(telepad.GetComponent<Telepad>());
                ToastHelper.ToastToTarget("Leaky Printing Pod!", "Your telepad is leaking ink everywhere!", telepad);
            }
        }

        private static void CreateLeekSpawners(Telepad pad)
        {
            var spawnerGo = new GameObject("spawner");
            var pos = pad.transform.position + new Vector3(0.5f, 1.5f);
            spawnerGo.transform.position = pos;
            var meteorSound = GlobalAssets.GetSound("Meteor_Medium_Impact");
            var liquidSound = GlobalAssets.GetSound("Liquid_footstep");

            var spawner = spawnerGo.AddComponent<PrefabSpawner>();
            spawner.minCount = 30;
            spawner.maxCount = 60;
            spawner.yeet = true;
            spawner.yeetMax = 5;
            spawner.yeetOnlyUp = false;
            spawner.fxHash = SpawnFXHashes.BuildingLeakGas;
            spawner.volume = 5f;
            spawner.rotate = true;
            spawner.OnItemSpawned += go =>
            {
                var sound = liquidSound;

                if (Random.value < 0.1f)
                {
                    Game.Instance.SpawnFX(SpawnFXHashes.MeteorImpactMetal, spawnerGo.transform.position, 0f);
                    sound = meteorSound;
                }

                if (CameraController.Instance.IsAudibleSound(pos, meteorSound))
                {
                    pos.z = 0f;

                    var instance = KFMOD.BeginOneShot(sound, pos, 0.8f);
                    instance.setParameterByName("userVolume_SFX", KPlayerPrefs.GetFloat("Volume_SFX"));
                    KFMOD.EndOneShot(instance);
                }
            };

            spawner.options = new List<(float, Tag)>
            {
                (0.5f, Items.LeekConfig.ID)
            };
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
