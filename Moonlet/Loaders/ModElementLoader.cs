using FUtility;
using Klei;
using Moonlet.Elements;
using System.Collections.Generic;
using System.IO;

namespace Moonlet.Loaders
{
	public class ModElementLoader : BaseLoader
	{
		public string ElementsFolder => Path.Combine(path, data.DataPath, ELEMENTS);
		public string DefaultsFile => Path.Combine(path, data.DataPath, ELEMENTS, "defaults.yaml");
		public string ModElementsFile => Path.Combine(path, data.DataPath, "modelements.yaml");

		public string ElementsTexturesFolder => Path.Combine(path, data.AssetsPath, ELEMENTS);

		public ElementDefaultsEntry modDefaults;

		public ModElementLoader(KMod.Mod mod, MoonletData data) : base(mod, data) { }

		public ElementOverrideEntryCollection CollectOverridesFromYAML()
		{
			return FileUtil.Read<ElementOverrideEntryCollection>(ModElementsFile);
		}

		public List<ExtendedElementEntry> CollectElementsFromYAML()
		{
			var path = ElementsFolder;

			if (data.DebugLogging)
				Log.Info($"Attempting to load elements {path}");

			if (!Directory.Exists(path))
			{
				if (data.DebugLogging)
					Log.Info($"No elements data folder found.");

				return null;
			}

			var result = new List<ExtendedElementEntry>();

			var defaults = DefaultsFile;

			if (File.Exists(defaults))
				modDefaults = FileUtil.Read<ElementDefaultsEntry>(defaults);

			foreach (var file in Directory.GetFiles(path, "*.yaml"))
			{
				if (data.DebugLogging)
					Log.Info("Loading element file " + file);

				var elementEntryCollection = FileUtil.Read<ExtendedElementEntryCollection>(file);

				if (elementEntryCollection?.Elements != null)
				{
					if (modDefaults != null && elementEntryCollection.Defaults != null)
						modDefaults.Merge(elementEntryCollection.Defaults);

					foreach (var element in elementEntryCollection.Elements)
					{
						element.addedBy = new List<string> { title };
						element.textureFolder = ElementsTexturesFolder;

						element.Validate();

						elementEntryCollection.Defaults?.Merge(element);
					}

					result.AddRange(elementEntryCollection.Elements);
				}
			}

			return result;
		}

		// structure kept intact with vanilla
		public class ElementOverrideEntryCollection
		{
			public List<ExtendedElementEntry> Elements { get; set; }
		}

		public class ExtendedElementEntryCollection
		{
			public ExtendedElementEntry[] Elements { get; set; }

			public ElementDefaultsEntry Defaults { get; set; }
		}
	}
}
