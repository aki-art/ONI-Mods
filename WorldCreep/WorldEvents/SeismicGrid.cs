using FUtility;
using LibNoiseDotNet.Graphics.Tools.Noise;
using LibNoiseDotNet.Graphics.Tools.Noise.Filter;
using LibNoiseDotNet.Graphics.Tools.Noise.Primitive;
using ProcGen;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace WorldCreep.WorldEvents
{
    public class SeismicGrid
    {
        private const int CHUNK_EDGE = 16;

        public static float[] activity;
        //public static float[] effectorValues;
        private static HashSet<int> dirtyCells;
        private static HashSet<int> dirtyChunks;
        private static HashSet<int> protectedCells;
        public static Dictionary<int, float> chunks;
        public static Dictionary<int, float> effectorValues;
        public static float highestActivity = 0;

        private static Element neutronium;
        public static HashSet<Tag> protectedObjectIDs;
        private static RidgedMultiFractal noise;

        private static int selectedChunk = -1;
        private static int selectedCell = -1;
        private static int widthInChunks;
        private static int heightInChunks;

        public static void Initialize(int seed)
        {
            effectorValues = new Dictionary<int, float>();
            dirtyChunks = new HashSet<int>();

            neutronium = ElementLoader.FindElementByHash(SimHashes.Unobtanium);
            noise = CreateNoise(seed);
            GenerateActivityMap(noise);
            SetProtectedObjects();
            SetChunks();
            var telepad = GameUtil.GetTelepad();
            if (telepad != null)
                telepad.AddOrGet<Buildings.SeismicStabilizer>().radius = ModSettings.WorldEvents.SafeZoneRadius;
        }

        public static float GetActivity(int cell) => effectorValues.TryGetValue(cell, out float m) ? activity[cell] * m : activity[cell];

        public static bool IsValidCell(int cell) => activity.Length > cell && cell >= 0;

        private static RidgedMultiFractal CreateNoise(int seed)
        {
            SimplexPerlin perlin = new SimplexPerlin
            {
                Quality = NoiseQuality.Standard,
                Seed = seed
            };

            return new RidgedMultiFractal
            {
                Frequency = 10,
                Lacunarity = 2,
                OctaveCount = 20,
                Offset = 1,
                Gain = 0,
                Primitive3D = perlin,
                Primitive2D = perlin
            };
        }

        private static void GenerateActivityMap(RidgedMultiFractal noise)
        {
            activity = new float[Grid.CellCount];

            for (int x = 0; x < Grid.WidthInCells; x++)
            {
                for (int y = 0; y < Grid.HeightInCells; y++)
                {
                    int cell = Grid.XYToCell(x, y);
                    bool spaceZone = Game.Instance.world.zoneRenderData.GetSubWorldZoneType(cell) == SubWorld.ZoneType.Space;
                    activity[cell] = spaceZone ? 0 : noise.GetValue(x / 1000f, y / 1000f) * (Grid.HeightInCells - y) / Grid.HeightInCells;
                }
            }
        }

        public static void SetDirty(int cell) => dirtyCells.Add(cell);

        private static float GetBaseValue(int cell)
        {
            if (!IsValidCell(cell) ||
                protectedCells.Contains(cell) ||
                Game.Instance.world.zoneRenderData.GetSubWorldZoneType(cell) == SubWorld.ZoneType.Space)
                return 0;

            Grid.CellToXY(cell, out int x, out int y);
            return noise.GetValue(x / 1000f, y / 1000f) * (Grid.HeightInCells - y) / Grid.HeightInCells;
        }

        private static void SetProtectedObjects()
        {
            protectedObjectIDs = new HashSet<Tag>()
            {
                HeadquartersConfig.ID,
                MassiveHeatSinkConfig.ID,
                OilWellConfig.ID
            };

            foreach (var geyser in Assets.GetPrefabsWithComponent<Geyser>())
                protectedObjectIDs.Add(geyser.PrefabID());
        }

        public static int GetRandomCellInCircle(int center, int r, HashSet<int> cells = null)
        {
            int attempt = 0;
            Vector3 middle = (Vector3)Grid.CellToPos(center);

            while (attempt++ < 200)
            {
                int targetDistance = Mathf.FloorToInt(r * Util.Bias(Random.value, 0.4f));
                Vector3 chosenCell = (Vector3)Random.insideUnitCircle.normalized * targetDistance + middle;
                int cell = Grid.PosToCell(chosenCell);
                if (Grid.IsValidCell(cell) && (cells == null || cells.Contains(cell)))
                {
                    return cell;
                }
            }

            return -1;
        }

        private static bool CanSpawnGeyser(int cell)
        {
            bool isNeutronium = Grid.Element[cell] == neutronium;
            bool isSpace = Game.Instance.world.zoneRenderData.GetSubWorldZoneType(cell) == SubWorld.ZoneType.Space;
            bool isActiveEnough = activity[cell] >= Tuning.WorldEvent.GEYSER_TRESHOLD;

            return !isNeutronium && !isSpace && isActiveEnough;
        }

        private static bool IsProtected(Pickupable go)
        {
            return go.GetComponent<Geyser>() != null;
        }

        internal static int GetRandomCell(bool solid)
        {
            List<int> cells = new List<int>();
            var eligibleChunks = chunks.Where(c => c.Value > 0);

            foreach (var chunk in eligibleChunks)
            {
                ChunkOffset(chunk.Key, out int cx, out int cy);
                for (int x = 0; x < CHUNK_EDGE; x++)
                {
                    for (int y = 0; y < CHUNK_EDGE; y++)
                    {
                        int cell = Grid.XYToCell(cx + x, cy + y);
                        if (Grid.IsSolidCell(cell) || !solid)
                            cells.Add(cell);
                    }
                }
            }

            return cells.Count > 0 ? cells.GetRandom() : -1;
        }

        private static int GetHighMagnitudeEpicenter(float power)
        {
            var geyserLocations = Components.Pickupables.Items
                .Where(p => !IsProtected(p))
                .Select(p => p.GetCell());

            var eligibleChunks = chunks.Where(c => c.Value >= power);
            List<int> cells = new List<int>();

            foreach (var chunk in eligibleChunks)
            {
                ChunkOffset(chunk.Key, out int cx, out int cy);
                for (int x = 0; x < CHUNK_EDGE; x++)
                {
                    for (int y = 0; y < CHUNK_EDGE; y++)
                    {
                        int cell = Grid.XYToCell(cx + x, cy + y);
                        if (CanSpawnGeyser(cell))
                            cells.Add(cell);
                    }
                }
            }

            foreach (var geyser in geyserLocations)
            {
                Extents extents = new Extents(geyser, 5);
                cells.RemoveAll(c => extents.Contains(Grid.CellToXY(c)));
            }

            // TODO: Check for protected buildings too

            return cells.Count > 0 ? cells.GetRandom() : -1;

        }

        public static List<int> GetBlob(int center, float radius)
        {
            return ProcGen.Util.GetFilledCircle(Grid.CellToPos(center), radius)
                .Select(v => Grid.PosToCell(v))
                .ToList();
        }

        public static Dictionary<int, float> GetCircle(WorldEvent ev, int closeRange = 10, int falloffRange = 5)
        {
            Dictionary<int, float> cells = new Dictionary<int, float>();
            AddCellRange(cells, ev.transform.position, ev.power, closeRange, falloffRange);

            return cells;
        }

        private static void AddCellRange(Dictionary<int, float> cells, Vector3 position, float power, int innerRadius, int outerRadius)
        {
            int radius = innerRadius + outerRadius;
            var range = ProcGen.Util.GetFilledCircle(position, radius);

            foreach (Vector2I pos in range)
            {
                int cell = Grid.XYToCell(Grid.ClampX(pos.x), pos.y);
                if (Grid.IsValidBuildingCell(cell))
                    cells[cell] = GetPower(position, power, innerRadius, outerRadius, pos);
            }
        }

        private static float GetPower(Vector3 position, float power, int innerRadius, int outerRadius, Vector2I pos)
        {
            float distance = Vector2.Distance(position, pos);
            float powerMultiplier = distance > innerRadius ? 1 - (distance - innerRadius) / outerRadius : 1f;
            return power * powerMultiplier;
        }


        public static bool FindAppropiateEpicenter(float power, float highManitudeTreshold, out int cell)
        {
            bool geyserOK = true;
            if (power >= highManitudeTreshold)
            {
                selectedCell = GetHighMagnitudeEpicenter(power);
                geyserOK = selectedCell > -1;
            }
            if (power < highManitudeTreshold || selectedCell == -1)
            {
                float min = power;
                selectedChunk = FindChunk(min);
                selectedCell = FindCell(selectedChunk, min);
            }

            cell = selectedCell;
            return geyserOK && cell > -1;
        }

        public static bool IsActiveChunk(int cell)
        {
            Grid.CellToXY(cell, out int x, out int y);
            return selectedChunk == XYToChunk(x / CHUNK_EDGE, y / CHUNK_EDGE);
        }

        private static void SetChunks()
        {
            widthInChunks = Grid.WidthInCells / CHUNK_EDGE;
            heightInChunks = Grid.HeightInCells / CHUNK_EDGE;
            chunks = new Dictionary<int, float>();
            chunks = new Dictionary<int, float>();

            for (int x = 0; x < widthInChunks; x++)
            {
                for (int y = 0; y < heightInChunks; y++)
                {
                    float max = FindMax(x, y);
                    chunks[XYToChunk(x, y)] = max;
                    if (max > highestActivity) highestActivity = max;
                }
            }
        }

        private static void UpdateDirtyChunks()
        {
            foreach(int chunk in dirtyChunks)
            {
                UpdateChunk(chunk);
            }
        }

        private static void UpdateChunk(int chunk)
        {
            ChunkToXY(chunk, out int x, out int y);
            float max = FindMax(x, y);
            chunks[chunk] = max;
            highestActivity = chunks.Max(c => c.Value);
        }

        public static void RemoveNullifier(Vector2 centerPos, int r)
        {
            List<Vector2I> cells = ProcGen.Util.GetFilledCircle(centerPos, r).Distinct().ToList();
            foreach (Vector2I cell in cells)
            {
                int c = Grid.PosToCell(cell);
                if (effectorValues.ContainsKey(c))
                {
                    float dist = Vector2.Distance(centerPos, cell);
                    effectorValues[c] /= DiminishedPower(dist, r);
                    if (effectorValues[c].IsAlmost(1f))
                        effectorValues.Remove(c);
                }

                dirtyChunks.Add(CellToChunk(c));
            }

            UpdateDirtyChunks();
        }

        private static float DiminishedPower(float dist, float r) => Util.Bias(Mathf.Clamp(dist, 0, r) / (float)r, 0.3f);

        public static void AddNullifier(Vector2 centerPos, int r)
        {
            List<Vector2I> cells = ProcGen.Util.GetFilledCircle(centerPos, r).Distinct().ToList();
            foreach (Vector2I cell in cells)
            {
                int c = Grid.PosToCell(cell);
                float dist = Vector2.Distance(centerPos, cell);
                float m = DiminishedPower(dist, r);

                if (effectorValues.ContainsKey(c))
                    effectorValues[c] *= m;
                else
                    effectorValues.Add(c, m);

                dirtyChunks.Add(CellToChunk(c));
            }

            UpdateDirtyChunks();
        }

        private static int FindChunk(float treshold)
        {
            Debug.Log("Findchnk");
            Debug.Log("chunks count: " + chunks.Count);
            foreach (var chunk in chunks)
            {
                Debug.Log(chunk.Value);
            };

            var eligibleChunks = chunks.Where(c => c.Value >= treshold).ToList();
            if (eligibleChunks.Count > 0)
                return eligibleChunks.GetRandom().Key;
            else return -1;
        }

        private static int FindCell(int chunk, float min)
        {
            ChunkOffset(chunk, out int x, out int y);
            int attempt = 0;
            int maxAttempts = CHUNK_EDGE * CHUNK_EDGE;

            // start at random cell
            Vector2I cellPos = new Vector2I(
                Random.Range(0, CHUNK_EDGE - 1) + x,
                Random.Range(0, CHUNK_EDGE - 1) + y);

            // iterate through the other cells in order
            while (attempt++ < maxAttempts)
            {
                int cell = Grid.XYToCell(cellPos.x, cellPos.y);
                if (activity[cell] >= min)
                {
                    return cell;
                }

                cellPos = NextCell(cellPos, x, y);
            }

            Log.Warning($"Selected chunk {chunk}, with a maximum of {chunks[chunk]}, " +
                $"but there wasn't an appropiate cell for event center. " +
                $"(Looking for {min}). Did the map data change?");

            return -1;
        }

        private static float FindMax(int cx, int cy)
        {
            float max = 0;

            for (int x = 0; x < CHUNK_EDGE; x++)
            {
                for (int y = 0; y < CHUNK_EDGE; y++)
                {
                    float num = GetActivity(Grid.XYToCell(cx * CHUNK_EDGE + x, cy * CHUNK_EDGE + y));
                    if (num > max) max = num;
                }
            }

            return max;
        }

        public static float GetChunkMaxActivity(int cell) => chunks[CellToChunk(cell)];

        private static Vector2I NextCell(Vector2I pos, int chunkX, int chunkY)
        {
            if (pos.x < chunkX + CHUNK_EDGE - 1)
            {
                // move one to right
                return new Vector2I(pos.x + 1, pos.y);
            }

            if (pos.y < chunkY + CHUNK_EDGE - 1)
            {
                // move to next line
                return new Vector2I(chunkX, pos.y + 1);
            }

            // move to first cell
            return new Vector2I(chunkX, chunkY);
        }

        public static int CellToChunk(int cell)
        {
            Grid.CellToXY(cell, out int x, out int y);
            return XYToChunk(x / CHUNK_EDGE, y / CHUNK_EDGE);
        }

        public static int XYToChunk(int x, int y)
        {
            return x + y * widthInChunks;
        }

        public static void ChunkToXY(int chunk, out int x, out int y)
        {
            x = chunk % widthInChunks;
            y = chunk / widthInChunks;
        }

        public static void ChunkOffset(int chunk, out int x, out int y)
        {
            x = (chunk % widthInChunks) * CHUNK_EDGE;
            y = (chunk / widthInChunks) * CHUNK_EDGE;
        }
    }
}