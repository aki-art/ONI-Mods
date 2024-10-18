using Moonlet.Templates;

namespace Moonlet.Scripts.Commands
{
	public class SpawnItemsCommand : BaseCommand
	{
		public class SpawnItemsData
		{
			public SpawnedItemEntry[] Items { get; set; }

			public IntNumber MinCount { get; set; }

			public IntNumber MaxCount { get; set; }

			public bool? AllowDuplicateRolls { get; set; }
		}

		public class SpawnedItemEntry
		{
			public string Tag { get; set; }

			public FloatNumber MinAmount { get; set; }
			public FloatNumber MaxAmount { get; set; }

			public TemperatureNumber Temperature { get; set; }

			public string Unit { get; set; }

			public string Disease { get; set; }

			public IntNumber DisaseCount { get; set; }

			public FloatNumber Weight { get; set; }
		}

		public override void Run(object data)
		{
			throw new System.NotImplementedException();
		}
	}
}
