using Backwalls.Cmps;
using UnityEngine;
using TUNING;

namespace Backwalls.Buildings
{
	public class DecorativeBackwallConfig : IBuildingConfig
	{
		public const string ID = "Backwall_DecorativeBackwall";

		public override BuildingDef CreateBuildingDef()
		{
			return BackwallTemplate.CreateDef(
				ID, 
				"decorative_backwall_kanim",
				Mod.Settings.DecorativeWall,
				BUILDINGS.WORK_TIME_SECONDS.VERYSHORT_WORK_TIME,
				false);
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
			go.AddOrGet<CopyBuildingSettings>().copyGroupTag = ID;
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
