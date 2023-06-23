using FUtility;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class PrefabRainSpawner : KMonoBehaviour, ISim200ms
	{
		[SerializeField] public Tag prefabTag;
		[SerializeField] public (float, float) totalAmountRangeKg;
		[SerializeField] public float spawnRadius;
		[SerializeField] private float dropletMassKg;
		[SerializeField] private bool overrideMass;
		[SerializeField] public float durationInSeconds;
		[SerializeField] public SpawnFXHashes spawnFXHashes;
		[SerializeField] private float temperature;
		[SerializeField] private bool overrideTemperature;
		[SerializeField] public Grid.SceneLayer sceneLayer = Grid.SceneLayer.Creatures;

		public float TIMEOUT = 600;

		public float elapsedTime;

		public int density;

		private float totalMassToBeSpawnedKg;
		private float spawnedMass;
		private GameObject prefab;
		private bool raining;
		private int originCell;

		public void SetTemperature(float celsius)
		{
			temperature = GameUtil.GetTemperatureConvertedToKelvin(celsius, GameUtil.TemperatureUnit.Celsius);
			overrideTemperature = true;
		}

		public void SetMass(float mass)
		{
			dropletMassKg = mass;
			overrideMass = true;
		}

		public override void OnSpawn()
		{
			base.OnSpawn();

			totalMassToBeSpawnedKg = Random.Range(totalAmountRangeKg.Item1, totalAmountRangeKg.Item2);

			var totalDropletCount = totalMassToBeSpawnedKg / dropletMassKg;
			density = (int)(totalDropletCount / durationInSeconds);
		}

		public void StartRaining()
		{
			prefab = Assets.GetPrefab(prefabTag);
			transform.position = ONITwitchLib.Utils.PosUtil.ClampedMouseCellWorldPos();
			originCell = Grid.PosToCell(this);
			raining = true;
		}

		public void Sim200ms(float dt)
		{
			if (!raining)
				return;

			elapsedTime += dt;

			if (elapsedTime > TIMEOUT)
			{
				Util.KDestroyGameObject(gameObject);
				return;
			}

			for (int i = 0; i < density; i++)
			{
				var pos = Random.insideUnitCircle * spawnRadius;
				var cell = Grid.OffsetCell(originCell, Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));

				if (!Grid.IsValidCellInWorld(cell, this.GetMyWorldId()) || Grid.Solid[cell])
					continue;

				var go = GameUtil.KInstantiate(prefab, Grid.CellToPos(cell), sceneLayer);

				if (go.TryGetComponent(out PrimaryElement primaryElement))
				{
					if (overrideMass)
						primaryElement.Mass = dropletMassKg;

					if(overrideTemperature)
						primaryElement.Temperature = temperature;

					spawnedMass += primaryElement.Mass;
				}
				else
				{
					spawnedMass += 1f;
				}

				go.SetActive(true);

				Log.Debuglog($"mass: {spawnedMass}/{totalMassToBeSpawnedKg}");

				if (spawnedMass > totalMassToBeSpawnedKg)
				{
					Util.KDestroyGameObject(gameObject);
					return;
				}
			}
		}
	}
}
