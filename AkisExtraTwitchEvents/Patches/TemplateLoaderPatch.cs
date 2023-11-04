using FUtility;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using TemplateClasses;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Patches
{
	public class TemplateLoaderPatch
	{
		public const string
			KEEP_ENTITIES = "AETE_KeepEntities",
			MIN_PRESSURE = "AETE_MinPressure",
			PEDESTAL = "AETE_Pedestal",
			PUZZLEDOOR = "AETE_PuzzleDoor",
			POCKET_DIMENSION = "AETE_PocketDimension",
			ROCKET = "AETE_Rocket";

		private class DoorPair
		{
			public Vector2I door;
			public Vector2I sensor;
		}

		[HarmonyPatch(typeof(TemplateLoader), "PlaceBuilding")]
		public class TemplateLoader_PlaceBuilding_Patch
		{
			public static void Postfix(Prefab prefab, int root_cell, ref GameObject __result)
			{
				if (prefab.other_values == null || __result == null)
					return;

				foreach (var entry in prefab.other_values)
				{
					if (entry.id == MIN_PRESSURE)
					{
						if (__result.TryGetComponent(out LogicPressureSensor sensor))
						{
							sensor.threshold = entry.value;
							sensor.activateAboveThreshold = true;
						}
					}

					/*					if (entry.id == PEDESTAL)
										{
											if (__result.TryGetComponent(out SingleEntityReceptacle receptacle)
												&& __result.TryGetComponent(out Storage storage)
												&& storage.items.Count > 0)
											{
												GameObject depositedObject = storage.Drop(storage.items[0]);
												if (depositedObject != null)
													receptacle.ForceDeposit(depositedObject);
											}
										}*/
				}
			}
		}

		private static bool GetValue(Prefab prefab, string id, out float value)
		{
			value = 0;
			if (prefab.other_values == null)
				return false;

			foreach (var entry in prefab.other_values)
			{
				if (entry.id == id)
				{
					value = entry.value;
					return true;
				}
			}

			return false;
		}


		[HarmonyPatch(typeof(TemplateLoader), "StampComplete")]
		public class TemplateLoader_StampComplete_Patch
		{
			public static void Prefix(TemplateLoader.ActiveStamp stamp)
			{
				if (stamp?.m_template?.info?.tags != null)
				{
					var tags = stamp.m_template.info.tags;

					if (tags.Contains(PUZZLEDOOR))
						ConnectPuzzleDoors(stamp);

					if (tags.Contains(ROCKET))
						ProcessRocket(stamp);

					if (tags.Contains(POCKET_DIMENSION))
						AddNoDeconstruct(stamp);
				}
			}

			private static void AddNoDeconstruct(TemplateLoader.ActiveStamp stamp)
			{
				var world = ClusterManager.Instance.GetWorldFromPosition(new Vector3(stamp.m_rootLocation.X + stamp.m_template.info.size.x / 2, stamp.m_rootLocation.Y + stamp.m_template.info.size.y / 2));
				if (world != null)
				{
					foreach (var building in Components.BuildingCompletes.GetWorldItems(world.id))
					{
						if (building.TryGetComponent(out KPrefabID kPrefabId))
						{
							if (kPrefabId.HasTag(GameTags.TemplateBuilding))
								kPrefabId.AddTag(GameTags.NoRocketRefund, true);
						}
					}
				}
			}

			private static void ProcessRocket(TemplateLoader.ActiveStamp stamp)
			{
				foreach (var building in stamp.m_template.buildings)
				{
					if (building.id == ItemPedestalConfig.ID)
					{
						var cell = Grid.OffsetCell(Grid.PosToCell(stamp.m_rootLocation), building.location_x, building.location_y);
						if (Grid.ObjectLayers[(int)ObjectLayer.Building].TryGetValue(cell, out var pedestalGo))
						{
							if (pedestalGo.TryGetComponent(out SingleEntityReceptacle receptacle)
								&& pedestalGo.TryGetComponent(out Storage storage))
							{
								var charge = FUtility.Utils.Spawn(GeneShufflerRechargeConfig.ID, Vector3.zero);
								storage.Store(charge);
								receptacle.ForceDeposit(charge);
							}
						}
					}
				}
			}

			/*			private static void ProcessRocket(TemplateLoader.ActiveStamp stamp)
						{
							var rootCell = Grid.PosToCell(stamp.m_rootLocation);

							var worldId = Grid.WorldIdx[rootCell];
							var world = ClusterManager.Instance.GetWorld(worldId);
							if (world == null)
								return;

							var verts = new List<Vector2>();

							foreach (Prefab building in stamp.m_template.buildings)
							{
								if (building.id == MetalTileConfig.ID)
								{
									var steelTileCell = Grid.OffsetCell(rootCell, building.location_x, building.location_y);
									verts.Add(Grid.CellToXY(steelTileCell));
								}
							}

							verts.Sort((v1, v2) => WorldContainer.IsClockwise(v1, v2, stamp.m_rootLocation));

							var polygon = new Polygon(verts);
							world.overworldCell.poly = polygon;
							world.overworldCell.zoneType = SubWorld.ZoneType.RocketInterior;
							world.overworldCell.tags = new TagSet()
							{
								WorldGenTags.RocketInterior
							};

							SaveLoader.Instance.clusterDetailSave.overworldCells.Add(world.overworldCell);

							for (int index1 = 0; index1 < world.worldSize.y; ++index1)
							{
								for (int index2 = 0; index2 < world.worldSize.x; ++index2)
								{
									Vector2I vector2I = new Vector2I(world.worldOffset.x + index2, world.worldOffset.y + index1);
									int cell = Grid.XYToCell(vector2I.x, vector2I.y);
									if (polygon.Contains(new Vector2((float)vector2I.x, (float)vector2I.y)))
									{
										SimMessages.ModifyCellWorldZone(cell, (byte)14);
										World.Instance.zoneRenderData.worldZoneTypes[cell] = SubWorld.ZoneType.RocketInterior;
									}
									else
									{
										SimMessages.ModifyCellWorldZone(cell, (byte)7);
										World.Instance.zoneRenderData.worldZoneTypes[cell] = SubWorld.ZoneType.Space;
									}
								}
							}
						}
			*/
			private static void ConnectPuzzleDoors(TemplateLoader.ActiveStamp stamp)
			{
				Dictionary<int, DoorPair> doors = new();

				foreach (var building in stamp.m_template.buildings)
				{
					if (building.other_values == null)
						continue;

					foreach (var value in building.other_values)
					{
						if (value.id == "AETE_PuzzleDoor")
						{
							var id = Mathf.RoundToInt(value.value);
							if (doors.TryGetValue(id, out var doorPair))
							{
								doorPair.door = new(building.location_x, building.location_y);
							}
							else
							{
								doors[id] = new DoorPair()
								{
									door = new(building.location_x, building.location_y)
								};
							}
						}
						else if (value.id == "AETE_PuzzleSensor")
						{
							var id = Mathf.RoundToInt(value.value);
							if (doors.TryGetValue(id, out var doorPair))
							{
								doorPair.sensor = new(building.location_x, building.location_y);
							}
							else
							{
								doors[id] = new DoorPair()
								{
									sensor = new(building.location_x, building.location_y)
								};
							}
						}
					}
				}

				if (doors.Count > 0)
				{
					Log.Debug("Has door pairs confifgured");

					foreach (var doorPair in doors)
					{
						var doorCell = Grid.OffsetCell(Grid.PosToCell(stamp.m_rootLocation), doorPair.Value.door.X, doorPair.Value.door.Y);
						if (Grid.ObjectLayers[(int)ObjectLayer.FoundationTile].TryGetValue(doorCell, out var doorGo))
						{
							Log.Debug("found a door");

							if (doorGo.TryGetComponent(out PuzzleDoor2 puzzleDoor))
							{
								var sensorCell = Grid.OffsetCell(Grid.PosToCell(stamp.m_rootLocation), doorPair.Value.sensor.X, doorPair.Value.sensor.Y);

								if (Grid.ObjectLayers[(int)ObjectLayer.Building].TryGetValue(sensorCell, out var sensorGo))
								{
									Log.Debug("found a sensor");
									if (sensorGo.TryGetComponent(out Switch sensor))
									{
										puzzleDoor.SetTarget(sensor);
									}
								}
							}
						}
					}
				}
			}
		}

		// Skip deleting entities from under a stamp
		[HarmonyPatch(typeof(TemplateLoader.ActiveStamp), nameof(TemplateLoader.ActiveStamp.NextPhase))]
		public class TemplateLoader_ActiveStamp_NextPhase_Patch
		{
			public static bool Prefix(TemplateLoader.ActiveStamp __instance)
			{
				Log.Debug("stamping template: " + __instance.m_template.name);

				if (__instance.currentPhase != 0)
					return true;

				if (__instance.m_template?.info?.tags != null && __instance.m_template.info.tags.Contains(KEEP_ENTITIES))
				{
					Log.Debug("has tag");
					BuildPhaseAlt1(__instance.m_rootLocation.x, __instance.m_rootLocation.y, __instance.m_template, __instance.NextPhase);
					__instance.currentPhase += 1;
					return false;
				}

				return true;
			}

			private static void BuildPhaseAlt1(int baseX, int baseY, TemplateContainer template, System.Action callback)
			{
				if (Grid.WidthInCells < 16)
					return;

				if (template.cells == null)
				{
					callback();
					return;
				}

				var cellOffsetArray = new CellOffset[template.cells.Count];

				for (int index = 0; index < template.cells.Count; ++index)
					cellOffsetArray[index] = new CellOffset(template.cells[index].location_x, template.cells[index].location_y);

				if (template.cells.Count > 0)
				{
					TemplateLoader.ApplyGridProperties(baseX, baseY, template);
					TemplateLoader.PlaceCells(baseX, baseY, template, callback);
				}
				else
					callback();
			}
		}
	}
}
