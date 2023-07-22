using Moonlet.ZoneTypes;
using System.Collections.Generic;
using System.IO;
using Log = FUtility.Log;

namespace Moonlet.Loaders
{
	public class ModZoneTypeLoader : BaseLoader
	{
		public string ZoneTypesFolder => Path.Combine(path, data.DataPath, ZONETYPES);

		public string BackgroundsFolder => Path.Combine(path, data.AssetsPath, ZONETYPES);

		public ModZoneTypeLoader(KMod.Mod mod, MoonletData data) : base(mod, data)
		{
		}

		public List<ZoneTypeData> CollectZoneTypesFromYaml()
		{
			var path = ZoneTypesFolder;

			if (data.DebugLogging)
				Log.Info($"Attempting to load zoneTypes {path}");

			if (!Directory.Exists(path))
			{
				if (data.DebugLogging)
					Log.Info($"No zoneTypes data folder found.");

				return null;
			}

			var zones = new List<ZoneTypeData>();

			foreach (var file in Directory.GetFiles(path, "*.yaml"))
			{
				if (data.DebugLogging)
					Log.Info("Loading zonetypes file " + file);

				var newZones = FileUtil.Read<List<ZoneTypeData>>(file);

				if (newZones != null)
					zones.AddRange(newZones);
			}

			foreach (var zone in zones)
				zone.texturesFolder = BackgroundsFolder;

			return zones;
		}
	}
}
