using System.Collections.Generic;
using Twitchery.Utils;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class MagicalFlox : KMonoBehaviour, ISim33ms
	{
		private static readonly float minTemp = GameUtil.GetTemperatureConvertedToKelvin(-40, GameUtil.TemperatureUnit.Celsius);
		[SerializeField] public float temperatureShift;
		[SerializeField] public float radius;
		[SerializeField] public int cellsPerUpdate;

		private HashSet<int> cells;

		public override void OnSpawn()
		{
			base.OnSpawn();
			cells = [];
			var effect = Instantiate(ModAssets.Prefabs.freezeGunFx);
			effect.transform.position = (transform.position + new Vector3(0, 0.5f, 0f)) with { z = Grid.GetLayerZ(Grid.SceneLayer.BuildingBack) };
			effect.transform.parent = transform.parent;
			effect.SetActive(true);

			GetComponent<KBatchedAnimController>().animScale *= 1.2f;
		}

		public void Sim33ms(float dt)
		{
			var cells = GetTilesInRadius(transform.position, radius);

			var worldIdx = this.GetMyWorldId();

			foreach (var cell in cells)
			{
				if (Grid.IsValidCellInWorld(cell, worldIdx))
				{
					UpdateCell(cell, dt);
				}
			}
		}

		private HashSet<int> GetTilesInRadius(Vector3 position, float radius)
		{
			cells.Clear();
			for (int i = 0; i < cellsPerUpdate; i++)
			{
				var offset = position + (Vector3)(UnityEngine.Random.insideUnitCircle * radius);
				cells.Add(Grid.PosToCell(offset));
			}

			return cells;
		}

		private void UpdateCell(int cell, float dt)
		{
			var element = Grid.Element[cell];

			if (element.id == SimHashes.Unobtanium || element.IsVacuum)
				return;

			var temp = Grid.Temperature[cell];
			if (temp < minTemp)
				return;

			temp += (temperatureShift * dt);
			temp = Mathf.Clamp(temp, 0, 9999);

			SimMessages.ReplaceElement(
				cell,
				Grid.Element[cell].id,
				AGridUtil.cellEvent,
				Grid.Mass[cell],
				temp,
				Grid.DiseaseIdx[cell],
				Grid.DiseaseCount[cell]);
		}
	}
}
