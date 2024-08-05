using System.Collections.Generic;
using UnityEngine;

namespace DecorPackB.Content.Scripts
{
	public class TextureLoader
	{
		private const string ASSET_BUNDLE = "decorpackii_crossplatform_assets";
		public static readonly string baseFolder = "assets/tiles";

		public static Dictionary<string, Texture2D> textureRegistry = [];

		public static void AddCustomTileAtlas(BuildingDef def, string name, string referenceAtlas = "tiles_metal")
		{
			var reference = Assets.GetTextureAtlas(referenceAtlas);

			// base
			def.BlockTileAtlas = GetCustomAtlas($"{name}_tiles", reference);

			// place
			def.BlockTilePlaceAtlas = GetCustomAtlas($"{name}_tiles_place", reference);

			FUtility.Assets.SaveImage(def.BlockTileAtlas.texture, Utils.ModPath + "/main.png");
			FUtility.Assets.SaveImage(def.BlockTilePlaceAtlas.texture, Utils.ModPath + "/place.png");
		}

		public static TextureAtlas GetCustomAtlas(string fileName, TextureAtlas tileAtlas)
		{
			Texture2D tex;

			if (textureRegistry.TryGetValue(fileName, out var texture))
				tex = texture;
			else
			{
				var bundle = FAssets.LoadAssetBundle(ASSET_BUNDLE);

				foreach (var asset in bundle.GetAllAssetNames())
					Log.Debug("Asset: " + asset);

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

		public static void AddCustomTileTops(BuildingDef def, string name, string decorInfo = "tiles_glass_tops_decor_info", string existingPlaceID = null)
		{
			var info = Object.Instantiate(Assets.GetBlockTileDecorInfo(decorInfo));

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
				def.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo(existingPlaceID);
		}
	}
}
