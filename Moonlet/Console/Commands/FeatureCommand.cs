using ProcGen;
using ProcGenGame;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Moonlet.Console.Commands
{
	public class FeatureCommand() : CommandBase(ID)
	{
		public const string ID = "feature";
		private SeededRandom rnd;
		private static FeatureSettings testFeature;

		public override void SetupArguments()
		{
			base.SetupArguments();
			arguments = new()
			{
				new ArgumentInfo[]
				{
					new StringArgument("place", "Place a template", optional: false),
					new StringArgument("featureId", "Id of feature", optional: false),
					new IntArgument("seed", "Seed", optional: true),
				},
				new ArgumentInfo[]
				{
					new StringArgument("test", "Runtime editable template", optional: false),
					new IntArgument("seed", "Seed", optional: true),
				},
				new ArgumentInfo[]
				{
					new StringArgument("open", "Open the editable template", optional: false),
				},
				new ArgumentInfo[]
				{
					new StringArgument("list", "List all loaded feature Id-s", optional: false),
				},
				new ArgumentInfo[]
				{
					new StringArgument("save", "save template", optional: false),
					new StringArgument("path", "path", optional: false),
				},
			};
		}

		public override CommandResult Run()
		{
			if (GetStringArgument(1, out var subCommand))
			{
				switch (subCommand)
				{
					case "save":
						return Save();
					case "open":
						Application.OpenURL(Utils.FileUtil.GetOrCreateDirectory(System.IO.Path.Combine(FUtility.Utils.ModPath, "test_worldgen", "features")));
						return CommandResult.success;
					case "place":
						return Place();
					case "test":
						return PlaceTest();
					case "list":
						foreach (var featureName in SettingsCache.featureSettings.Keys)
							DevConsole.Log(featureName);
						return CommandResult.success;
				}
			}

			return CommandResult.Warning("Not a command.");
		}

		private CommandResult PlaceTest()
		{
			var cell = SelectTool.Instance.selectedCell;
			if (!Grid.IsValidCell(cell))
				return CommandResult.noCellSelection;

			LoadTestFeature();

			if (testFeature != null)
			{
				rnd = GetIntArgument(3, out var seed)
					? new SeededRandom(seed)
					: new SeededRandom(Random.Range(0, int.MaxValue));

				return SpawnFeature(testFeature, cell);
			}
			else
				return CommandResult.Warning("something went wrong trying to spawn the test feature");
		}

		private CommandResult Place()
		{
			if (GetStringArgument(2, out var featureId)
				&& SettingsCache.featureSettings.TryGetValue(featureId, out var feature))
			{
				var cell = SelectTool.Instance.selectedCell;
				if (!Grid.IsValidCell(cell))
					return CommandResult.noCellSelection;

				rnd = GetIntArgument(3, out var seed)
					? new SeededRandom(seed)
					: new SeededRandom(Random.Range(0, int.MaxValue));

				return SpawnFeature(feature, cell);
			}

			return CommandResult.Warning("Requires a feature Id.");
		}

		private CommandResult Save()
		{
			if (DevConsole.currentProject.IsNullOrWhiteSpace())
				return CommandResult.Warning("Please use command `project <StaticID>` to set an active project first.");

			if (GetStringArgument(2, out var path))
				return SaveFeature(DevConsole.currentProject, path);
			else
				return CommandResult.Warning("Path required");
		}

		private CommandResult SaveFeature(string staticId, string partialPath)
		{
			if (testFeature == null)
				LoadTestFeature();

			var path = MoonletMods.Instance.GetDataPath(staticId, "worldgen");
			if (path == null)
				return CommandResult.Error("No mod with id " + staticId);

			if (!partialPath.StartsWith("features/"))
				path = System.IO.Path.Combine(path, "features");

			path = Utils.FileUtil.GetOrCreateDirectory(path);

			path = System.IO.Path.Combine(path, partialPath);

			if (!path.EndsWith(".yaml") && !path.EndsWith(".YAML"))
				path += ".yaml";

			Utils.FileUtil.WriteYAML(path, testFeature);

			return CommandResult.Success($"File saved to {path}");
		}

		private void LoadTestFeature()
		{
			var path = System.IO.Path.Combine(FUtility.Utils.ModPath, "test_worldgen", "features");
			var file = System.IO.Path.Combine(path, "test.yaml");

			if (!File.Exists(file))
			{
				testFeature = new FeatureSettings()
				{
					ElementChoiceGroups = new Dictionary<string, ElementChoiceGroup<WeightedSimHash>>()
					{
						{
							"RoomCenterElements",
							new ElementChoiceGroup<WeightedSimHash>()
							{
								choices = new()
								{
									new WeightedSimHash(SimHashes.Algae.ToString(), 1f),
									new WeightedSimHash(SimHashes.Dirt.ToString(), 1f),
								},
								selectionMethod = ProcGen.Room.Selection.WeightedResample
							}
						},

						{
							"RoomBorderChoices0",
							new ElementChoiceGroup<WeightedSimHash>()
							{
								choices = new()
								{
									new WeightedSimHash(SimHashes.Iron.ToString(), 1f),
								},
								selectionMethod = ProcGen.Room.Selection.HorizontalSlice
							}
						},

						{
							"RoomBorderChoices1",
							new ElementChoiceGroup<WeightedSimHash>()
							{
								choices = new()
								{
									new WeightedSimHash(SimHashes.Katairite.ToString(), 1f),
								},
								selectionMethod = ProcGen.Room.Selection.HorizontalSlice
							}
						}
					},
					borders = new List<int>() { 1, 1 },
					tags = new List<string>(),
					internalMobs = new List<MobReference>(),
					blobSize = new(2, 4),
					shape = ProcGen.Room.Shape.Circle
				};


				Utils.FileUtil.GetOrCreateDirectory(path);
				Utils.FileUtil.WriteYAML(file, testFeature);
			}
			else
			{
				if (!Utils.FileUtil.TryReadBasicYaml(file, out testFeature))
					DevConsole.Log("Could not load test feature file.");
			}
		}

		private CommandResult SpawnFeature(FeatureSettings feature, int cell)
		{
			var featureCenterPoints = new List<Vector2I>();
			var featureBorders = new List<List<Vector2I>>();
			var sourcePoints = new HashSet<Vector2I>();

			var center = Grid.CellToPos(cell);
			var size = feature.blobSize.GetRandomValueWithinRange(rnd);

			switch (feature.shape)
			{
				case ProcGen.Room.Shape.Circle:
					featureCenterPoints = ProcGen.Util.GetFilledCircle(center, size);
					break;
				case ProcGen.Room.Shape.Blob:
					featureCenterPoints = ProcGen.Util.GetBlob(center, size, rnd.RandomSource());
					break;
				case ProcGen.Room.Shape.Square:
					featureCenterPoints = ProcGen.Util.GetFilledRectangle(center, size, size, rnd);
					break;
				case ProcGen.Room.Shape.TallThin:
					featureCenterPoints = ProcGen.Util.GetFilledRectangle(center, size / 4f, size, rnd);
					break;
				case ProcGen.Room.Shape.ShortWide:
					featureCenterPoints = ProcGen.Util.GetFilledRectangle(center, size, size / 4f, rnd);
					break;
				case ProcGen.Room.Shape.Splat:
					featureCenterPoints = ProcGen.Util.GetSplat(center, size, rnd.RandomSource());
					break;
			}

			sourcePoints.UnionWith(featureCenterPoints);

			var bordersWidths = feature.borders;

			if (bordersWidths != null && bordersWidths.Count > 0 && bordersWidths[0] > 0)
			{
				for (int index = 0; index < bordersWidths.Count && bordersWidths[index] > 0; ++index)
				{
					featureBorders.Add(ProcGen.Util.GetBorder(sourcePoints, bordersWidths[index]));
					sourcePoints.UnionWith(featureBorders[index]);
				}
			}

			ApplyPlaceElementForRoom(feature, "RoomCenterElements", featureCenterPoints, rnd);

			if (featureBorders != null)
			{
				for (int index = 0; index < featureBorders.Count; ++index)
					ApplyPlaceElementForRoom(feature, $"RoomBorderChoices{index}", featureBorders[index], rnd);
			}

			return CommandResult.success;
		}


		private void ApplyPlaceElementForRoom(FeatureSettings feature, string group, List<Vector2I> cells, SeededRandom rnd)
		{
			if (cells == null || cells.Count == 0 || !feature.HasGroup(group))
				return;

			switch (feature.ElementChoiceGroups[group].selectionMethod)
			{
				case ProcGen.Room.Selection.Weighted:
				case ProcGen.Room.Selection.WeightedResample:
					for (int index = 0; index < cells.Count; ++index)
					{
						int cell = Grid.XYToCell(cells[index].x, cells[index].y);

						if (Grid.IsValidCell(cell))
						{
							var oneWeightedSimHash = feature.GetOneWeightedSimHash(group, rnd);
							TerrainCell.ElementOverride elementOverride = GetElement(oneWeightedSimHash);

							PlaceElement(cell, elementOverride.element, elementOverride.pdelement, elementOverride.dc);
						}
					}
					break;
				case ProcGen.Room.Selection.HorizontalSlice:
					int b1 = int.MaxValue;
					int b2 = int.MinValue;
					for (int index = 0; index < cells.Count; ++index)
					{
						b1 = Mathf.Min(cells[index].y, b1);
						b2 = Mathf.Max(cells[index].y, b2);
					}
					int num = b2 - b1;
					for (int index = 0; index < cells.Count; ++index)
					{
						var cell = Grid.XYToCell(cells[index].x, cells[index].y);
						if (Grid.IsValidCell(cell))
						{
							var percentage = (float)(1.0 - (double)(cells[index].y - b1) / (double)num);
							var weightedSimHashAtChoice = feature.GetWeightedSimHashAtChoice(group, percentage);
							var elementOverride = GetElement(weightedSimHashAtChoice);

							PlaceElement(cell, elementOverride.element, elementOverride.pdelement, elementOverride.dc);
						}
					}
					break;
				default:
					WeightedSimHash oneWeightedSimHash1 = feature.GetOneWeightedSimHash(group, rnd);
					for (int index = 0; index < cells.Count; ++index)
					{
						int cell = Grid.XYToCell(cells[index].x, cells[index].y);
						if (Grid.IsValidCell(cell))
						{
							var elementOverride = GetElement(oneWeightedSimHash1);
							PlaceElement(cell, elementOverride.element, elementOverride.pdelement, elementOverride.dc);
						}
					}
					break;
			}
		}

		private static TerrainCell.ElementOverride GetElement(WeightedSimHash oneWeightedSimHash)
		{
			var element = ElementLoader.FindElementByName(oneWeightedSimHash.element);

			if (element == null)
				return default;

			var elementOverride = TerrainCell.GetElementOverride(oneWeightedSimHash.element, oneWeightedSimHash.overrides);

			if (!elementOverride.overrideTemperature)
				elementOverride.pdelement.temperature = element.defaultValues.temperature;

			if (!elementOverride.overrideMass)
				elementOverride.pdelement.mass = element.defaultValues.mass;

			return elementOverride;
		}

		private void PlaceElement(int cell, Element element, Sim.PhysicsData pdelement, Sim.DiseaseCell dc)
		{
			if (element == null)
				return;

			SimMessages.ReplaceElement(
				cell,
				element.id,
				DevConsole.cellElementEvent,
				pdelement.mass,
				pdelement.temperature,
				dc.diseaseIdx,
				dc.elementCount);
		}
	}
}
