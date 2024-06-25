using FUtility;
using Moonlet.Scripts;
using Moonlet.Templates;
using Moonlet.Utils;
using System.IO;

namespace Moonlet.TemplateLoaders
{
	public class TileLoader(TileTemplate template, string sourceMod) : BuildingLoaderBase<TileTemplate>(template, sourceMod)
	{
		public override void CreateAndRegister()
		{
			var def = ConfigureDef();

			var config = new TileBuildingConfig()
			{
				def = def,
				skipLoading = false,
				strengthMultiplier = template.Strength.CalculateOrDefault(1.0f),
				speedMultiplier = template.WalkSpeedMultiplier.CalculateOrDefault(1.0f),
				// TODO: airflow type shenanigans
			};

			def.IsFoundation = true;
			def.isKAnimTile = true;
			def.PermittedRotations = PermittedRotations.Unrotatable;
			def.Floodable = false;
			def.Entombable = false;
			def.Overheatable = false;
			def.UseStructureTemperature = false;
			def.BaseTimeUntilRepair = -1f;
			def.SceneLayer = template.Transparent ? Grid.SceneLayer.GlassTile : Grid.SceneLayer.TileMain;
			def.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
			def.isKAnimTile = true;
			def.BlockTileMaterial = Assets.GetMaterial("tiles_solid");

			LoadTextures(def, MoonletMods.Instance.GetAssetsPath(sourceMod, "tiles"));

			if (!template.OverlayMode.IsNullOrWhiteSpace())
				def.ViewMode = template.OverlayMode;

			RegisterBuilding(config, def, template);
			AddToTech();
			AddToMenu();
		}

		private void LoadTextures(BuildingDef def, string texturesPath)
		{
			var reference = Assets.GetTextureAtlas("tiles_metal");

			// basic texture
			string lowerCasedId = template.Id.ToLowerInvariant();
			var tile = template.Texture.IsNullOrWhiteSpace() ? $"{lowerCasedId}" : template.Texture;
			var path = Path.Combine(texturesPath, tile);

			def.BlockTileAtlas = FAssets.GetCustomAtlas(tile, texturesPath, reference);

			if (!template.SpecularTexture.IsNullOrWhiteSpace())
			{
				def.BlockTileShineAtlas = FAssets.GetCustomAtlas(template.SpecularTexture, texturesPath, reference);
			}
			else if (template.Shiny)
			{
				def.BlockTileShineAtlas = FAssets.GetCustomAtlas($"{lowerCasedId}_specular", texturesPath, reference);
			}

			var tops = template.TopsTexture.IsNullOrWhiteSpace() ? $"{lowerCasedId}_tops" : template.TopsTexture;
			var topsPlace = template.TopsPlaceTexture.IsNullOrWhiteSpace() ? $"{lowerCasedId}_tops_place" : template.TopsPlaceTexture;
			var topsSpec = template.TopsSpecularTexture.IsNullOrWhiteSpace() ? $"{lowerCasedId}_tops_specular" : template.TopsSpecularTexture; // TODO

			EntityUtil.AddCustomTileTops(
				def,
				lowerCasedId,
				topsPlace,
				topsSpec,
				texturesPath,
				!template.TopsSpecularTexture.IsNullOrWhiteSpace(),
				template.TopsLayout);
		}
	}
}
