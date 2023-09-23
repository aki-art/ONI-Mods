using System.Linq;
using TUNING;
using UnityEngine;

namespace SoftLockBegone.Content
{
	public class PlaceHolderBuildingConfig : IBuildingConfig
	{
		public const string DEFAULT_ID = "SoftLockBegone_PlaceHolderBuilding";
		public bool skipLoading = true; // used to skip the default unconfigured template
		public string ID;
		internal float mass;
		internal int materialCount;

		public override BuildingDef CreateBuildingDef()
		{
			materialCount = Mathf.Max(1, materialCount);

			var materials = Enumerable.Repeat("BuildableAny", materialCount).ToArray();
			var masses = Enumerable.Repeat(mass, materialCount).ToArray();

			var def = BuildingTemplates.CreateBuildingDef(
				ID ?? DEFAULT_ID,
				1,
				1,
				"walls_basic_pink_flamingo_kanim", // TODO: something not protected
				100,
				1,
				masses,
				materials,
				BUILDINGS.MELTING_POINT_KELVIN.TIER4,
				BuildLocationRule.Anywhere,
				default,
				default);

			def.Breakable = false;
			def.Overheatable = false;
			def.Floodable = false;
			def.Entombable = false;

			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			base.ConfigureBuildingTemplate(go, prefab_tag);
			go.AddComponent<Storage>();
			go.AddComponent<Storage>();
			go.AddComponent<Storage>();
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
		}
	}
}
