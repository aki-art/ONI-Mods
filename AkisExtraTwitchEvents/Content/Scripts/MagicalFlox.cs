using System.Collections.Generic;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class MagicalFlox : KMonoBehaviour, ISim200ms, IGameObjectEffectDescriptor
	{
		private static readonly float minTemp = GameUtil.GetTemperatureConvertedToKelvin(-40, GameUtil.TemperatureUnit.Celsius);
		[SerializeField] public float temperatureShift;

		private static readonly List<CellOffset> cellOffsets = FUtility.Utils.MakeCellOffsetsFromMap(true,
			" X ",
			"XOX",
			" X ");

		private HashSet<int> cells;

		public override void OnSpawn()
		{
			base.OnSpawn();
			cells = [];
			var effect = Instantiate(ModAssets.Prefabs.magicalFloxFx);
			effect.transform.position = (transform.position + new Vector3(0, 0.5f, 0f)) with { z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2) };
			effect.transform.parent = transform;
			effect.SetActive(true);

			GetComponent<KBatchedAnimController>().animScale *= 1.2f;
		}

		public void Sim200ms(float dt)
		{
			var worldIdx = this.GetMyWorldId();
			var originCell = Grid.PosToCell(this);

			foreach (var offset in cellOffsets)
			{
				var cell = Grid.OffsetCell(originCell, offset);
				if (Grid.IsValidCellInWorld(cell, worldIdx))
				{
					UpdateCell(cell, dt);
				}
			}
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
			temp = Mathf.Clamp(temp, 0, 9975);

			SimMessages.ModifyEnergy(cell, -0.15f, 5000f, SimMessages.EnergySourceID.DebugCool);
			/*			SimMessages.ReplaceElement(
							cell,
							Grid.Element[cell].id,
							AGridUtil.cellEvent,
							Grid.Mass[cell],
							temp,
							Grid.DiseaseIdx[cell],
							Grid.DiseaseCount[cell]);*/
		}

		public List<Descriptor> GetDescriptors(GameObject go)
		{
			return [new Descriptor($"{FUtility.Utils.FormatAsLink("Temperature", "HEAT")}: {GameUtil.GetFormattedTemperature(temperatureShift, GameUtil.TimeSlice.PerSecond, GameUtil.TemperatureInterpretation.Relative)}", "tooltip")];
		}
	}
}
