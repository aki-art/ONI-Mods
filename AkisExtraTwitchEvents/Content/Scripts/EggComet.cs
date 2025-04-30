using KSerialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class EggComet : FallingDebris
	{
		[Serialize] public string eggPrefabId;

		private static List<string> validEggs;

		public static Tag[] forbiddenEggTags =
		[
			ONITwitchLib.ExtraTags.OniTwitchSurpriseBoxForceDisabled
		];

		public override void OnSpawn()
		{
			if (!eggPrefabId.IsNullOrWhiteSpace())
				SetEgg(eggPrefabId);
			else
			{
				if (validEggs == null || validEggs.Count == 0)
				{
					validEggs = Assets.GetPrefabsWithTag(GameTags.Egg)
						.Where(p => !p.HasAnyTags(forbiddenEggTags))
						.Select(p => p.PrefabID().name)
						.ToList();
				}

				SetEgg(validEggs.GetRandom());

				spawnAngleMin = -90f;
				spawnAngleMax = -100f;
				RandomizeVelocity();
			}
		}

		public void SetEgg(string eggPrefabId)
		{
			var eggPrefab = Assets.GetPrefab(eggPrefabId);
			var kbac = eggPrefab.GetComponent<KBatchedAnimController>();
			var myKbac = GetComponent<KBatchedAnimController>();
			myKbac.SwapAnims(kbac.animFiles);
			myKbac.Play("idle");

			TryGetComponent(out PrimaryElement primaryElement);
			TryGetComponent(out PrimaryElement eggPrimaryElement);
			primaryElement.SetMass(eggPrimaryElement.Mass);

			this.eggPrefabId = eggPrefabId;
		}

		protected override void DepositTiles(int cell1, Element element, int world, int prev_cell, float temperature)
		{
			var roll = Random.value;
			var position = Grid.CellToPosCCC(Grid.PosToCell(this), Grid.SceneLayer.Ore) with { z = -19.5f };
			position += Vector3.up;

			if (roll < 0.66f)
			{
				var prefab = Assets.GetPrefab(eggPrefabId);
				var mass = prefab.GetComponent<PrimaryElement>().Mass;

				var rawEggGo = FUtility.Utils.Spawn(RawEggConfig.ID, position);
				rawEggGo.GetComponent<PrimaryElement>().Mass = mass * 0.5f;

				var eggShellGo = FUtility.Utils.Spawn(EggShellConfig.ID, position);
				eggShellGo.GetComponent<PrimaryElement>().Mass = mass * 0.5f;

				AudioUtil.PlaySound(ModAssets.Sounds.EGG_SMASH, position, ModAssets.GetSFXVolume() * 0.8f, Random.Range(0.9f, 1.05f));

				Game.Instance.SpawnFX(ModAssets.Fx.eggSplat, Grid.PosToCell(position), Random.Range(0, 360));

			}
			else
			{
				FUtility.Utils.Spawn(eggPrefabId, position);
			}

			Util.KDestroyGameObject(gameObject);
		}
	}
}
