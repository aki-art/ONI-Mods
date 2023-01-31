using FUtility;
using System.Collections.Generic;
using System.Linq;
using Twitchery.Content.Defs;
using Twitchery.Content.Scripts;

namespace Twitchery.Content.Events
{
    public class RadDishEvent : ITwitchEvent
    {
        public const string ID = "RadDish";
        private OccupyArea prefabOccupyArea;

        public bool Condition(object data)
        {
            var minKcal = GetMinKcal();

            foreach (var world in ClusterManager.Instance.WorldContainers)
            {
                if (IsWorldEligible(world, minKcal))
                {
                    return true;
                }
            }

            return false;
        }

        private static int GetMinKcal()
        {
            return !AkisTwitchEvents.Instance.hasRaddishSpawnedBefore ? 300_000 : 100_000;
        }

        private bool IsWorldEligible(WorldContainer world, float minKcal)
        {
            if (!IsInhabited(world))
            {
                return false;
            }

            var rationPerWorld = RationTracker.Get().CountRations(null, world.worldInventory, true);
            Log.Debuglog($"checking {world.GetProperName()} {rationPerWorld}");

            return rationPerWorld <= minKcal;
        }

        private bool IsInhabited(WorldContainer world)
        {
            if (!world.IsDupeVisited || !world.isDiscovered)
            {
                return false;
            }

            var minions = Components.MinionIdentities.GetWorldItems(world.id);
            return minions != null && minions.Count != 0;
        }

        public string GetID() => ID;

        private float GetCaloriesPerDupe(WorldContainer world)
        {
            var totalRations = RationTracker.Get().CountRations(null, world.worldInventory);
            var minions = Components.LiveMinionIdentities.GetWorldItems(world.id).Count;

            return totalRations / minions;
        }

        public void Run(object data)
        {
            prefabOccupyArea = Assets.GetPrefab(GiantRadishConfig.ID).GetComponent<OccupyArea>();

            var rationTracker = RationTracker.Get();

            var worlds = new List<WorldContainer>(ClusterManager.Instance.WorldContainers)
                .Where(IsInhabited)
                .OrderBy(GetCaloriesPerDupe);

            var targetWorld = worlds.Last();

            if (targetWorld == null)
            {
                Log.Warning("something went wrong trying to find the hungriest asteroid");
                return;
            }

            var cavities = new List<CavityInfo>();
            foreach (CavityInfo cavity in Game.Instance.roomProber.cavityInfos)
            {
                var middle = Grid.PosToCell(cavity.GetCenter());

                if (!Grid.IsVisible(middle) || !Grid.IsValidCellInWorld(middle, targetWorld.id))
                {
                    continue;
                }

                cavities.Add(cavity);
            }

            if (cavities.Count == 0)
            {
                Log.Warning("No cavities in this world apparently. " + targetWorld.GetProperName());
                return;
            }

            cavities.Shuffle();

            var potentialButLameCavities = new List<CavityInfo>();

            foreach (CavityInfo cavity in cavities)
            {
                if (cavity.maxX - cavity.minX < 3)
                {
                    continue;
                }

                var height = cavity.maxY - cavity.minY;

                if (height < 4)
                {
                    continue;
                }

                if (height < 6)
                {
                    // save for later in case we dont find a really good spot
                    potentialButLameCavities.Add(cavity);
                    continue;
                }

                var cell = GetValidPlacementInCavity(cavity);

                if (cell == -1)
                {
                    continue;
                }

                SpawnRadish(cell, targetWorld);

                return;
            }

            if(potentialButLameCavities.Count > 0)
            {
                foreach(var cavity in potentialButLameCavities)
                {
                    var lameCell = GetSortofValidPlacementInCavity(cavity);
                    if (lameCell != -1)
                    {
                        SpawnRadish(lameCell, targetWorld);
                        return;
                    }
                }
            }

            ONITwitchLib.ToastManager.InstantiateToast("Rad dish...?", "But something went wrong, there was nowhere to spawn it. :(");
        }

        private static readonly CellOffset[] smallerArea = EntityTemplates.GenerateOffsets(3, 2);

        private static void SpawnRadish(int cell, WorldContainer world)
        {
            var radish = FUtility.Utils.Spawn(GiantRadishConfig.ID, Grid.CellToPos(cell));
            ONITwitchLib.ToastManager.InstantiateToastWithGoTarget(
                "Rad Dish",
                $"A singular radish has been spawned on {world?.GetProperName()}",
                radish);

            AkisTwitchEvents.Instance.hasRaddishSpawnedBefore = true;
        }

        private int GetValidPlacementInCavity(CavityInfo cavity)
        {
            var minX = cavity.minX + 1; // no need to check up against wall
            var maxX = cavity.maxX - 1;
            var minY = cavity.minY;
            var maxY = cavity.maxY - 5;

            for (var x = minX; x <= maxX; x++)
            {
                for (var y = maxY; y >= minY; y--)
                {
                    var cell = Grid.XYToCell(x, y);
                    if (prefabOccupyArea.TestArea(cell, null, (cell, _) => Grid.IsValidCell(cell) && !Grid.Solid[cell])
                        && prefabOccupyArea.CanOccupyArea(cell, ObjectLayer.Building))
                    {
                        return cell;
                    }
                }
            }

            return -1;
        }

        private int GetSortofValidPlacementInCavity(CavityInfo cavity)
        {
            var minX = cavity.minX + 1; // no need to check up against wall
            var maxX = cavity.maxX - 1;
            var minY = cavity.minY;
            var maxY = cavity.maxY - 2;

            for (var x = minX; x <= maxX; x++)
            {
                for (var y = maxY; y >= minY; y--)
                {
                    var cell = Grid.XYToCell(x, y);

                    foreach (CellOffset offset in smallerArea)
                    {
                        var offsetCell = Grid.OffsetCell(cell, offset);
                        if(!Grid.IsValidCell(offsetCell)
                            || Grid.Solid[offsetCell]
                            || Grid.Objects[offsetCell, (int)ObjectLayer.Building] != null)
                        {
                            continue;
                        }
                    }

                    return cell;
                }
            }

            return -1;
        }
    }
}
