using ONITwitchLib;
using ProcGen;
using System.Collections.Generic;
using Twitchery.Content.Scripts;
using Twitchery.Utils;

namespace Twitchery.Content.Events.EventTypes.PandoraSubEvents
{
	public class PandoraDangerousLiquidFloodEvent : PandoraEventBase
	{
		public override Danger GetDanger() => Danger.Deadly;

		public override void Run(PandorasBox box)
		{
			base.Run(box);

			if (box == null)
				return;

			var elementOption = MiscUtil.dangerousLiquids.GetWeightedRandom();
			var element = ElementLoader.FindElementByTag(elementOption.id);

			var cell = box.NaturalBuildingCell();

			Flood(
				cell,
				element,
				elementOption.temperature == -1 ? element.defaultValues.temperature : elementOption.temperature,
				elementOption.mass == -1 ? element.defaultValues.mass : elementOption.mass,
				5,
				Grid.WorldIdx[cell]);

			End();
		}

		public PandoraDangerousLiquidFloodEvent(float weight) : base(weight)
		{
		}

		private void Flood(int cell, Element element, float temperature, float mass, int tiles, int world)
		{
			var valid_cells = new List<int>();
			var visited_cells = HashSetPool<int, PandorasBox>.Allocate();
			var queue = QueuePool<GameUtil.FloodFillInfo, PandorasBox>.Allocate();

			queue.Enqueue(new GameUtil.FloodFillInfo()
			{
				cell = cell,
				depth = 0
			});

			bool condition(int cell2) => Grid.IsValidCellInWorld(cell2, world) && !Grid.Solid[cell2];

			GameUtil.FloodFillConditional(queue, condition, visited_cells, valid_cells, tiles);

			foreach (var gameCell in valid_cells)
			{
				if (tiles > 0)
				{
					SimMessages.AddRemoveSubstance(gameCell, element.id, CellEventLogger.Instance.ElementEmitted, mass, temperature, byte.MaxValue, 0);
					--tiles;
				}
				else
					break;
			}
		}

		public struct ElementOption(string id, float temperature = -1, float mass = -1, float weight = 1.0f) : IWeighted
		{
			public float weight { get; set; } = weight;
			public string id = id;
			public float temperature = temperature;
			public float mass = mass;

			public ElementOption(SimHashes id, float temperature = -1, float mass = -1, float weight = 1.0f) : this(id.ToString(), temperature, mass, weight)
			{
			}
		}
	}
}
