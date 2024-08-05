using DecorPackB.Content.Scripts;
using TUNING;
using UnityEngine;

namespace DecorPackB.Content.Defs.Buildings
{
	internal class MuseumLampConfig : IBuildingConfig
	{
		public const string ID = "DecorPackB_MuseumLamp";

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				1,
				1,
				"farmtile_kanim",
				BUILDINGS.HITPOINTS.TIER1,
				BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2,
				BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
				MATERIALS.RAW_METALS,
				BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				BuildLocationRule.OnFloor,
				DECOR.NONE,
				default);


			return def;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			var light = go.AddOrGet<Light2D>();
			light.Color = Color.white;
			light.Lux = 1000;
			light.shape = LightShape.Cone;
			light.Range = 10;
			light.Direction = Vector2.one;

			go.AddComponent<RotatableLamp>();
		}
	}
}
