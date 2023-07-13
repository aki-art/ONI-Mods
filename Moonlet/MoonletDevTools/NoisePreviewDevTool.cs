using FUtility;
using ImGuiNET;
using Klei;
using LibNoiseDotNet.Graphics.Tools.Noise.Builder;
using ProcGen;
using ProcGenGame;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Path = System.IO.Path;

namespace Moonlet.MoonletDevTools
{
	public class NoisePreviewDevTool : DevTool
	{
		public static int width = 100;
		public static int height = 100;
		public static SimHashes border = SimHashes.Unobtanium;
		public static string biomeFilepath = "worldgen/test_biome";
		public static string noiseFilepath = "worldgen/test_noise";
		public static Vector3 bottomLeft = new(0, 0);

		public static LineRenderer areaMarker;

		public static bool watchFile = false;

		public static List<ElementBandConfiguration> biomeElements;

		private NoiseMap noiseMap;
		private BiomeSettings biomeSettings;

		public NoisePreviewDevTool()
		{
			RequiresGameRunning = true;
			biomeElements = new List<ElementBandConfiguration>();
		}

		private static void UpdateMarker()
		{
			areaMarker.SetPositions(new[]
			{
				bottomLeft,
				new (bottomLeft.x + width, bottomLeft.y),
				new (bottomLeft.x + width, bottomLeft.y + height),
				new (bottomLeft.x, bottomLeft.y + height)
			});
		}

		public override void RenderTo(DevPanel panel)
		{
			if (areaMarker == null)
			{
				areaMarker = ModDebug.AddSimpleLineRenderer(Game.Instance.transform, Color.green, Color.green, 0.1f);
				areaMarker.loop = true;
				areaMarker.positionCount = 4;
				UpdateMarker();
			}

			areaMarker.gameObject.SetActive(true);

			ImGui.Spacing();

			ImGui.InputText("biome file", ref biomeFilepath, 1024);
			ImGui.InputText("noise file", ref noiseFilepath, 1024);
			ImGui.Spacing();

			ImGui.InputInt("width", ref width);
			ImGui.InputInt("height", ref height);
			ImGui.Spacing();

			if (ImGui.Button("Set Bottom Left corner"))
			{
				var cell = SelectTool.Instance.selectedCell;
				if (Grid.IsValidCell(cell))
				{
					bottomLeft = Grid.CellToPos(cell);
					UpdateMarker();
				}
			}

			if (ImGui.Button("Set Bottom Right corner"))
			{
				var cell = SelectTool.Instance.selectedCell;
				if (Grid.IsValidCell(cell))
				{
					Grid.CellToXY(cell, out int x, out int y);
					width = x - (int)bottomLeft.x;
					height = y - (int)bottomLeft.y;

					UpdateMarker();
				}
			}

			ImGui.Spacing();

			if (ImGui.Button("Generate"))
			{
				LoadElementGradients();
				LoadNoise();
				GenerateTerrain();
				DrawBorder();
			}

			DrawBiomesPanel();

			if (isRequestingToClosePanel)
				areaMarker.gameObject.SetActive(false);
		}

		private void DrawBiomesPanel()
		{
		}

		private void OnChanged(object sender, FileSystemEventArgs e)
		{
			LoadElementGradients();
			LoadNoise();
			GenerateTerrain();
			DrawBorder();
		}

		private void LoadElementGradients()
		{
			var path = Path.Combine(Utils.ModPath, biomeFilepath + ".yaml");
			biomeSettings = YamlIO.LoadFile<BiomeSettings>(path);

			foreach (var config in biomeSettings.TerrainBiomeLookupTable)
			{
				config.Value.ConvertBandSizeToMaxSize();
			}
		}

		private void GenerateTerrain()
		{
			var table = biomeSettings.TerrainBiomeLookupTable["Default"];
			Grid.PosToXY(bottomLeft, out var offsetx, out var offsety);

			for (var x = 1; x < width - 2; x++)
			{
				for (var y = 1; y < height - 2; y++)
				{
					var e = GetElementFromBiomeElementTable(x, y, table);
					PlaceElement(x + offsetx, y + offsety, e.element.id, e.temperature, e.mass);
				}
			}
		}

		private TerrainCell.ElementOverride GetElementFromBiomeElementTable(int x, int y, List<ElementGradient> gradients)
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
				{
					return TerrainCell.GetElementOverride(gradients[i].content, gradients[i].overrides);
				}
			}

			return TerrainCell.GetElementOverride(gradients[gradients.Count - 1].content, gradients[gradients.Count - 1].overrides);
		}

		private void LoadNoise()
		{
			var path = Path.Combine(Utils.ModPath, noiseFilepath + ".yaml");
			var tree = YamlIO.LoadFile<ProcGen.Noise.Tree>(path);
			var plane = BuildNoiseSource(width, height, tree);
			noiseMap = WorldGen.BuildNoiseMap(Vector3.zero, tree.settings.zoom, plane, width, height, null);
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

		private void DrawBorder()
		{
			Grid.PosToXY(bottomLeft, out var x, out var y);
			for (var xo = 0; xo < width; xo++)
			{
				PlaceElement(x + xo, y, border);
				PlaceElement(x + xo, y + height, border);
			}

			for (var yo = 0; yo < height; yo++)
			{
				PlaceElement(x, y + yo, border);
				PlaceElement(x + width, y + yo, border);
			}
		}

		private void PlaceElement(int x, int y, SimHashes element, float temperature = 300f, float mass = 1000f)
		{
			var cell = Grid.XYToCell(x, y);

			if (!Grid.IsValidCell(cell))
			{
				return;
			}

			SimMessages.ReplaceElement(cell, element, CellEventLogger.Instance.DebugTool, mass, temperature, 255, 0);
		}
	}
}
