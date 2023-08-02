using FUtility;
using Moonlet.Content.Scripts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Moonlet.Loaders
{
	public class ModGeyserLoader : BaseLoader
	{
		public string GeysersFolder => Path.Combine(path, data.DataPath, "geysers");

		public List<GeyserData> geysers;

		public ModGeyserLoader(KMod.Mod mod, MoonletData data) : base(mod, data)
		{
			LoadYAMLs();
		}

		public void RegisterConfigs(ref List<GeyserGenericConfig.GeyserPrefabParams> configs)
		{
			if (geysers == null)
				return;

			foreach (var geyser in geysers)
			{
				if (configs.Any(g => g.id == geyser.Id))
				{
					Log.Warning("Duplicate Geyser entry: " + geyser.Id);
					continue;
				}

				if (!MiscUtil.ParseElementEntry(geyser.Element, out var elementId))
					continue;

				if (!Mod.AreAnyOfTheseEnabled(geyser.RequiredMods))
					continue;

				if (geyser.Type.IsNullOrWhiteSpace()
					|| !Enum.TryParse(geyser.Type, out GeyserConfigurator.GeyserShape shape))
				{
					var element = ElementLoader.FindElementByHash(elementId);
					if (element.IsLiquid)
					{
						shape = element.HasTag(GameTags.Metal)
							? GeyserConfigurator.GeyserShape.Molten
							: GeyserConfigurator.GeyserShape.Liquid;
					}
					else
						shape = GeyserConfigurator.GeyserShape.Gas;
				}

				var geyserType = new GeyserConfigurator.GeyserType(
					geyser.Id,
					elementId,
					shape,
					geyser.TemperatureCelsius,
					geyser.MinRatePerCycle,
					geyser.MaxRatePerCycle,
					geyser.MaxPressure,
					geyser.MinIterationLength, geyser.MaxIterationLength,
					geyser.MinIterationPercent, geyser.MaxIterationPercent,
					geyser.MinYearLength, geyser.MaxYearLength,
					geyser.MinYearPercent, geyser.MaxYearPercent,
					geyser.GeyserTemperature,
					geyser.DlcId);

				configs.Add(new GeyserGenericConfig.GeyserPrefabParams(
					geyser.Animation,
					geyser.Width,
					geyser.Height,
					geyserType,
					geyser.Generic));

				if (data.DebugLogging)
					Log.Info("Registered geyser " + geyser.Id);
			}
		}

		public void LoadYAMLs()
		{
			var path = GeysersFolder;

			if (!Directory.Exists(path))
				return;

			foreach (var file in Directory.GetFiles(path, "*.yaml"))
			{
				if (data.DebugLogging)
					Log.Info("Loading geysers file " + file);

				var collection = FileUtil.Read<GeyserCollection>(file);

				if (collection?.Geysers == null)
					continue;

				geysers ??= new List<GeyserData>();
				geysers.AddRange(collection.Geysers);
			}
		}

		public void PostProcessGeyser(GameObject result)
		{
			if (geysers == null)
				return;

			var preset = result.GetComponent<GeyserConfigurator>().presetType;

			foreach (var geyser in geysers)
			{
				if (geyser.Id == preset
					&& !geyser.TintFx.IsNullOrWhiteSpace()
					&& geyser.TintSymbols != null)
				{
					var color = MiscUtil.ParseColor(geyser.TintFx);
					result.GetComponent<KPrefabID>().prefabSpawnFn += go =>
					{
						go.TryGetComponent(out KBatchedAnimController kbac);

						foreach (var symbol in geyser.TintSymbols)
							kbac.SetSymbolTint(symbol, color);
					};
				}
			}
		}

		public class GeyserCollection
		{
			public List<GeyserData> Geysers { get; set; }
		}

	}
}
