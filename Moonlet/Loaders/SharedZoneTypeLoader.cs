using FUtility;
using HarmonyLib;
using Moonlet.ZoneTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static ProcGen.SubWorld;

namespace Moonlet.Loaders
{
	public class SharedZoneTypeLoader
	{
		public List<ZoneTypeData> zones;

		public void PreLoad()
		{
			zones = new List<ZoneTypeData>();

			foreach (var mod in Mod.modLoaders)
			{
				var newZones = mod.zoneTypesLoader.CollectZoneTypesFromYaml();

				if (newZones == null)
					continue;

				foreach (var zone in newZones)
				{
					var count = zones.Count(z => z.Id == zone.Id);
					if (count > 1)
						zone.Id += $"_{count - 1}";

					zones.Add(zone);
				}
			}

			if (zones == null)
				return;

			foreach (var zone in zones)
			{
				zone.type = ZoneTypeUtil.Register(zone);
				zone.color32 = Util.ColorFromHex(zone.Color);

				if (zone.Border.IsNullOrWhiteSpace())
					zone.borderType = ZoneType.Sandstone;
				else if (Enum.TryParse<ZoneType>(zone.Border, out var t))
					zone.borderType = t;
				else
				{
					Log.Warning($"Invalid border type for zone type {zone.Id}. Possible values: " + Enum.GetNames(typeof(ZoneType)).Join());
					zone.borderType = ZoneType.Sandstone;
				}

				if (!zone.Background.IsNullOrWhiteSpace())
					zone.texture = LoadBackground(zone);
			}
		}

		public static Texture2D LoadTexture(string path, TextureFormat format)
		{
			Texture2D texture = null;

			if (File.Exists(path))
			{
				byte[] data = FUtility.Assets.TryReadFile(path);
				texture = new Texture2D(1, 1, format, false);
				texture.LoadImage(data);
			}

			return texture;
		}

		private Texture2DArray LoadBackground2D(ZoneTypeData zone)
		{
			var path = Path.Combine(zone.texturesFolder, zone.Background + ".png");

			if (Directory.Exists(path))
			{
				Log.Warning("Trying to load zone types, expecting a texture file at " + path);
				return null;
			}

			var texture = LoadTexture(path, TextureFormat.RGB24);

			if (texture == null)
			{
				Log.Warning("Could not load texture " + path);
				return null;
			}

			if (texture.width != 1024 || texture.height != 1024)
			{
				Log.Warning($"(debug) {zone.Id} texture is not the recommended size. (it is {texture.width}x{texture.height}, recommended is 1024x1024)");
				return null;
			}

			texture.Compress(false);

			Log.Debuglog($"FORMAT: {texture.format}");

			if(texture.format != TextureFormat.DXT1)
			{
				var temp = new Texture2D(texture.width, texture.height, TextureFormat.DXT1, false);
				Graphics.ConvertTexture(texture, temp);

				texture = temp;
			}

			Log.Debuglog(texture.format);

			var textureArr = new Texture2DArray(1024, 1024, 1, TextureFormat.DXT1, false);
			textureArr.SetPixelData(texture.GetPixelData<byte>(0), 0, 0);
			textureArr.Apply();

			return textureArr;
		}

		private Texture2DArray LoadBackground(ZoneTypeData zone)
		{
			var path = Path.Combine(zone.texturesFolder, zone.Background + ".dds");

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
				Log.Warning($"(debug) {zone.Id} texture is not the recommended size. (it is {texture.width}x{texture.height}, recommended is 1024x1024)");
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
				var tex2D = LoadTextureDXT(data, TextureFormat.DXT1);

				texture = new Texture2DArray(1024, 1024, 1, TextureFormat.DXT1, false);
				texture.SetPixelData(tex2D.GetPixelData<byte>(0), 0, 0);
				texture.Apply();
			}
			else if (warnIfFailed)
			{
				Log.Warning($"Could not load texture at path {path}.");
			}

			return texture;
		}

		// credit: https://discussions.unity.com/t/can-you-load-dds-textures-during-runtime/84192/2

		public static Texture2D LoadTextureDXT(byte[] ddsBytes, TextureFormat textureFormat)
		{
			if (textureFormat != TextureFormat.DXT1 && textureFormat != TextureFormat.DXT5)
				throw new Exception("Invalid TextureFormat. Only DXT1 and DXT5 formats are supported by this method.");

			byte ddsSizeCheck = ddsBytes[4];
			if (ddsSizeCheck != 124)
				throw new Exception("Invalid DDS DXTn texture. Unable to read");  //this header byte should be 124 for DDS image files

			int height = ddsBytes[13] * 256 + ddsBytes[12];
			int width = ddsBytes[17] * 256 + ddsBytes[16];

			int DDS_HEADER_SIZE = 128;
			byte[] dxtBytes = new byte[ddsBytes.Length - DDS_HEADER_SIZE];
			Buffer.BlockCopy(ddsBytes, DDS_HEADER_SIZE, dxtBytes, 0, ddsBytes.Length - DDS_HEADER_SIZE);

			Texture2D texture = new Texture2D(width, height, textureFormat, false);
			texture.LoadRawTextureData(dxtBytes);
			texture.Apply();

			return (texture);
		}


		public void StitchBgTextures(TerrainBG terrainBg)
		{
			var zonesWithBg = zones.Where(z => z.texture != null).ToList();

			var srcArray = terrainBg.backgroundMaterial.GetTexture("images") as Texture2DArray;
			var extraDepth = zonesWithBg.Count;
			var startDepth = srcArray.depth;
			var newDepth = srcArray.depth + extraDepth;

			// make new array
			var newArray = new Texture2DArray(srcArray.width, srcArray.height, newDepth, srcArray.format, false);

			// copy existing textures over
			for (var i = 0; i < srcArray.depth; i++)
				Graphics.CopyTexture(srcArray, i, 0, newArray, i, 0);

			// insert new textures
			for (var i = 0; i < extraDepth; i++)
			{
				var zoneTex = zonesWithBg[i].texture;
				Graphics.CopyTexture(src: zoneTex, 0, 0, newArray, startDepth + i, 0);

				//zonesWithBg[i].texture = null;
				UnityEngine.Object.Destroy(zonesWithBg[i].texture);
			}

			newArray.Apply();

			terrainBg.backgroundMaterial.SetTexture("images", newArray);
		}
	}
}
