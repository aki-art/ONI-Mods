using FUtility;
using HarmonyLib;
using Moonlet.Content.Scripts;
using Moonlet.Entities;
using Moonlet.Entities.Commands;
using Moonlet.Entities.ComponentTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Moonlet.Loaders
{
	public class ModEntitiesLoader : BaseLoader
	{
		public string EntitiesFolder => Path.Combine(path, data.DataPath, "entities");

		public string DebrisFolder => Path.Combine(EntitiesFolder, "debris");

		public string BuildingsFolder => Path.Combine(EntitiesFolder, "buildings");

		public string PoisFolder => Path.Combine(EntitiesFolder, "pois");

		public Dictionary<string, BuildingData> buildings;

		public static Dictionary<string, Type> mappings = new()
		{
			// components
			{ "tag:yaml.org,2002:sublimates", typeof(SublimatesComponent) },
			{ "tag:yaml.org,2002:edible", typeof(EdibleComponent) },
			{ "tag:yaml.org,2002:lightEmitter", typeof(LightEmitterComponent) },
			{ "tag:yaml.org,2002:radiationEmitter", typeof(RadiationEmitterComponent) },
			{ "tag:yaml.org,2002:storage", typeof(StorageComponent) },
			{ "tag:yaml.org,2002:demolishable", typeof(DemolishableComponent) },
			{ "tag:yaml.org,2002:rummagable", typeof(RummagableComponent) },
			{ "tag:yaml.org,2002:simpleGenerator", typeof(SimpleGeneratorComponent) },
			{ "tag:yaml.org,2002:diggable", typeof(DiggableComponent) },
			{ "tag:yaml.org,2002:foundationMonitor", typeof(FoundationMonitorComponent) },

			// commands
			{ "tag:yaml.org,2002:destroy", typeof(DestroyCommand) },
			{ "tag:yaml.org,2002:spawnitems", typeof(SpawnItemsCommand) },
			{ "tag:yaml.org,2002:spawnelement", typeof(SpawnElementCommand) },
			{ "tag:yaml.org,2002:playRandom", typeof(RandomAnimationCommand) },
			{ "tag:yaml.org,2002:play", typeof(PlayAnimationCommand) },
			{ "tag:yaml.org,2002:dropMaterials", typeof(DropMaterialsCommand) },
		};

		public ModEntitiesLoader(KMod.Mod mod, MoonletData data) : base(mod, data)
		{
		}

		public void LoadBuildings()
		{
			var path = BuildingsFolder;

			if (!Directory.Exists(path))
				return;

			foreach (var file in Directory.GetFiles(path, "*.yaml", SearchOption.AllDirectories))
			{
				buildings ??= new();

				var entry = FileUtil.Read<BuildingData>(file, mappings: mappings);

				if (entry == null)
				{
					Log.Warning($"Building file {file} was found, but could not be parsed.");
					continue;
				}

				if (entry.Materials == null)
				{
					Log.Warning($"Building file {file} requires at least 1 material.");
					continue;
				}

				var mass = new float[entry.Materials.Length];
				var ingredients = new string[entry.Materials.Length];

				for (int i = 0; i < entry.Materials.Length; i++)
				{
					var material = entry.Materials[i];
					mass[i] = material.Mass;
					ingredients[i] = material.Material;
				}

				var prefab = BuildingTemplates.CreateBuildingDef(
					entry.Id,
					entry.Width,
					entry.Height,
					entry.Animation.File,
					entry.HitPoints,
					entry.ConstructionTime,
					mass,
					ingredients,
					entry.MeltingPointKelvin,
					entry.BuildLocationRule,
					entry.GetDecor(),
					default);

				var config = new GenericBuildingConfig
				{
					def = prefab,
					skipLoading = false
				};

				if (!entry.OverlayMode.IsNullOrWhiteSpace())
					prefab.ViewMode = entry.OverlayMode;

				if (entry.PowerOutlet != null)
				{
					prefab.RequiresPowerOutput = true;
					prefab.PowerOutputOffset = new(entry.PowerOutlet.X, entry.PowerOutlet.Y);
				}

				if (entry.PowerInlet != null)
				{
					prefab.RequiresPowerInput = true;
					prefab.PowerInputOffset = new(entry.PowerInlet.X, entry.PowerInlet.Y);
				}

				if (entry.ConduitIn != null)
				{
					prefab.InputConduitType = entry.ConduitIn.Type;
					prefab.UtilityInputOffset = new(entry.ConduitIn.X, entry.ConduitIn.Y);
				}

				if (entry.ConduitOut != null)
				{
					prefab.OutputConduitType = entry.ConduitIn.Type;
					prefab.UtilityOutputOffset = new(entry.ConduitOut.X, entry.ConduitOut.Y);
				}

				if (entry.Generator != null)
				{
					prefab.GeneratorBaseCapacity = entry.Generator.BaseCapacity;
					prefab.GeneratorWattageRating = entry.Generator.Wattage;
				}

				prefab.ExhaustKilowattsWhenActive = entry.ExhaustKilowattsWhenActive;
				prefab.SelfHeatKilowattsWhenActive = entry.SelfHeatKilowattsWhenActive;

				buildings.Add(entry.Id, entry);
				Log.Debuglog("added building " + entry.Id);

				BuildingConfigManager.Instance.RegisterBuilding(config);

				if (!entry.Category.IsNullOrWhiteSpace())
				{
					if (entry.Neighbor.IsNullOrWhiteSpace())
						ModUtil.AddBuildingToPlanScreen(
							entry.Category,
							entry.Id,
							entry.SubCategory ?? "uncategorized");
					else
						ModUtil.AddBuildingToPlanScreen(
							entry.Category,
							entry.Id,
							entry.SubCategory ?? "uncategorized",
							entry.Neighbor,
							entry.IsAfterNeighbor ? ModUtil.BuildingOrdering.After : ModUtil.BuildingOrdering.Before);
				}

				Mod.AddStrings($"STRINGS.BUILDINGS.PREFABS.{entry.Id.ToUpperInvariant()}.NAME", entry.Name ?? "MISSING.");
				Mod.AddStrings($"STRINGS.BUILDINGS.PREFABS.{entry.Id.ToUpperInvariant()}.DESC", entry.Description ?? "MISSING.");
				Mod.AddStrings($"STRINGS.BUILDINGS.PREFABS.{entry.Id.ToUpperInvariant()}.EFFECT", entry.EffectDescription ?? "MISSING.");
			}
		}

		public void LoadPois()
		{
			Log.Debuglog("Loading POIs");

			var path = PoisFolder;

			if (!Directory.Exists(path))
				return;

			foreach (var file in Directory.GetFiles(path, "*.yaml", SearchOption.AllDirectories))
			{
				var entityData = FileUtil.Read<PoiData>(file, mappings: mappings);

				if (entityData == null)
				{
					Log.Warning($"Debris file {file} was found, but could not be parsed.");
					continue;
				}

				Log.Debuglog("Loading entity: " + entityData.Id);

				MiscUtil.ParseElementEntry(entityData.Element, out var elementId);
				elementId = elementId == SimHashes.Void ? SimHashes.Creature : elementId;


				if (entityData.DefaultTemperatureCelsius.HasValue)
					entityData.DefaultTemperature = entityData.DefaultTemperatureCelsius.Value + 273.15f;

				var temperature = entityData.DefaultTemperature ?? 293f;

				var prefab = EntityTemplates.CreatePlacedEntity(
					entityData.Id,
					entityData.Name,
					entityData.Description,
					entityData.Mass,
					Assets.GetAnim(entityData.Animation.File),
					entityData.Animation.DefaultAnimation,
					Grid.SceneLayer.Creatures,
					entityData.Width,
					entityData.Height,
					new EffectorValues(entityData.Decor.Value, entityData.Decor.Range),
					default,
					elementId,
					entityData.Tags?.ToTagList(),
					temperature);

				if(entityData.Layers != null)
				{
					var occupyArea = prefab.AddComponent<OccupyArea>();
					occupyArea.objectLayers = entityData.Layers;
					occupyArea.SetCellOffsets(Utils.MakeCellOffsets(entityData.Width, entityData.Height));
				}

				prefab.AddOrGet<MoonletEntityComponent>();

				ProcessCommands(entityData, prefab);
				ProcessComponents(entityData, prefab);

				Assets.AddPrefab(prefab.GetComponent<KPrefabID>());
			}
		}

		public void LoadDebris(HashSet<SimHashes> simHashes)
		{
			var path = DebrisFolder;

			if (!Directory.Exists(path))
				return;


			foreach (var file in Directory.GetFiles(path, "*.yaml", SearchOption.AllDirectories))
			{
				var debris = FileUtil.Read<DebrisData>(file, mappings: mappings);

				if (debris == null)
				{
					Log.Warning($"Debris file {file} was found, but could not be parsed.");
					continue;
				}

				var elementEntry = Mod.sharedElementsLoader.GetEntry(debris.Id);

				if (elementEntry == null)
				{
					Log.Warning($"Debris entry for {debris.Id} was defined, but there is no such element.");
					continue;
				}

				var element = ElementLoader.FindElementByHash(elementEntry.simHash);

				if (element == null)
				{
					Log.Warning($"No element with id {elementEntry.ElementId}");
					continue;
				}

				if (simHashes.Contains(element.id))
				{
					Log.Warning($"The element {element.id} already has a debris registered.");
					continue;
				}

				GameObject prefab = null;

				if (element.IsSolid)
					prefab = EntityTemplates.CreateSolidOreEntity(element.id);
				else if (element.IsLiquid)
					prefab = EntityTemplates.CreateLiquidOreEntity(element.id);
				else if (element.IsGas)
					prefab = EntityTemplates.CreateGasOreEntity(element.id);

				prefab.AddOrGet<MoonletEntityComponent>();

				simHashes.Add(elementEntry.simHash);

				ProcessComponents(debris, prefab);
				ProcessCommands(debris, prefab);

				Assets.AddPrefab(prefab.GetComponent<KPrefabID>());
			}
		}

		public static void ProcessCommands(EntityData data, GameObject prefab)
		{
			if (prefab.TryGetComponent(out KPrefabID prefabID))
			{
				if (data.OnSpawn != null)
				{
					foreach (var item in data.OnSpawn)
					{
						if (item == null)
							continue;

						prefabID.prefabSpawnFn += item.Run;
					}
				}

				// have to do this an odd ways because Unity is unwilling to serialize Lists or Events in mods for some reason
				if (data.OnDestroy != null)
				{
					foreach (var item in data.OnDestroy)
					{
						if (item == null)
							continue;

						prefabID.prefabInitFn += go =>
						{
							if (go.TryGetComponent(out MoonletEntityComponent component))
								component.AddOnDestroyFn(item.Run);
						};
					}
				}
			}
		}

		public static void ProcessComponents(EntityData data, GameObject prefab)
		{
			var components = data.Components;

			Log.Debuglog($"processing components: {data.Id}");

			if (components == null)
			{
				Log.Debuglog("no components");
				return;
			}

			foreach (var component in components)
			{
				if (component == null)
				{
					Log.Debuglog("null component");
					continue;
				}

				Log.Debuglog("processing component: " + component.GetType());
				component?.Apply(prefab);
			}
		}

		public void LoadItems()
		{
		}
	}
}
