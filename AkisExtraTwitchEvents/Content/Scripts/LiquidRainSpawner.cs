using FUtility;
using ProcGen;
using System.Collections.Generic;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class LiquidRainSpawner : KMonoBehaviour, ISim200ms
	{
		[SerializeField] public (float, float) totalAmountRangeKg;
		[SerializeField] public float spawnRadius;
		[SerializeField] public float dropletMassKg;
		[SerializeField] public float durationInSeconds;

		public float TIMEOUT = 600;

		public float elapsedTime;

		public int density;

		private float totalMassToBeSpawnedKg;
		private float spawnedMass;

		private bool raining;
		private int originCell;

		private List<SpawnData> spawnables = [];


		public struct SpawnData : IWeighted
		{
			public ushort elementId;
			public float massMultiplier;
			public float weight { get; set; }
			public float temperature;
			public float spaceTemperature;
		}

		public LiquidRainSpawner AddElement(SimHashes elementId, float weight = 1f, float temperatureOverride = -1, float massMultiplier = 1f)
		{
			var element = ElementLoader.FindElementByHash(elementId);

			if (element == null)
				return this;

			var temperature = temperatureOverride != -1
				? temperatureOverride
				: element.defaultValues.temperature;


			var spaceTemperature = temperatureOverride != -1
				? temperatureOverride
				: element.lowTemp - 5f;

			spawnables.Add(new SpawnData
			{
				elementId = element.idx,
				massMultiplier = massMultiplier,
				temperature = temperature,
				spaceTemperature = spaceTemperature,
				weight = weight
			});

			return this;
		}

		public override void OnSpawn()
		{
			base.OnSpawn();

			totalMassToBeSpawnedKg = Random.Range(totalAmountRangeKg.Item1, totalAmountRangeKg.Item2);
			// element = ElementLoader.FindElementByHash(elementId);
			var totalDropletCount = totalMassToBeSpawnedKg / dropletMassKg;
			density = (int)(totalDropletCount / durationInSeconds);
		}

		public void StartRaining()
		{
			transform.position = ONITwitchLib.Utils.PosUtil.ClampedMouseCellWorldPos();
			originCell = Grid.PosToCell(this);
			raining = true;
		}

		public void Sim200ms(float dt)
		{
			if (!raining)
			{
				return;
			}

			elapsedTime += dt;
			if (elapsedTime > TIMEOUT)
			{
				Util.KDestroyGameObject(gameObject);
				return;
			}

			for (int i = 0; i < density; i++)
			{
				//var cell = ONITwitchLib.Utils.PosUtil.ClampedMousePosWithRange(spawnRadius);
				var pos = Random.insideUnitCircle * spawnRadius;
				var cell = Grid.OffsetCell(originCell, Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));

				if (!Grid.IsValidCellInWorld(cell, this.GetMyWorldId()) || Grid.Solid[cell])
					continue;

				var spawn = spawnables.GetWeightedRandom();

				var temperature = World.Instance.zoneRenderData.GetSubWorldZoneType(cell) == ProcGen.SubWorld.ZoneType.Space
					? spawn.spaceTemperature
					: spawn.temperature;

				FallingWater.instance.AddParticle(
					cell,
					spawn.elementId,
					dropletMassKg * spawn.massMultiplier,
					temperature,
					byte.MaxValue,
					0);

				spawnedMass += dropletMassKg;

				if (spawnedMass > totalMassToBeSpawnedKg)
				{
					Util.KDestroyGameObject(gameObject);
					return;
				}
			}
		}
	}
}
