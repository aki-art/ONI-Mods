using Moonlet.TemplateLoaders.WorldgenLoaders;
using Moonlet.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static GroundMasks;
using static ProcGen.SubWorld;

namespace Moonlet.Loaders
{
	public class ZoneTypesLoader : TemplatesLoader<ZoneTypeLoader>
	{
		public static int LAST_INDEX = 16; // the number of textures in the TextureArray2D in the zone bg-s asset
		public static Dictionary<string, Texture2DArray> preloadedBgs;

		public List<string> zoneTypeCache;
		public int vanillaZoneTypesCount;

		public int GetCount() => loaders.Count;

		public ZoneTypesLoader(string path) : base(path)
		{
			//LAST_INDEX = DlcManager.IsContentSubscribed(DlcManager.DLC2_ID) ? 18 : 16;

			zoneTypeCache = [];
			vanillaZoneTypesCount = Enum.GetValues(typeof(ZoneType)).Length;

			/*	var tempRenderData = new SubworldZoneRenderData();
				LAST_INDEX = tempRenderData.zoneTextureArrayIndices.Length;*/
		}

		public void WriteCache()
		{
			for (int zoneIndex = 0; zoneIndex < World.Instance.zoneRenderData.zoneColours.Length; zoneIndex++)
			{
				try
				{
					var zoneType = (ZoneType)zoneIndex;
					var zoneName = zoneType.ToString();

					if (zoneTypeCache.Count >= zoneIndex)
					{
						zoneTypeCache.Add(zoneName);
					}
					else
					{
						if (zoneTypeCache[zoneIndex] != zoneName)
							Log.Debug($"Desynced zone IDs (╯‵□′)╯︵┻━┻ , was expecting {zoneTypeCache[zoneIndex]}, but got {zoneName}");
					}
				}
				catch (Exception e)
				{
					Log.Warn(e);
				}

				var folder = FUtility.Utils.ConfigPath("Moonlet");
				var path = Path.Combine(folder, "zonetypecache.json");

				FileUtil.WriteYAML(path, zoneTypeCache);
			}
		}

		public override void LoadYamls<TemplateType>(MoonletMod mod, bool singleEntry)
		{
			base.LoadYamls<TemplateType>(mod, singleEntry);

			if (loaders.Count > 0)
				OptionalPatches.requests |= OptionalPatches.PatchRequests.Enums;

			ModAPI.OnZoneTypeSet?.Invoke(ModAPI.GetZoneTypes());
		}

		public void AddBorders(GroundRenderer renderer, BiomeMaskData[] biomeMasks)
		{
			foreach (var zone in loaders)
			{
				var index = (int)zone.type;

				BiomeMaskData reference = null;
				var id = zone.borderType.ToString().ToLowerInvariant();

				foreach (var data in biomeMasks)
				{
					if (data != null && data.name == id)
						reference = data;
				}

				if (reference == null)
				{
					Log.Warn("There is no border type with id " + id);
					return;
				}

				var newEntry = new BiomeMaskData(zone.template.Id)
				{
					tiles = reference.tiles
				};

				biomeMasks[index] = newEntry;
				biomeMasks[index].GenerateRotations();

				renderer.masks.Initialize();
			}
		}

		public void StitchBgTextures(TerrainBG terrainBg)
		{
			// TODO: fragile to textures that did not load
			var zonesWithBg = loaders.ToList();//.Where(z => z.template.texture != null).ToList();

			var srcArray = terrainBg.backgroundMaterial.GetTexture("images") as Texture2DArray;
			var extraDepth = zonesWithBg.Count;
			var startDepth = srcArray.depth;
			var newDepth = srcArray.depth + extraDepth;

			Log.Debug("array length is " + srcArray.depth);

			// make new array
			var newArray = new Texture2DArray(srcArray.width, srcArray.height, newDepth, srcArray.format, false);

			// copy existing textures over
			for (var i = 0; i < srcArray.depth; i++)
				Graphics.CopyTexture(srcArray, i, 0, newArray, i, 0);

			// insert new textures
			for (var i = 0; i < extraDepth; i++)
			{
				var zoneTex = zonesWithBg[i].texture;

				//Moonlet_ZoneTypeTracker.textureArrayIndices[zonesWithBg[i].type] = startDepth + i;

				if (zoneTex == null)
				{
					Log.Warn($"Could not set texture of {zonesWithBg[i].id}, texture was not loaded.", zonesWithBg[i].sourceMod);

					// pad to not ruin the further ones
					Graphics.CopyTexture(srcArray, 0, 0, newArray, i, 0);
				}
				else
				{
					Graphics.CopyTexture(src: zoneTex, zonesWithBg[i].TextureIndex, 0, newArray, startDepth + i, 0);
					Log.Debug($"added tex {zonesWithBg[i].id} to {startDepth + i}");
				}

				//zonesWithBg[i].texture = null;
				//Object.Destroy(zonesWithBg[i].texture);
			}

			newArray.Apply();

			terrainBg.backgroundMaterial.SetTexture("images", newArray);
		}
	}
}
