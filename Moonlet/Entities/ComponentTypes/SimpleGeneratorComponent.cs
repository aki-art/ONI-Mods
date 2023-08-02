using UnityEngine;

namespace Moonlet.Entities.ComponentTypes
{
	public class SimpleGeneratorComponent : BaseComponent
	{
		public string Input { get; set; }

		public float InputMassRate { get; set; }

		public float? MaxStoredInputKg { get; set; }

		public string Output { get; set; }

		public float OutputMassRate { get; set; }

		public bool IgnoreBatteryRefillPercent { get; set; }

		public bool StoreOutputMass { get; set; }

		public Vector2I OutputOffset { get; set; }

		public float MinOutputTemperature { get; set; }

		public override void Apply(GameObject prefab)
		{
			var maxStoredInputKg = MaxStoredInputKg ?? InputMassRate * 20f;

			var storage = prefab.AddComponent<Storage>();

			MiscUtil.ParseElementEntry(Input, out var inputId);
			MiscUtil.ParseElementEntry(Output, out var outputId);

			var conduitConsumer = prefab.AddOrGet<ConduitConsumer>();
			conduitConsumer.conduitType = ConduitType.Gas;
			conduitConsumer.consumptionRate = 1f;
			conduitConsumer.capacityTag = inputId.CreateTag();
			conduitConsumer.capacityKG = maxStoredInputKg;
			conduitConsumer.forceAlwaysSatisfied = true;
			conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
			conduitConsumer.storage = storage;

			var energyGenerator = prefab.AddOrGet<EnergyGenerator>();


			energyGenerator.formula = EnergyGenerator.CreateSimpleFormula(
				inputId.CreateTag(),
				InputMassRate,
				maxStoredInputKg,
				outputId,
				OutputMassRate,
				StoreOutputMass,
				OutputOffset == null ? CellOffset.none : new CellOffset(OutputOffset.X, OutputOffset.Y),
				MinOutputTemperature);

			energyGenerator.powerDistributionOrder = 8;
			energyGenerator.ignoreBatteryRefillPercent = IgnoreBatteryRefillPercent;
			energyGenerator.meterOffset = Meter.Offset.Behind;
			energyGenerator.storage = storage;

			Tinkerable.MakePowerTinkerable(prefab);
			prefab.AddOrGetDef<PoweredActiveController.Def>();

			prefab.AddTag(MTags.ExcludeFromSliderScreen);
		}
	}
}
