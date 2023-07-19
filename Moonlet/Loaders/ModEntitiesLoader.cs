using FUtility;
using HarmonyLib;
using Moonlet.Entities;
using Moonlet.Entities.Commands;
using Moonlet.Entities.ComponentTypes;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Moonlet.Loaders
{
	public class ModEntitiesLoader : BaseLoader
	{
		public string EntitiesFolder => Path.Combine(path, data.DataPath, "entities");
		public string DebrisFolder => Path.Combine(EntitiesFolder, "debris");

		public ModEntitiesLoader(KMod.Mod mod, MoonletData data) : base(mod, data)
		{
		}

		public void LoadDebris(HashSet<SimHashes> simHashes)
		{
			var path = DebrisFolder;

			if (!Directory.Exists(path))
				return;

			var mappings = new Dictionary<string, Type>()
			{
				// components
				{ "tag:yaml.org,2002:sublimates", typeof(SublimatesComponent) },
				{ "tag:yaml.org,2002:edible", typeof(EdibleComponent) },

				// commands
				{ "tag:yaml.org,2002:destroy", typeof(DestroyCommand) },
			};

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

				simHashes.Add(elementEntry.simHash);

				ProcessComponents(debris, prefab);
				ProcessCommands(debris, prefab);

				Assets.AddPrefab(prefab.GetComponent<KPrefabID>());
			}
		}

		private void ProcessCommands(DebrisData debris, GameObject prefab)
		{
			debris.OnSpawn?.Do(a => a.RunOnPrefab(prefab));
		}

		private void ProcessComponents(DebrisData debris, GameObject prefab)
		{
			var components = debris.Components;

			if (components == null)
			{
				Log.Debuglog("no components");
				return;
			}

			foreach (var component in components)
				component.Apply(prefab);
		}
	}
}
