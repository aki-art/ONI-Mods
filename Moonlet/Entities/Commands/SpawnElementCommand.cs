using FUtility;
using ProcGen;
using System.Collections.Generic;
using UnityEngine;
using YamlDotNet.Serialization;

namespace Moonlet.Entities.Commands
{
	public class SpawnElementCommand : BaseCommand
	{
		public const int MAX_ITEMS_SPAWNED = 200;

		public static CellElementEvent spawnEvent = new CellElementEvent("Tundra_SpawnElementCommand", "Command spawn", false);

		public float? Mass { get; set; }

		public float? Temperature { get; set; }

		public string Disease { get; set; }

		public int DiseaseCount { get; set; }

		public string SelectionMethod { get; set; }

		public List<WeightedOption> Items { get; set; }

		public override void Run(GameObject go)
		{
			if (Items == null || Items.Count == 0)
			{
				Log.Warning($"{go.name} has defined a SpawnElement command, with no items.");
				return;
			}

			var nextItem = Items.GetWeightedRandom();
			var element = ElementLoader.FindElementByName(nextItem.Name);

			if (element == null)
				return;

			var primaryElement = go.GetComponent<PrimaryElement>();
			var mass = nextItem.Mass ?? primaryElement.Mass;
			var temperature = nextItem.Temperature ?? primaryElement.Temperature;


			SpawnElement(go.transform.position, element, mass, temperature);
		}

		private static void SpawnElement(Vector3 position, Element element, float mass, float temperature)
		{
			if (element.IsLiquid)
			{
				BubbleManager.instance.SpawnBubble(position, Vector3.zero, element.id, mass, temperature);
			}
			else
			{
				SimMessages.ReplaceAndDisplaceElement(Grid.PosToCell(position), element.id, spawnEvent, mass, temperature);
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

			public WeightedOption()
			{
				SelectionMethod = SelectionMethods.WEIGHTED;
				weight = 1f;
				Offset = Vector2.zero;
			}
		}
	}
}
