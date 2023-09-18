using System.Collections.Generic;
using UnityEngine;

namespace DecorPackA.Buildings.StainedGlassTile
{
	public class TextureLoader
	{
		public static string baseFolder = "assets/tiles";

		public static Dictionary<string, Texture2D> textureRegistry = new();

		public static void AddCustomTileAtlas(BuildingDef def, string name, bool shiny = false, string referenceAtlas = "tiles_metal")
		{
			TextureAtlas reference = Assets.GetTextureAtlas(referenceAtlas);

			// base
			def.BlockTileAtlas = GetCustomAtlas($"{name}_tiles", reference);

			// place
			def.BlockTilePlaceAtlas = GetCustomAtlas($"{name}_tiles_place", reference);

			// specular
			if (shiny)
				def.BlockTileShineAtlas = GetCustomAtlas($"{name}_tiles_spec", reference);
		}

		public static TextureAtlas GetCustomAtlas(string fileName, TextureAtlas tileAtlas)
		{
			Texture2D tex;

			if (textureRegistry.TryGetValue(fileName, out var texture))
				tex = texture;
			else
			{
				var bundle = FUtility.FAssets.LoadAssetBundle("decorpacki_crossplatform_assets"); // if its already loaded it just grabs it
				tex = bundle.LoadAsset<Texture2D>(fileName);
			}

			if (tex == null)
				return null;

			var atlas = ScriptableObject.CreateInstance<TextureAtlas>();
			atlas.texture = tex;
			atlas.scaleFactor = tileAtlas.scaleFactor;
			atlas.items = tileAtlas.items;

			return atlas;
		}

		public static void AddCustomTileTops(BuildingDef def, string name, bool shiny = false, string decorInfo = "tiles_glass_tops_decor_info", string existingPlaceID = null, string existingSpecID = null)
		{
			var info = Object.Instantiate(global::Assets.GetBlockTileDecorInfo(decorInfo));

			// base
			if (info != null)
			{
				info.atlas = GetCustomAtlas($"{name}_tiles_tops", info.atlas);
				def.DecorBlockTileInfo = info;
			}

			// placement
			if (existingPlaceID.IsNullOrWhiteSpace())
			{
				var placeInfo = Object.Instantiate(Assets.GetBlockTileDecorInfo(decorInfo));
				placeInfo.atlas = GetCustomAtlas($"{name}_tiles_tops_place", placeInfo.atlas);
				def.DecorPlaceBlockTileInfo = placeInfo;
			}
			else
				def.DecorPlaceBlockTileInfo = global::Assets.GetBlockTileDecorInfo(existingPlaceID);

			// specular
			if (shiny)
			{
				string id = existingSpecID.IsNullOrWhiteSpace() ? $"{name}_tiles_tops_spec" : existingSpecID;
				info.atlasSpec = GetCustomAtlas(id, info.atlasSpec);
			}
		}
	}
}
