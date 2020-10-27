using FUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TUNING;
using UnityEngine;

namespace TransparentAluminium
{
	class LogicLightSensorConfig : IBuildingConfig, IModdedBuilding
	{
		public const string ID = "TAT_LogicLightSensor";

		public MBInfo Info => new MBInfo(ID, "Base");

		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(
				id: ID,
				width: 1,
				height: 1,
				anim: "switchliquidpressure_kanim",
				hitpoints: BUILDINGS.HITPOINTS.TIER1,
				construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2,
				construction_mass: new float[] { BUILDINGS.CONSTRUCTION_MASS_KG.TIER4[0] },
				construction_materials: new string[] { "TransparentAluminum" },
				melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER3,
				build_location_rule: BuildLocationRule.Tile,
				decor: BUILDINGS.DECOR.PENALTY.TIER2,
				noise: NOISE_POLLUTION.NONE
				);

			buildingDef.Overheatable = false;
			buildingDef.Floodable = false;
			buildingDef.Entombable = false;
			buildingDef.ViewMode = OverlayModes.Logic.ID;
			buildingDef.AudioCategory = "Metal";
			buildingDef.SceneLayer = Grid.SceneLayer.Building;
			buildingDef.AlwaysOperational = true;
			buildingDef.LogicOutputPorts = new List<LogicPorts.Port>()
			{
			  LogicPorts.Port.OutputPort(
				  LogicSwitch.PORT_ID, 
				  new CellOffset(0, 0), 
				  global::STRINGS.BUILDINGS.PREFABS.LOGICPRESSURESENSORLIQUID.LOGIC_PORT, 
				  global::STRINGS.BUILDINGS.PREFABS.LOGICPRESSURESENSORLIQUID.LOGIC_PORT_ACTIVE, 
				  global::STRINGS.BUILDINGS.PREFABS.LOGICPRESSURESENSORLIQUID.LOGIC_PORT_INACTIVE, 
				  true)
			};

			GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, ID);
			return buildingDef;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			var lightSensor = go.AddOrGet<LogicLightSensor>();
			lightSensor.rangeMin = 0.0f;
			lightSensor.rangeMax = 100000f;
			lightSensor.Threshold = 1000f;
			lightSensor.ActivateAboveThreshold = false;
			lightSensor.manuallyControlled = false;

			go.AddTag(GameTags.OverlayInFrontOfConduits);
		}
	}
}
