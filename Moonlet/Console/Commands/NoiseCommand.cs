using Klei;
using LibNoiseDotNet.Graphics.Tools.Noise.Builder;
using ProcGen;
using ProcGenGame;
using System.Collections.Generic;
using UnityEngine;

namespace Moonlet.Console.Commands
{
	public class NoiseCommand() : BaseConsoleCommand("noise")
	{
		public static SimHashes border = SimHashes.Unobtanium;
		public static List<ElementBandConfiguration> biomeElements;

		private const string DEFAULT_NOISE = "worldgen/test_noise";
		private const string DEFAULT_BIOME = "worldgen/test_biome";
		public override void SetupArguments()
		{
			expectedArgumentVariations = [[
				new StringArgument("noise", "noise path", DEFAULT_NOISE, true),
				new StringArgument("biome", "biome path", DEFAULT_BIOME, true),
				new IntArgument("width", "width of area from selected cell", 100, true),
				new IntArgument("height", "height of area from selected cell", 100, true)
				]];
		}

		public override CommandResult Run()
		{
			Log.Debug("run");
			var cell = SelectTool.Instance.selectedCell;

			if (cell == -1)
				return CommandResult.Error("No cell selected");

			if (!Grid.IsValidCell(cell))
				return CommandResult.Error("Invalid cell selection");

			Log.Debug("cell ok");
			GetStringArgument(1, out var noise);
			GetStringArgument(2, out var biome);
			GetIntArgument(3, out var width);
			GetIntArgument(4, out var height);

			Log.Debug($"arguments: {noise} {biome} {width} {height}");
			var worldIdx = Grid.WorldIdx[cell];

			var gradientsResult = LoadElementGradients(biome, out var biomeSettings);

			Log.Debug("LoadElementGradients ok");
			if (!gradientsResult.IsSuccess())
				return gradientsResult;

			var noiseResult = LoadNoise(noise, width, height, out var noiseMap);

			Log.Debug("LoadNoise ok");
			if (!noiseResult.IsSuccess())
				return noiseResult;

			GenerateTerrain(biomeSettings, cell, width, height, noiseMap, worldIdx);
			Log.Debug("GenerateTerrain ok");
			DrawBorder(cell, width, height);
			Log.Debug("DrawBorder ok");

			return CommandResult.Success();
		}

		private CommandResult LoadNoise(string noiseId, int width, int height, out NoiseMap noiseMap)
		{
			ProcGen.Noise.Tree tree = null;
			noiseMap = null;

			if (noiseId == DEFAULT_NOISE)
			{
				var path = System.IO.Path.Combine(FUtility.Utils.ModPath, $"{DEFAULT_NOISE}.yaml");
				tree = YamlIO.LoadFile<ProcGen.Noise.Tree>(path);
			}
			else
				tree = SettingsCache.noise.GetTree(noiseId);

			if (tree == null)
				return CommandResult.Error("Noise does not exist");

			var plane = BuildNoiseSource(width, height, tree);
			noiseMap = WorldGen.BuildNoiseMap(Vector3.zero, tree.settings.zoom, plane, width, height, null);

			return noiseMap == null
				? CommandResult.Error("Failed to load noise map.")
				: CommandResult.Success();
		}

		public NoiseMapBuilderPlane BuildNoiseSource(int width, int height, ProcGen.Noise.Tree tree)
		{
			var lowerBound = tree.settings.lowerBound;
			var upperBound = tree.settings.upperBound;

			var noiseMapBuilderPlane = new NoiseMapBuilderPlane(lowerBound.x, upperBound.x, lowerBound.y, upperBound.y, false);
			noiseMapBuilderPlane.SetSize(width, height);
			noiseMapBuilderPlane.SourceModule = tree.BuildFinalModule(0);

			return noiseMapBuilderPlane;
		}


		private CommandResult LoadElementGradients(string biomeId, out BiomeSettings biomeSettings)
		{
			biomeSettings = null;

			if (biomeId == DEFAULT_BIOME)
			{
				var path = System.IO.Path.Combine(FUtility.Utils.ModPath, $"{biomeId}.yaml");
				biomeSettings = YamlIO.LoadFile<BiomeSettings>(path);
			}
			else if (!SettingsCache.biomeSettingsCache.TryGetValue(biomeId, out biomeSettings))
				return CommandResult.Error("Biome does not exist.");

			foreach (var config in biomeSettings.TerrainBiomeLookupTable)
				config.Value.ConvertBandSizeToMaxSize();

			return CommandResult.Success();
		}


		private void GenerateTerrain(BiomeSettings biomeSettings, int bottomLeftCell, int width, int height, NoiseMap noiseMap, int worldIdx)
		{
			var table = biomeSettings.TerrainBiomeLookupTable["Default"];
			Grid.CellToXY(bottomLeftCell, out var offsetx, out var offsety);

			for (var x = 1; x < width - 2; x++)
			{
				for (var y = 1; y < height - 2; y++)
				{
					var e = GetElementFromBiomeElementTable(x, y, table, noiseMap);
					PlaceElement(worldIdx, x + offsetx, y + offsety, e.element.id, e.temperature, e.mass);
				}
			}
		}


		private TerrainCell.ElementOverride GetElementFromBiomeElementTable(int x, int y, List<ElementGradient> gradients, NoiseMap noiseMap)
		{
			var num = noiseMap.GetValue(x, y);

			var elementOverride = TerrainCell.GetElementOverride(WorldGen.voidElement.tag.ToString(), null);

			if (gradients.Count == 0)
			{
				return elementOverride;
			}

			for (var i = 0; i < gradients.Count; i++)
			{
				Debug.Assert(gradients[i].content != null, i.ToString());
				if (num < gradients[i].maxValue)
					return TerrainCell.GetElementOverride(gradients[i].content, gradients[i].overrides);
			}

			return TerrainCell.GetElementOverride(gradients[gradients.Count - 1].content, gradients[gradients.Count - 1].overrides);
		}

		private void DrawBorder(int bottomLeftCell, int width, int height)
		{
			var worldIdx = Grid.WorldIdx[bottomLeftCell];
			Grid.CellToXY(bottomLeftCell, out var x, out var y);

			for (var xo = 0; xo < width; xo++)
			{
				PlaceElement(worldIdx, x + xo, y, border);
				PlaceElement(worldIdx, x + xo, y + height, border);
			}

			for (var yo = 0; yo < height; yo++)
			{
				PlaceElement(worldIdx, x, y + yo, border);
				PlaceElement(worldIdx, x + width, y + yo, border);
			}
		}

		private void PlaceElement(int worldIdx, int x, int y, SimHashes element, float temperature = 300f, float mass = 1000f)
		{
			var cell = Grid.XYToCell(x, y);

			if (!Grid.IsValidCell(cell) || !Grid.IsValidCellInWorld(cell, worldIdx))
				return;

			SimMessages.ReplaceElement(cell, element, CellEventLogger.Instance.DebugTool, mass, temperature);
		}
	}
}
