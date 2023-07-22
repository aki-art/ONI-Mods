using FUtility;
using Moonlet.Content.Scripts;
using Moonlet.Loaders;
using UnityEngine;

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
			go.AddOrGet<MoonletEntityComponent>();

			var id = go.PrefabID().ToString();

			Log.Debuglog("post process " + id);

			foreach (var mod in Mod.modLoaders)
			{
				if (mod.entitiesLoader.buildings == null)
					continue;

				if (mod.entitiesLoader.buildings.TryGetValue(id, out var data))
				{
					ModEntitiesLoader.ProcessComponents(data, go);
					ModEntitiesLoader.ProcessCommands(data, go);
				}
			}
		}
	}
}
