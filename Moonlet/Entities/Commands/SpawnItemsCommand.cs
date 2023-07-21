using FUtility;
using ProcGen;
using System.Collections.Generic;
using UnityEngine;
using YamlDotNet.Serialization;

namespace Moonlet.Entities.Commands
{
	public class SpawnItemsCommand : BaseCommand
	{
		public const int MAX_ITEMS_SPAWNED = 200;

		public bool KeepTotalMass { get; set; }

		public float MassToSpawn { get; set; }

		public string SelectionMethod { get; set; }

		public List<WeightedOption> Items { get; set; }

		public override void Run(GameObject go)
		{
			if (Items == null || Items.Count == 0)
			{
				Log.Warning($"{go.name} has defined a SpawnItems command, with no items.");
				return;
			}

			var totalMassSpawned = 0f;
			var itemsSpawned = 0;

			if (KeepTotalMass)
				MassToSpawn = go.GetComponent<PrimaryElement>().Mass;

			while (totalMassSpawned < MassToSpawn && itemsSpawned++ < MAX_ITEMS_SPAWNED)
			{
				var nextItem = Items.GetWeightedRandom();
				var item = Utils.Spawn(nextItem.Name, go.transform.position + (Vector3)nextItem.Offset);

				if (item == null)
					continue;

				var primaryElement = item.GetComponent<PrimaryElement>();
				var mass = nextItem.Mass ?? primaryElement.Mass;
				var temperature = nextItem.Temperature ?? primaryElement.Temperature;

				primaryElement.Mass = mass;
				primaryElement.Temperature = temperature;

				if (nextItem.ThrowForce.HasValue)
					Utils.YeetRandomly(
						item,
						true,
						nextItem.ThrowForce.Value * 0.5f,
						nextItem.ThrowForce.Value,
						true);

				totalMassSpawned += mass;
			}
		}

		public class SelectionMethods
		{
			public const string
				WEIGHTED = "WeightedRandom",
				RANDOM = "Random",
				CHOOSEONE = "ChooseOne",
				ONEOFEACH = "OneOfEach";
		}

		public class WeightedOption : IWeighted
		{
			[YamlMember(Alias = "weight")]
			public float weight { get; set; }

			public float? Mass { get; set; }

			public float? Temperature { get; set; }

			public string Name { get; set; }

			[YamlIgnore]
			public Vector2 Offset { get; set; }

			public string SelectionMethod { get; set; }

			public float? ThrowForce { get; set; }

			public WeightedOption()
			{
				SelectionMethod = SelectionMethods.WEIGHTED;
				weight = 1f;
				Offset = Vector2.zero;
			}
		}
	}
}
