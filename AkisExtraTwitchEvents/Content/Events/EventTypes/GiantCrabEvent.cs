using FUtility;
using System;
using System.Collections.Generic;
using Twitchery.Content.Defs.Critters;
using Twitchery.Content.Scripts;

namespace Twitchery.Content.Events.EventTypes
{
    internal class GiantCrabEvent : ITwitchEvent
    {
        public const string ID = "GiantCrab";
        private OccupyArea prefabOccupyArea;
        private static readonly CellOffset[] smallerArea = EntityTemplates.GenerateOffsets(3, 5);

        public int GetWeight() => TwitchEvents.Weights.COMMON;

        public bool Condition(object data) => Mod.giantCrabs.Count == 0;

        public string GetID() => ID;

        // TODO: could probably be merged with Radish logic instead of copy paste
        public void Run(object data)
        {
            prefabOccupyArea = Assets.GetPrefab(GiantCrabConfig.ID).GetComponent<OccupyArea>();
            var targetWorld = ClusterManager.Instance.activeWorld;

            var cavities = new List<CavityInfo>();
            foreach (CavityInfo cavity in Game.Instance.roomProber.cavityInfos)
            {
                var middle = Grid.PosToCell(cavity.GetCenter());

                if (!Grid.IsVisible(middle) || !Grid.IsValidCellInWorld(middle, targetWorld.id))
                    continue;

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
                    continue;

                var height = cavity.maxY - cavity.minY;

                if (height < 4)
                    continue;

                if (height < 7)
                {
                    // save for later in case we dont find a really good spot
                    potentialButLameCavities.Add(cavity);
                    continue;
                }

                var cell = GetValidPlacementInCavity(cavity);

                if (cell == -1)
                    continue;

                SpawnCrab(cell, targetWorld);

                return;
            }

            if (potentialButLameCavities.Count > 0)
            {
                foreach (var cavity in potentialButLameCavities)
                {
                    var lameCell = GetSortofValidPlacementInCavity(cavity);
                    if (lameCell != -1)
                    {
                        SpawnCrab(lameCell, targetWorld);
                        return;
                    }
                }
            }
        }

        private static void SpawnCrab(int cell, WorldContainer world)
        {
            var entity = FUtility.Utils.Spawn(GiantCrabConfig.ID, Grid.CellToPos(cell));

            ONITwitchLib.ToastManager.InstantiateToastWithGoTarget(
                "",
                STRINGS.AETE_EVENTS.GIANT_CRAB.DESC,
                entity);
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

            if (maxY < minY) return -1;

            for (var x = minX; x <= maxX; x++)
            {
                for (var y = maxY; y >= minY; y--)
                {
                    var cell = Grid.XYToCell(x, y);
                    bool invalid = false;

                    foreach (CellOffset offset in smallerArea)
                    {
                        var offsetCell = Grid.OffsetCell(cell, offset);
                        if (!Grid.IsValidCell(offsetCell)
                            || Grid.Solid[offsetCell]
                            || Grid.Objects[offsetCell, (int)ObjectLayer.Building] != null)
                        {
                            invalid = true;
                            break;
                        }
                    }

                    if (!invalid)
                        return cell;
                }
            }

            return -1;
        }
    }
}
