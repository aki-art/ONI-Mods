using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace DecorPackA.Buildings.MoodLamp
{
	public class MoodLampConfig : IBuildingConfig
	{
		public static string ID = Mod.PREFIX + "MoodLamp";

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				1,
				2,
				"dpi_desk_kanim",
				BUILDINGS.HITPOINTS.TIER2,
				BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER4,
				BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
				MATERIALS.TRANSPARENTS,
				BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				BuildLocationRule.OnFloor,
				new EffectorValues(Mod.Settings.MoodLamp.Decor.Amount, Mod.Settings.MoodLamp.Decor.Range),
				NOISE_POLLUTION.NONE);

			def.Floodable = false;
			def.Overheatable = false;
			def.AudioCategory = "Glass";
			def.BaseTimeUntilRepair = -1f;
			def.ViewMode = OverlayModes.Light.ID;
			def.DefaultAnimState = "slab";
			def.PermittedRotations = PermittedRotations.FlipH;

			def.RequiresPowerInput = true;
			def.ExhaustKilowattsWhenActive = Mathf.Max(0, Mod.Settings.MoodLamp.PowerUse.ExhaustKilowattsWhenActive);
			def.EnergyConsumptionWhenActive = Mathf.Max(0, Mod.Settings.MoodLamp.PowerUse.EnergyConsumptionWhenActive);
			def.SelfHeatKilowattsWhenActive = Mathf.Max(0, Mod.Settings.MoodLamp.PowerUse.SelfHeatKilowattsWhenActive);

			def.ForegroundLayer = Grid.SceneLayer.BuildingBack;

			def.DefaultAnimState = "variant_1_off";

			return def;
		}

		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
		{
			var lightShapePreview = go.AddComponent<LightShapePreview>();
			lightShapePreview.lux = Mod.Settings.MoodLamp.Lux.Amount;
			lightShapePreview.radius = Mod.Settings.MoodLamp.Lux.Range;
			lightShapePreview.offset = CellOffset.up;
			lightShapePreview.shape = LightShape.Circle;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.AddTag(RoomConstraints.ConstraintTags.LightSource);
			go.AddTag(RoomConstraints.ConstraintTags.Decor20);
			go.AddTag(ModAssets.Tags.noPaint);
			go.AddTag(GameTags.Decoration);
			go.AddOrGet<CopyBuildingSettings>().copyGroupTag = ID;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddOrGet<EnergyConsumer>();
			go.AddOrGet<LoopingSounds>();

			var light2d = go.AddOrGet<Light2D>();
			light2d.overlayColour = LIGHT2D.FLOORLAMP_OVERLAYCOLOR;
			light2d.Color = Color.white;
			light2d.Range = Mod.Settings.MoodLamp.Lux.Range;
			light2d.Lux = Mod.Settings.MoodLamp.Lux.Amount;
			light2d.shape = LightShape.Circle;
			light2d.Offset = new Vector2(0, 1f);
			light2d.drawOverlay = !Mod.Settings.MoodLamp.DisableLightRays;

			go.AddComponent<MoodLamp>().lampOffset = new(0, 0.6f, -0.01f);

			go.AddComponent<Hamis>();
			go.AddComponent<GlitterLight2D>();
			go.AddComponent<ShiftyLight2D>();
			go.AddComponent<TintableLamp>();
			go.AddComponent<RotatableLamp>();
			go.AddComponent<BigBird>();
			go.AddComponent<SimpleParticleLamp>();
			go.AddComponent<ScatterLightLamp>();

			if (types != null)
			{
				Log.Info("Registering add-on components to Moodlamps:");
				foreach (var type in types)
				{
					Log.Info($"\t- {type.Name}");
					go.AddComponent(type);
				}
			}

			go.AddOrGetDef<PoweredController.Def>();
		}

		private static List<Type> types;

		public static void RegisterType(Type type)
		{
			types ??= new();
			types.Add(type);
		}
	}
}
