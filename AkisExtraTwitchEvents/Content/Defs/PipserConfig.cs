using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Defs
{
	[EntityConfigOrder(999)] // after all critters
	public class PipserConfig : IEntityConfig
	{
		public const string ID = "AkisExtraTwitchEvents_Pipser";

		public GameObject CreatePrefab()
		{
			var placedEntity = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.CREATURES.SPECIES.GEYSER.AKISEXTRATWITCHEVENTS_PIPSER.NAME,
				STRINGS.CREATURES.SPECIES.GEYSER.AKISEXTRATWITCHEVENTS_PIPSER.DESC,
				100f,
				Assets.GetAnim("farmtile_kanim"),
				"idle",
				Grid.SceneLayer.Building,
				3,
				3,
				TUNING.DECOR.NONE);

			placedEntity.AddOrGet<Prioritizable>();
			placedEntity.AddOrGet<Uncoverable>();

			placedEntity.AddOrGet<OccupyArea>().objectLayers = [ObjectLayer.Building];

			//placedEntity.AddOrGet<Geyser>().outputOffset = new Vector2I(0, 1);
			//placedEntity.AddOrGet<GeyserConfigurator>().presetType = presetType;

			var pipser = placedEntity.AddOrGet<Pipser>();
			pipser.minDormancy = 5f * CONSTS.CYCLE_LENGTH;
			pipser.maxDormancy = 15f * CONSTS.CYCLE_LENGTH;
			pipser.minPipPerEruption = 8;
			pipser.maxPipPerEruption = 15;
			pipser.timeBetweenPips = 0.033f * 21f;
			pipser.spawnOffset = new Vector3(0, 2f);
			pipser.pips =
			[
				BabySquirrelConfig.ID,
				BabySquirrelHugConfig.ID
			];

			if (Mod.isBeachedHere)
				pipser.pips.Add("Beached_BabyMerpip");

			// todo: pip morphs

			var studyable = placedEntity.AddOrGet<Studyable>();
			studyable.meterTrackerSymbol = "geotracker_target";
			studyable.meterAnim = "tracker";

			placedEntity.AddOrGet<LoopingSounds>();

			placedEntity.AddOrGet<UserNameable>();


			return placedEntity;
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
