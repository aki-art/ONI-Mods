using ONITwitchLib.Utils;
using System.Collections.Generic;
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

			Explode(cell);
			SpawnPip(cell);

			if (pipsSpawned >= pipsToSpawn)
				Util.KDestroyGameObject(gameObject);
		}

		private void SpawnPip(int cell)
		{
			FUtility.Utils.Spawn(SquirrelConfig.ID, Grid.CellToPos(cell) with { z = Grid.GetLayerZ(Grid.SceneLayer.Move) });
			pipsSpawned++;
		}

		private void Explode(int cell)
		{
			var pos = Grid.CellToPos(cell);
			Game.Instance.SpawnFX(SpawnFXHashes.MissileExplosion, pos, 0);
			SendDebrisFlying(cell);
			PlayImpactSound(impactSound, pos);
			DamageTiles(cell);
		}

		private void PlayImpactSound(string impactSound, Vector3 position)
		{
			string sound = GlobalAssets.GetSound(impactSound);

			var cell = Grid.PosToCell(position);
			if (!Grid.IsValidCell(cell) || Grid.WorldIdx[cell] != ClusterManager.Instance.activeWorldId)
				return;

			position.z = 0.0f;

			var instance = KFMOD.BeginOneShot(sound, position, 0.75f);
			instance.setParameterByName("userVolume_SFX", KPlayerPrefs.GetFloat("Volume_SFX"));
			KFMOD.EndOneShot(instance);
		}

		private void DamageTiles(int impactCell)
		{
			foreach (var offset in targetTiles)
			{
				var cell = Grid.OffsetCell(impactCell, offset);

				if (Grid.IsValidCell(cell))
					WorldDamage.Instance.ApplyDamage(cell, 0.33f, -1);
			}

			WorldDamage.Instance.ApplyDamage(impactCell, 1f, -1);
		}

		private void SendDebrisFlying(int cell)
		{
			var pos = Grid.CellToPos(cell);
			var nearbyPickupables = ListPool<ScenePartitionerEntry, Comet>.Allocate();
			GameScenePartitioner.Instance.GatherEntries((int)pos.x - 2, (int)pos.y - 2, 4, 4, GameScenePartitioner.Instance.pickupablesLayer, nearbyPickupables);

			foreach (var partitionerEntry in nearbyPickupables)
			{
				var gameObject = (partitionerEntry.obj as Pickupable).gameObject;

				var notSentient = gameObject.GetComponent<Navigator>() == null;

				if (notSentient)
				{
					var vec = (Vector2)(gameObject.transform.GetPosition() - pos);
					vec = vec.normalized;
					var velocity = (vec + new Vector2(0.0f, 0.55f)) * (0.5f * Random.Range(this.explosionSpeedRange.x, explosionSpeedRange.y));

					if (GameComps.Fallers.Has(gameObject))
						GameComps.Fallers.Remove(gameObject);

					if (GameComps.Gravities.Has(gameObject))
						GameComps.Gravities.Remove(gameObject);

					GameComps.Fallers.Add(gameObject, velocity);
				}
			}

			nearbyPickupables.Recycle();
		}

		private bool ValidSpawnCell(int cell) =>
			Grid.IsValidCell(cell)
			&& Grid.Element[cell].hardness < byte.MaxValue;
	}
}
