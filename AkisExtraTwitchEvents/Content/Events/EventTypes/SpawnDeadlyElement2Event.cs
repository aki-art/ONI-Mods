using ONITwitchLib;
using ONITwitchLib.Utils;

namespace Twitchery.Content.Events.EventTypes
{
	public class SpawnDeadlyElement2Event() : TwitchEventBase(ID)
	{
		private static readonly CellElementEvent spawnEvent = new(
			"SpawnDeadlyElement2",
			"Spawned by Twitch",
			true
		);

		public const string ID = "SpawnDeadlyElement2";

		public override int GetWeight() => Consts.EventWeight.Uncommon;

		public override Danger GetDanger() => Danger.High;

		public override void Run()
		{
			var cellNearMouse = PosUtil.RandomCellNearMouse();
			var cell = GridUtil.FindCellWithFoundationClearance(cellNearMouse);

			var insulationElement = ElementLoader.FindElementByHash(SimHashes.SuperInsulator);
			var goop = ElementLoader.FindElementByHash(Elements.PinkSlime);

			SimMessages.ReplaceAndDisplaceElement(
			cell,
				Elements.PinkSlime,
				spawnEvent,
				100_000,
				goop.defaultValues.temperature);


			foreach (var neighborCell in GridUtil.GetNeighborsInBounds(cell))
			{
				SimMessages.ReplaceAndDisplaceElement(
					neighborCell,
					insulationElement.id,
					spawnEvent,
					float.Epsilon,
					goop.defaultValues.temperature
				);
			}
		}
	}
}
