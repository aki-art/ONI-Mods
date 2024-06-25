using UnityEngine;

namespace Moonlet.Scripts
{
	namespace Moonlet.Entities
	{
		public class GenericBuildingConfig : IBuildingConfig
		{
			public BuildingDef def;
			public bool skipLoading = true; // used to skip the default unconfigured template

			public override BuildingDef CreateBuildingDef() => def;

			public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
			{
				base.ConfigureBuildingTemplate(go, prefab_tag);
			}

			public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
			{
				base.DoPostConfigurePreview(def, go);
			}

			public override void DoPostConfigureUnderConstruction(GameObject go)
			{
				base.DoPostConfigureUnderConstruction(go);
			}

			public override void DoPostConfigureComplete(GameObject go)
			{
			}
		}
	}
}
