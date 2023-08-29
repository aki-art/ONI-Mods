using UnityEngine;
using static STRINGS.BUILDINGS.PREFABS.EXTERIORWALL.FACADES;

namespace Twitchery.Content.Scripts
{
	public class LiquidRainSpawner : KMonoBehaviour, ISim200ms
	{
		[SerializeField] public SimHashes elementId;
		[SerializeField] public (float, float) totalAmountRangeKg;
		[SerializeField] public float spawnRadius;
		[SerializeField] public float dropletMassKg;
		[SerializeField] public float durationInSeconds;
		[SerializeField] private float temperature;
		[SerializeField] private bool overrideTemperature;

		public float TIMEOUT = 600;

		public float elapsedTime;

		public int density;

		private float totalMassToBeSpawnedKg;
		private float spawnedMass;
		private Element element;
		private bool raining;
		private int originCell;

		public void SetTemperature(float celsius)
		{
			temperature = GameUtil.GetTemperatureConvertedToKelvin(celsius, GameUtil.TemperatureUnit.Celsius);
			overrideTemperature = true;
		}

		public override void OnSpawn()
		{
			base.OnSpawn();

			totalMassToBeSpawnedKg = Random.Range(totalAmountRangeKg.Item1, totalAmountRangeKg.Item2);
			element = ElementLoader.FindElementByHash(elementId);
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


				float temp;

				if (World.Instance.zoneRenderData.GetSubWorldZoneType(cell) == ProcGen.SubWorld.ZoneType.Space)
					temp = element.lowTemp - 5;
				else
					temp = overrideTemperature ? temperature : element.defaultValues.temperature;

				FallingWater.instance.AddParticle(
				cell,
				element.idx,
				dropletMassKg,
				temp,
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
