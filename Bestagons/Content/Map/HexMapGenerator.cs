using Bestagons.Content.ModDb;
using Bestagons.Content.Scripts;
using HarmonyLib;
using Klei;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Bestagons.Content.Map
{
    public class HexMapGenerator
    {
        private HexMapData hexMapData;
        private List<HexTile> availableHexes;

        public HexMapGenerator(ref Dictionary<HexCoord, PurchasableHex> hexes, HexCoord hexCoord, string yamlId, Bestagons_HexagonGrid grid, WorldContainer world)
        {
            Log.Debug("Generating hex layout...");
            var path = Path.Combine(Utils.ModPath, "rings", yamlId + ".yaml");

            if (!File.Exists(path))
            {
                Log.Warning($"No rings for {yamlId}");
                return;
            }

            var errors = ListPool<YamlIO.Error, HexTiles>.Allocate();

            var yaml = Mod.TryRead(path);
            hexMapData = YamlIO.LoadFile<HexMapData>(path, (error, _) => errors.Add(error));

            Global.Instance.modManager.HandleErrors(errors);
            errors.Recycle();

            var centerPos = new Vector2(
                world.Width * hexMapData.StartHorizontal, 
                world.Height * hexMapData.StartVertical);

            var center = grid.GetHexAtPos(centerPos);

            if (hexMapData != null)
                GenerateRings(ref hexes, center);
        }

        private void GenerateRings(ref Dictionary<HexCoord, PurchasableHex> hexes, HexCoord center)
        {
            Log.Debug("hexMapData.GlobalForbiddenTags " + hexMapData.GlobalForbiddenTags.Join());
            availableHexes = new List<HexTile>(BDb.hexTiles.resources)
                .Where(hex => !hex.tags.Any(hexMapData.GlobalForbiddenTags.Contains))
                .ToList();

            Log.Debug("tags of hexes:");
            foreach(var h in availableHexes)
            {
                Log.Debug(h.Id + " - " + h.tags.Join());
            }

            if (!hexes.ContainsKey(center))
            {
                Log.Warning("Invalid center");
                center = hexes.Keys.GetRandom();
            }

            if (hexMapData == null)
            {
                Log.Warning("Hexmapdata is null");
                return;
            }

            // set start tile
            Log.Info("Placing start tile");
            hexes[center].Purchase(hexMapData.StartTile);

            var i = 1;
            while (GenerateRing(ref hexes, i++, center)) { }
        }

        bool GenerateRing(ref Dictionary<HexCoord, PurchasableHex> hexes, int radius, HexCoord center)
        {
            Log.AssertNotNull("hexes", hexes);
            Log.AssertNotNull("center", center);
            Log.Info($"Generating ring {radius}...");

            if (availableHexes.Count == 0)
            {
                Log.Warning("Ran out of hexes to place.");
                return false;
            }

            var ringConfig = GetRingConfig(radius);

            Log.AssertNotNull("ringConfig", ringConfig);

            var ringHexes = new List<HexTile>(availableHexes);
            ringHexes.RemoveAll(hex => hex.tags != null && hex.tags.Any(ringConfig.ForbiddenTags.Contains));

            var requiredTags = ringConfig.RequiredTags;
            requiredTags?.Shuffle();

            var rings = center.Ring(radius);

            if (rings == null || rings.Count == 0)
                return false;

            var atLeastOneGenerated = false;

            Log.Debug("before foreach");

            foreach (var ring in rings)
            {
                HexTile tile = null;

                Log.AssertNotNull("hexes", hexes);
                if (hexes.TryGetValue(ring, out var hex))
                {
                    Log.AssertNotNull("hex", hex);
                    if (requiredTags != null && requiredTags.Count > 0)
                    {
                        var tag = requiredTags[0];
                        Log.AssertNotNull("tag", tag);
                        Log.AssertNotNull("ringHexes", ringHexes);
                        tile = FindHexWithTag(tag, ringHexes);
                        requiredTags.RemoveAt(0);
                    }

                    if (tile == null && ringHexes.Count > 0)
                    {
                        tile = ringHexes.GetRandom();
                    }

                    if (tile == null)
                    {
                        Log.Warning("no tile selected");
                        continue;
                    }
                    else
                    {
                        if (tile.HasTag(HexTags.UNIQUE))
                            ringHexes.Remove(tile);

                        hex.Purchase(tile.Id);
                    }

                    atLeastOneGenerated = true;
                }
            }

            if (requiredTags != null && requiredTags.Count > 0)
            {
                Log.Warning("Could not place all required tags: " + requiredTags.Join());
            }

            return atLeastOneGenerated;
        }


        private HexTile FindHexWithTag(string tag, List<HexTile> ringHexes)
        {
            foreach (var hex in ringHexes)
            {
                if (hex.HasTag(tag))
                    return hex;
            }

            Log.Warning($"Requested hex with tag {tag}, but found none.");
            return null;
        }

        // returns the ring tag options for the given radius
        // if not defined, the highest configured ring applies
        private HexMapData.RingData GetRingConfig(int radius)
        {
            var rings = hexMapData.Rings;
            var index = Mathf.Min(radius, rings.Length - 1);

            return rings[index];
        }
    }
}
