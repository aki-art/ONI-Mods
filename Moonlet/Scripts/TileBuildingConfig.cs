using FUtility;
using Moonlet.Scripts.Moonlet.Entities;
using Moonlet.Templates;
using Moonlet.Utils;
using System.IO;
using UnityEngine;

namespace Moonlet.Scripts
{
	public class TileBuildingConfig : GenericBuildingConfig
	{
		public TileTemplate tileTemplate; // generics dont play nice with IBuildingConfigs so we do a scuff

		public TileBuildingConfig() { }

		public TileBuildingConfig(bool skipLoading, BuildingTemplate template) : base(skipLoading, template)
		{
			tileTemplate = template as TileTemplate;
		}

		public string sourceMod;

		public override BuildingDef ConfigureDef(BuildingDef def)
		{
			BuildingTemplates.CreateFoundationTileDef(def);

			if (template is TileTemplate timeTemplate)
			{
				def.IsFoundation = true;
				def.isKAnimTile = true;
				def.PermittedRotations = PermittedRotations.Unrotatable;
				def.Floodable = false;
				def.Entombable = false;
				def.Overheatable = false;
				def.BlockTileIsTransparent = timeTemplate.Transparent;
				def.UseStructureTemperature = false;
				def.BaseTimeUntilRepair = -1f;
				def.SceneLayer = timeTemplate.Transparent ? Grid.SceneLayer.GlassTile : Grid.SceneLayer.TileMain;
				def.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
				def.isKAnimTile = true;
				def.DragBuild = true;
				def.AudioCategory = timeTemplate.AudioCategory;
				def.AudioSize = timeTemplate.AudioSize;

				if (!timeTemplate.OverlayMode.IsNullOrWhiteSpace())
					def.ViewMode = timeTemplate.OverlayMode;

				def.BlockTileMaterial = Assets.GetMaterial("tiles_solid");

				LoadTextures(ref def, MoonletMods.Instance.GetAssetsPath(sourceMod, "tiles"));

				if (def.BlockTilePlaceAtlas.texture == null)
					Log.Debug("place atlas texture is null");
			}

			return def;
		}

		private void LoadTextures(ref BuildingDef def, string texturesPath)
		{
			var reference = Assets.GetTextureAtlas("tiles_metal");

			// basic texture
			string lowerCasedId = tileTemplate.Id.ToLowerInvariant();
			var tile = tileTemplate.Texture.IsNullOrWhiteSpace() ? $"{lowerCasedId}.png" : tileTemplate.Texture;
			var placePath = tileTemplate.PlaceTexture.IsNullOrWhiteSpace() ? $"{lowerCasedId}_place.png" : tileTemplate.Texture;

			var mainTexturePath = Path.Combine(texturesPath, tile);

			def.BlockTileAtlas = FAssets.GetCustomAtlas(mainTexturePath, reference);

			var placeTexturePath = Path.Combine(texturesPath, placePath);
			def.BlockTilePlaceAtlas = FAssets.GetCustomAtlas(placeTexturePath, reference);

			if (tileTemplate.Shiny || !tileTemplate.SpecularTexture.IsNullOrWhiteSpace())
			{
				var specularFileName = tileTemplate.SpecularTexture ?? $"{lowerCasedId}_specular.png";
				def.BlockTileShineAtlas = FAssets.GetCustomAtlas(Path.Combine(texturesPath, specularFileName), reference);
			}

			var tops = tileTemplate.TopsTexture.IsNullOrWhiteSpace() ? $"{lowerCasedId}_tops" : tileTemplate.TopsTexture;
			var topsPlace = tileTemplate.TopsPlaceTexture.IsNullOrWhiteSpace() ? $"{lowerCasedId}_tops_place" : tileTemplate.TopsPlaceTexture;
			var topsSpec = tileTemplate.TopsSpecularTexture.IsNullOrWhiteSpace() ? $"{lowerCasedId}_tops_specular" : tileTemplate.TopsSpecularTexture; // TODO

			EntityUtil.AddCustomTileTops(
				def,
				tops,
				topsPlace,
				topsSpec,
				texturesPath,
				!tileTemplate.TopsSpecularTexture.IsNullOrWhiteSpace(),
				tileTemplate.TopsLayout);
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), go.PrefabID());

			var sco = go.AddOrGet<SimCellOccupier>();
			sco.doReplaceElement = tileTemplate.ReplaceSimCellOccupier;
			sco.movementSpeedMultiplier = tileTemplate.WalkSpeedMultiplier.CalculateOrDefault(1);
			sco.strengthMultiplier = tileTemplate.Strength.CalculateOrDefault(1);
			sco.setLiquidImpermeable = !tileTemplate.AllowLiquid;
			sco.setGasImpermeable = !tileTemplate.AllowAir;
			sco.setTransparent = tileTemplate.Transparent;
			sco.notifyOnMelt = true;

			go.AddOrGet<TileTemperature>();
			go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = Hash.SDBMLower($"{tileTemplate.Id.ToLowerInvariant()}_tiles_tops");
			go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.RemoveLoopingSounds(go);
			var kPrefabID = go.GetComponent<KPrefabID>();

			kPrefabID.AddTag(GameTags.FloorTiles);

			if (tileTemplate.Transparent)
				kPrefabID.AddTag(GameTags.Transparent);

			go.AddComponent<SimTemperatureTransfer>();
			go.AddComponent<ZoneTile>();
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			go.AddOrGet<KAnimGridTileVisualizer>();
		}
	}
}
