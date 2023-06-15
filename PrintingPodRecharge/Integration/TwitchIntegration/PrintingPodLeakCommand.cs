using FUtility.Components;
using PrintingPodRecharge.Content.Cmps;
using PrintingPodRecharge.Content.Items;
using System.Collections.Generic;
using UnityEngine;

namespace PrintingPodRecharge.Integration.TwitchIntegration
{
	public class PrintingPodLeakCommand
	{
		public const string ID = "PrindingPodLeak";

		public static bool Condition(object _) => Components.Telepads != null && Components.Telepads.Count > 0;

		public static void Run(object data)
		{
			var telepad = GameUtil.GetActiveTelepad();

			var chance = 0.33f;
			var typo = ImmigrationModifier.Instance.hasHadLeaky && Random.value < chance;

			if (typo)
			{
				CreateLeekSpawners(telepad.GetComponent<Telepad>());

				ONITwitchLib.ToastManager.InstantiateToastWithGoTarget(
					STRINGS.TWITCH.PRINTING_POD_LEEK.NAME,
					STRINGS.TWITCH.PRINTING_POD_LEEK.DESC,
					telepad);

				ImmigrationModifier.Instance.hasHadLeaky = true;
			}
			else
			{
				CreateSpawner(telepad.GetComponent<Telepad>());

				ONITwitchLib.ToastManager.InstantiateToastWithGoTarget(
					STRINGS.TWITCH.PRINTING_POD_LEAK.NAME,
					STRINGS.TWITCH.PRINTING_POD_LEAK.DESC,
					telepad);
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
				(0.5f, LeekConfig.ID)
			};
		}

		private static void CreateSpawner(Telepad pad)
		{
			var spawnerGo = new GameObject("spawner");
			spawnerGo.transform.position = pad.transform.position + new Vector3(0.5f, 1.5f);

			var spawner = spawnerGo.AddComponent<PrefabSpawner>();
			spawner.minCount = 4;
			spawner.maxCount = 8;
			spawner.yeet = true;
			spawner.yeetMax = 5;
			spawner.yeetOnlyUp = true;
			spawner.fxHash = SpawnFXHashes.BuildingLeakLiquid;
			spawner.soundFx = GlobalAssets.GetSound("Liquid_footstep");
			spawner.volume = 3f;

			spawner.options = new List<(float, Tag)>
			{
				(2f, BioInkConfig.GERMINATED),
				(2f, BioInkConfig.GERMINATED),
				(2f, BioInkConfig.GERMINATED),
				(2f, BioInkConfig.METALLIC),
				(2f, BioInkConfig.METALLIC),
				(2f, BioInkConfig.METALLIC),
				(2f, BioInkConfig.SEEDED),
				(2f, BioInkConfig.SEEDED),
				(2f, BioInkConfig.SEEDED),
				(2f, BioInkConfig.VACILLATING),
				(2f, BioInkConfig.FOOD),
				(2f, BioInkConfig.FOOD),
				(2f, BioInkConfig.FOOD),
				(2f, BioInkConfig.SHAKER)
			};

			if (Mod.otherMods.IsDiseasesExpandedHere)
			{
				spawner.options.Add((2f, BioInkConfig.MEDICINAL));
				spawner.options.Add((2f, BioInkConfig.MEDICINAL));
				spawner.options.Add((2f, BioInkConfig.MEDICINAL));
			}
		}
	}
}
