using Moonlet.TemplateLoaders.WorldgenLoaders;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GroundMasks;

namespace Moonlet.Loaders
{
	public class ZoneTypesLoader : TemplatesLoader<ZoneTypeLoader>
	{
		public static int LAST_INDEX = 21; /// <see cref="SubworldZoneRenderData.zoneTextureArrayIndices", but i need to hardcode because this array is filled way too late otherwise/>
		public static Dictionary<string, Texture2DArray> preloadedBgs;

		public int GetCount() => loaders.Count;

		public ZoneTypesLoader(string path) : base(path)
		{
			/*	var tempRenderData = new SubworldZoneRenderData();
				LAST_INDEX = tempRenderData.zoneTextureArrayIndices.Length;*/
		}

		public override void LoadYamls<TemplateType>(MoonletMod mod, bool singleEntry)
		{
			base.LoadYamls<TemplateType>(mod, singleEntry);

			if (loaders.Count > 0)
				OptionalPatches.requests |= OptionalPatches.PatchRequests.Enums;
		}

		public void AddBorders(GroundRenderer renderer, BiomeMaskData[] biomeMasks)
		{
			foreach (var zone in loaders)
			{
				var index = (int)zone.type;

				BiomeMaskData reference = null;
				var id = zone.borderType.ToString().ToLowerInvariant();

				foreach (var data in biomeMasks)
					Log.Debug(data?.name);

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

				renderer.masks.Regenerate();
			}
		}

		public void StitchBgTextures(TerrainBG terrainBg)
		{
			var zonesWithBg = loaders.Where(z => z.texture != null).ToList();

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
				Log.Debug($"adding new zonetype bg: {i} {zonesWithBg[i].id}, {zonesWithBg[i].TextureIndex}");
				var zoneTex = zonesWithBg[i].texture;

				if (zoneTex == null)
					Log.Warn($"Could not set texture of {zonesWithBg[i].id}, texture was not loaded.", zonesWithBg[i].sourceMod);
				else
					Graphics.CopyTexture(src: zoneTex, zonesWithBg[i].TextureIndex, 0, newArray, startDepth + i, 0);

				//zonesWithBg[i].texture = null;
				//Object.Destroy(zonesWithBg[i].texture);
			}

			newArray.Apply();

			terrainBg.backgroundMaterial.SetTexture("images", newArray);
		}
	}
}
