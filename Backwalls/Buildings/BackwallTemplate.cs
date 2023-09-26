using FUtility;
using TUNING;
using UnityEngine;
using static Backwalls.Settings.Config;

namespace Backwalls.Buildings
{
	public static class BackwallTemplate
	{
		public static BuildingDef CreateDef(string ID, string anim, WallConfig config, float workTime, bool halfDecor)
		{
			ValidateConfig(ID, config);

			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				1,
				1,
				anim,
				BUILDINGS.HITPOINTS.TIER1,
				workTime,
				config.ConstructionMass,
				config.ConstructionMaterials,
				BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				BuildLocationRule.Anywhere,
				new EffectorValues(halfDecor ? Mathf.CeilToInt(config.Decor.Amount / 2f) : config.Decor.Amount, config.Decor.Range),
				NOISE_POLLUTION.NONE);

			def.ObjectLayer = ObjectLayer.Backwall;
			def.SceneLayer = Grid.SceneLayer.Backwall;

			def.Floodable = false;
			def.Breakable = false;
			def.Entombable = false;
			def.Overheatable = false;

			def.AudioCategory = CONSTS.AUDIO_CATEGORY.PLASTIC;
			def.AudioSize = AUDIO.SIZE.SMALL;

			def.BaseTimeUntilRepair = -1f;

			return def;
		}

		private static void ValidateConfig(string ID, WallConfig config)
		{
			if (config.ConstructionMass == null || config.ConstructionMass.Length == 0)
			{
				Log.Warning($"Incorrect configuration of {ID}, no mass defined.");
				config.ConstructionMass = new[] { 10f };
			}

			if (config.ConstructionMaterials == null || config.ConstructionMaterials.Length == 0)
			{
				Log.Warning($"Incorrect configuration of {ID}, no materials defined.");
				config.ConstructionMaterials = MATERIALS.ALL_MINERALS;
			}

			if (config.ConstructionMass.Length != config.ConstructionMaterials.Length)
			{
				Log.Warning($"Incorrect configuration of {ID}, mismatched mass and materials. " +
					$"Make sure the Mass and Materials lists are the same counts.");
				config.ConstructionMass = new[] { config.ConstructionMass[0] };
				config.ConstructionMaterials = new[] { config.ConstructionMaterials[0] };
			}
		}
	}
}
