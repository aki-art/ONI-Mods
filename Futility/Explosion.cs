using System;
using System.Collections.Generic;
using UnityEngine;
using static MathUtil;

namespace FUtility
{
	public class Explosion
	{
		public string damageSource;
		public string damagePop;
		public float totalTileDamage = 0.2f;
		public float totalEntityDamage = 1f;
		public int radius = 1;
		public int debrisFlyRange = 3;
		public string impactSound = "Meteor_Large_Impact";
		public SpawnFXHashes impactFx = SpawnFXHashes.MissileExplosion;
		public MinMax debrisSpeed = new MinMax(8f, 14f);

		private Vector3 position;

		private List<int> cells;

		public void Explode(Vector3 position)
		{
			this.position = position;

			PlayImpactSound(impactSound);
			Game.Instance.SpawnFX(impactFx, position, 0);

			Utils.GetAllVisibleCells(Grid.PosToCell(position), radius, 0, cells, DoesExplode);
			SendDebrisFlying();
		}

		private bool DoesExplode(int x, int y) => true;


		private void SendDebrisFlying()
		{
			var nearbyPickupables = ListPool<ScenePartitionerEntry, Comet>.Allocate();

			GameScenePartitioner.Instance.GatherEntries(
				(int)position.x - debrisFlyRange,
				(int)position.y - debrisFlyRange,
				debrisFlyRange * 2,
				debrisFlyRange * 2,
				GameScenePartitioner.Instance.pickupablesLayer,
				nearbyPickupables);

			foreach (var partitionerEntry in nearbyPickupables)
			{
				var gameObject = (partitionerEntry.obj as Pickupable).gameObject;

				var notSentient = gameObject.GetComponent<Navigator>() == null;

				if (notSentient)
				{
					var vec = (Vector2)(gameObject.transform.GetPosition() - position);
					vec = vec.normalized;

					var speed = debrisSpeed.Get();

					var velocity = (vec + new Vector2(0.0f, 0.55f)) * (0.5f * speed);

					if (GameComps.Fallers.Has(gameObject))
						GameComps.Fallers.Remove(gameObject);

					if (GameComps.Gravities.Has(gameObject))
						GameComps.Gravities.Remove(gameObject);

					GameComps.Fallers.Add(gameObject, velocity);
				}
			}

			nearbyPickupables.Recycle();
		}

		private void PlayImpactSound(string impactSound)
		{
			string sound = GlobalAssets.GetSound(impactSound);

			var cell = Grid.PosToCell(position);
			if (!Grid.IsValidCell(cell) || Grid.WorldIdx[cell] != ClusterManager.Instance.activeWorldId)
				return;

			position.z = 0.0f;

			var instance = KFMOD.BeginOneShot(sound, position, 1f);
			instance.setParameterByName("userVolume_SFX", KPlayerPrefs.GetFloat("Volume_SFX"));
			KFMOD.EndOneShot(instance);
		}
	}
}
