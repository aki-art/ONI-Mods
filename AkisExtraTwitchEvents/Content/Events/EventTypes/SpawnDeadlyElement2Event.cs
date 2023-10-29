using ONITwitchLib.Utils;

namespace Twitchery.Content.Events.EventTypes
{
    internal class SpawnDeadlyElement2Event : ITwitchEvent
    {
        private static readonly CellElementEvent spawnEvent = new(
            "SpawnDeadlyElement2",
            "Spawned by Twitch",
            true
        );

        public bool Condition(object data) => true;

        public string GetID() => "SpawnDeadlyElement2";

        public int GetWeight() => TwitchEvents.Weights.COMMON;

        public void Run(object data)
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
