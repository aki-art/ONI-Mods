using Backwalls.Cmps;
using TUNING;
using UnityEngine;

namespace Backwalls.Buildings
{
	public class SealedBackwallConfig : IBuildingConfig
	{
		public const string ID = "Backwall_SealedBackwall";

		public override BuildingDef CreateBuildingDef()
		{
			return BackwallTemplate.CreateDef(
				ID, 
				"sealed_backwall_kanim", 
				Mod.Settings.SealedWall,
				BUILDINGS.WORK_TIME_SECONDS.SHORT_WORK_TIME,
				true);
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
			go.AddOrGet<CopyBuildingSettings>().copyGroupTag = ID;
			go.AddComponent<ZoneTile>();
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.RemoveLoopingSounds(go);
			go.AddComponent<BackwallLink>();
			go.AddComponent<Backwall>();
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			base.DoPostConfigureUnderConstruction(go);
			go.AddComponent<BackwallUnderConstruction>();
		}
	}
}
