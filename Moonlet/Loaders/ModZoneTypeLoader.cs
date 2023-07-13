using HarmonyLib;
using Klei;
using Moonlet.ZoneTypes;
using System;
using System.IO;
using System.Linq;
using UnityEngine;
using static ProcGen.SubWorld;
using Log = FUtility.Log;

namespace Moonlet.Loaders
{
	public class ModZoneTypeLoader : BaseLoader
	{
		public ZoneTypeData[] zones;

		public string ZoneTypesFolder => Path.Combine(path, data.DataPath, ZONETYPES);

		public string BackgroundsFolder => Path.Combine(path, data.AssetsPath, ZONETYPES);

		public ModZoneTypeLoader(KMod.Mod mod, MoonletData data) : base(mod, data)
		{
			CollectZoneTypesFromYaml();

			if (zones == null)
				return;

			foreach (var zone in zones)
			{
				zone.type = ZoneTypeUtil.Register(zone);
				zone.color32 = Util.ColorFromHex(zone.color);

				if (zone.border.IsNullOrWhiteSpace())
					zone.borderType = ZoneType.Sandstone;
				else if (Enum.TryParse<ZoneType>(zone.border, out var t))
					zone.borderType = t;
				else
				{
					Log.Warning($"Invalid border type for zone type {zone.id}. Possible values: " + Enum.GetNames(typeof(ZoneType)).Join());
					zone.borderType = ZoneType.Sandstone;
				}

				if (!zone.background.IsNullOrWhiteSpace())
					zone.texture = LoadBackground(zone);
			}
		}

		private Texture2DArray LoadBackground(ZoneTypeData zone)
		{
			var path = Path.Combine(BackgroundsFolder, zone.background + ".dds");

			if (Directory.Exists(path))
			{
				Log.Warning("Trying to load zone types, expecting a texture file at " + path);
				return null;
			}

			var texture = LoadTexture(path);

			if (texture == null)
			{
				Log.Warning("Could not load texture " + path);
				return null;
			}

			if (texture.width != 1024 || texture.height != 1024)
			{
				Log.Warning($"(debug) {zone.id} texture is not the recommended size. (it is {texture.width}x{texture.height}, recommended is 1024x1024)");
				return null;
			}

			return texture;
		}

		public static Texture2DArray LoadTexture(string path, bool warnIfFailed = true)
		{
			Texture2DArray texture = null;

			if (File.Exists(path))
			{
				var data = FUtility.Assets.TryReadFile(path);
				texture = new Texture2DArray(1024, 1024, 1, TextureFormat.DXT1, false);
				texture.SetPixelData(data, 0, 0);
			}
			else if (warnIfFailed)
			{
				Log.Warning($"Could not load texture at path {path}.");
			}

			return texture;
		}

		private void CollectZoneTypesFromYaml()
		{
			var path = ZoneTypesFolder;

			if (data.DebugLogging)
				Log.Info($"Attempting to load zoneTypes {path}");

			if (!Directory.Exists(path))
			{
				if (data.DebugLogging)
					Log.Info($"No zoneTypes data folder found.");

				return;
			}

			var errors = ListPool<YamlIO.Error, ElementLoader>.Allocate();

			foreach (var file in Directory.GetFiles(path, "*.yaml"))
			{
				if (data.DebugLogging)
					Log.Info("Loading element file " + file);

				zones = YamlIO.LoadFile<ZoneTypeData[]>(file, (error, warning) => errors.Add(error));

				if (Global.Instance != null && Global.Instance.modManager != null)
					Global.Instance.modManager.HandleErrors(errors);

				if (zones != null && zones.Length > 0)
					PatchTracker.loadsZoneTypes = true;

				errors.Recycle();
			}
		}
	}
}