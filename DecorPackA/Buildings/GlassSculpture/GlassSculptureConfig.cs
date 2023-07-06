using TUNING;
using UnityEngine;

namespace DecorPackA.Buildings.GlassSculpture
{
	internal class GlassSculptureConfig : IBuildingConfig
	{
		public static string ID = Mod.PREFIX + "GlassSculpture";

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
			   ID,
			   2,
			   2,
			   "decorpacka_glasssculpture_default_kanim",
			   BUILDINGS.HITPOINTS.TIER2,
			   BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER4,
			   BUILDINGS.CONSTRUCTION_MASS_KG.TIER4,
			   MATERIALS.TRANSPARENTS,
			   BUILDINGS.MELTING_POINT_KELVIN.TIER1,
			   BuildLocationRule.OnFloor,
			   new EffectorValues(Mod.Settings.GlassSculpture.BaseDecor.Amount, Mod.Settings.GlassSculpture.BaseDecor.Range),
			   NOISE_POLLUTION.NONE
		   );

			def.Floodable = false;
			def.Overheatable = false;
			def.AudioCategory = "Glass";
			def.BaseTimeUntilRepair = -1f;
			def.ViewMode = OverlayModes.Decor.ID;
			def.DefaultAnimState = "slab";
			def.PermittedRotations = PermittedRotations.FlipH;

			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.AddOrGet<BuildingComplete>().isArtable = true;
			go.AddTag(GameTags.Decoration);
			go.AddTag(ModAssets.Tags.noPaint);
			go.AddComponent<Fabulous>().offset = new Vector3(.5f, .5f, .4f);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddComponent<Sculpture>().defaultAnimName = "slab";
		}
	}
}
