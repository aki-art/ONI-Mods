using DecorPackB.Content.Scripts;
using TUNING;
using UnityEngine;

namespace DecorPackB.Content.Defs.Buildings
{
	public class DefaultFloorLampConfig : IBuildingConfig
	{
		public string name = "Default";

		public const string SUFFIX = "_FloorLamp";

		public const string DEFAULT_ID = $"{Mod.PREFIX}Default{SUFFIX}";
		public static readonly int BlockTileConnectorID = Hash.SDBMLower("decorpackb_lamp_tops");

		public string ID => $"{Mod.PREFIX}{name}{SUFFIX}";


		public static EffectorValues decor;

		public override string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public override BuildingDef CreateBuildingDef()
		{
			var materials = new[]
			{
				ModTags.floorLampFrameMaterial.ToString(),
				ModTags.floorLampPaneMaterial.ToString()
			};

			var mass = new[]
			{
				150f,
				50f
			};

			var id = ID;
			var anim = id == DEFAULT_ID ? "dpii_floorlamp" : $"dpii_{name.ToLowerInvariant()}_floorlamp_tiles";

			var def = BuildingUtil.CreateTileDef(id, anim, mass, materials, decor, true);
			def.ShowInBuildMenu = true;
			def.BlockTileIsTransparent = true;
			def.SceneLayer = Grid.SceneLayer.GlassTile;

			if (id == DEFAULT_ID)
			{
				TextureLoader.AddCustomTileAtlas(def, $"goldamalgam");
				TextureLoader.AddCustomTileTops(def, $"goldamalgam", "tiles_metal_tops_decor_info", "tiles_glass_tops_decor_place_info");
			}
			else
			{
				TextureLoader.AddCustomTileAtlas(def, $"{name.ToLowerInvariant()}");
				TextureLoader.AddCustomTileTops(def, $"{name.ToLowerInvariant()}", "tiles_metal_tops_decor_info", "tiles_glass_tops_decor_place_info");
			}

			def.DecorBlockTileInfo.atlasSpec = null;

			if (id != DEFAULT_ID)
			{
				var element = ElementLoader.FindElementByName(name);
				var elementName = element != null ? element.tag.ProperNameStripLink() : "N/A";

				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{id.ToUpperInvariant()}.NAME", FloorLampFrames.GetFormattedName(elementName));
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{id.ToUpperInvariant()}.DESC", STRINGS.BUILDINGS.PREFABS.DECORPACKB_DEFAULT_FLOORLAMP.DESC);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{id.ToUpperInvariant()}.EFFECT", STRINGS.BUILDINGS.PREFABS.DECORPACKB_DEFAULT_FLOORLAMP.EFFECT);
			}

			def.RequiresPowerInput = true;
			def.EnergyConsumptionWhenActive = 8f;
			def.SelfHeatKilowattsWhenActive = 0.5f;
			def.ViewMode = OverlayModes.Light.ID;
			def.AudioCategory = AUDIO.CATEGORY.HOLLOW_METAL;

			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), tag);

			var simCellOccupier = go.AddOrGet<SimCellOccupier>();
			simCellOccupier.notifyOnMelt = true;
			simCellOccupier.setTransparent = true;
			simCellOccupier.movementSpeedMultiplier = 1.0f;

			go.AddOrGet<TileTemperature>();
			go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = BlockTileConnectorID;
			go.AddOrGet<BuildingHP>().destroyOnDamaged = true;

			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.LightSource);
		}

		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
		{
			var lightShapePreview = go.AddComponent<LightShapePreview>();
			lightShapePreview.lux = 300;
			lightShapePreview.radius = 2f;
			lightShapePreview.shape = LightShape.Circle;
			lightShapePreview.offset = CellOffset.none;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.RemoveLoopingSounds(go);

			if (go.TryGetComponent(out KPrefabID prefabID))
			{
				prefabID.AddTag(GameTags.FloorTiles);
				prefabID.AddTag(GameTags.Window);
				prefabID.AddTag(ModTags.floorLamp);
				prefabID.AddTag(ModTags.noPaint);
			}

			ConfigureKbac(go, "off");


			go.AddOrGet<EnergyConsumer>();
			go.AddOrGet<LoopingSounds>();

			var light2D = go.AddOrGet<Light2D>();
			light2D.Color = new Color(1.5f, 1.2f, 0.4f);
			light2D.Range = 2f;
			light2D.Direction = LIGHT2D.FLOORLAMP_DIRECTION;
			light2D.Offset = Vector2.zero;
			light2D.shape = LightShape.Circle;
			light2D.drawOverlay = false;

			go.AddComponent<FloorLamp>();

			go.AddOrGetDef<LightController.Def>();
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			go.AddOrGet<KAnimGridTileVisualizer>();
			ConfigureKbac(go, "off");
		}

		private static void ConfigureKbac(GameObject go, string initialAnim)
		{
			// manually adding this because being a tile it is skipped from being added by the game
			var kbac = go.AddOrGet<KBatchedAnimController>();
			kbac.AnimFiles = [Assets.GetAnim("dpii_floorlamppane_glass_kanim")];
			kbac.initialAnim = initialAnim;
			kbac.Offset = new Vector3(0, 1f);
			kbac.SetSceneLayer(Grid.SceneLayer.InteriorWall);
		}
	}
}
