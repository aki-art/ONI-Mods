using ONITwitchLib.Utils;
using System.Collections.Generic;
using Twitchery.Utils;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	internal class PipSplosionSpawner : KMonoBehaviour, ISim33ms
	{
		[SerializeField] public int minPips;
		[SerializeField] public int maxPips;

		public string impactSound = "Meteor_Large_Impact";
		public Vector2 explosionSpeedRange = new(8f, 14f);
		public int splashRadius = 1;

		private int pipsToSpawn;
		private int pipsSpawned;

		public static List<CellOffset> targetTiles = FUtility.Utils.MakeCellOffsetsFromMap(false,
			" X ",
			"XOX",
			" X ");

		public override void OnSpawn()
		{
			base.OnSpawn();
			pipsToSpawn = Random.Range(minPips, maxPips + 1);
		}

		public void Sim33ms(float dt)
		{
			if (Random.value > 0.1f)
				return;

			var cell = PosUtil.RandomCellNearMouse();

			if (!ValidSpawnCell(cell))
				return;

			var pos = Grid.CellToPos(cell);
			Game.Instance.SpawnFX(SpawnFXHashes.MissileExplosion, pos, 0);

			ExplosionUtil.Explode(cell, 0.33f, explosionSpeedRange, targetTiles);
			PlayImpactSound(impactSound, pos);
			SpawnPip(cell);

			if (pipsSpawned >= pipsToSpawn)
				Util.KDestroyGameObject(gameObject);
		}

		private void SpawnPip(int cell)
		{
			FUtility.Utils.Spawn(SquirrelConfig.ID, Grid.CellToPos(cell) with { z = Grid.GetLayerZ(Grid.SceneLayer.Move) });
			pipsSpawned++;
		}

		private void PlayImpactSound(string impactSound, Vector3 position)
		{
			string sound = GlobalAssets.GetSound(impactSound);

			var cell = Grid.PosToCell(position);
			if (!Grid.IsValidCell(cell) || Grid.WorldIdx[cell] != ClusterManager.Instance.activeWorldId)
				return;

			position.z = 0.0f;

			var instance = KFMOD.BeginOneShot(sound, position, 0.3f);
			instance.setParameterByName("userVolume_SFX", KPlayerPrefs.GetFloat("Volume_SFX"));
			KFMOD.EndOneShot(instance);
		}

		private bool ValidSpawnCell(int cell) =>
			Grid.IsValidCell(cell)
			&& Grid.Element[cell].hardness < byte.MaxValue;
	}
}
