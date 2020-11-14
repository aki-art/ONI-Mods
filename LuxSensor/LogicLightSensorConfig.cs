using FUtility;
using System.Collections.Generic;
using TUNING;
using UnityEngine;
using static LuxSensor.STRINGS.BUILDINGS.PREFABS.LS_LOGICLIGHTSENSOR;

namespace LuxSensor
{
	class LogicLightSensorConfig : IBuildingConfig, IModdedBuilding
	{
		public const string ID = "LS_LogicLightSensor";

		public MBInfo Info => new MBInfo(ID, "Automation", "GenericSensors");

		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(
				id: ID,
				width: 1,
				height: 1,
				anim: "luxsensor_kanim",
				hitpoints: BUILDINGS.HITPOINTS.TIER1,
				construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2,
				construction_mass: new float[] { BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0], BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0] },
				construction_materials: new string[] { "Transparent", "Metal" },
				melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER3,
				build_location_rule: BuildLocationRule.Tile,
				decor: BUILDINGS.DECOR.PENALTY.TIER2,
				noise: NOISE_POLLUTION.NONE);

			buildingDef.Overheatable = false;
			buildingDef.Floodable = false;
			buildingDef.Entombable = false;
			buildingDef.ViewMode = OverlayModes.Logic.ID;
			buildingDef.AudioCategory = "Metal";
			buildingDef.SceneLayer = Grid.SceneLayer.Building;
			buildingDef.AlwaysOperational = true;
			buildingDef.LogicOutputPorts = new List<LogicPorts.Port>()
			{
			  LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, CellOffset.none, LOGIC_PORT, LOGIC_PORT_ACTIVE,LOGIC_PORT_INACTIVE, true)
			};

			GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, ID);
			return buildingDef;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			/*			var lightSensor = go.AddOrGet<LogicLightSensor>();
			/*			
						lightSensor.rangeMin = 0;
						lightSensor.rangeMax = 80000;
						lightSensor.Threshold = 1000;
						lightSensor.ActivateAboveThreshold = true;
						lightSensor.manuallyControlled = false;*/
			var lightSensor = go.AddOrGet<SliderTest>();
			go.AddTag(GameTags.OverlayInFrontOfConduits);
		}
	}
}
