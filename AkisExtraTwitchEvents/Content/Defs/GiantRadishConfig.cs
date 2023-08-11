using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Defs
{
	public class GiantRadishConfig : IEntityConfig
	{
		public const string ID = "AkisExtraTwitchEvents_GiantRadish";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateLooseEntity(
				ID,
				STRINGS.MISC.AKISEXTRATWITCHEVENTS_GIANTRADISH.NAME,
				STRINGS.MISC.AKISEXTRATWITCHEVENTS_GIANTRADISH.DESC,
				200f,
				true,
				Assets.GetAnim("aete_radish_kanim"),
				"falling",
				Grid.SceneLayer.Creatures,
				EntityTemplates.CollisionShape.RECTANGLE,
				3,
				6,
				false);

			var storage = prefab.AddComponent<Storage>();
			storage.capacityKg = Mod.Settings.GiantRadish_Kcal / TFoodInfos.RADISH_KCAL_PER_KG;
			storage.allowItemRemoval = true;
			storage.gunTargetOffset = new Vector2(0, -2);

			var raddish = prefab.AddComponent<Radish>();
			raddish.consumedStages = 5;
			raddish.raddishStorage = storage;

			prefab.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(-1, -3, 1, 2));

			// needs pickupable. dont reenable until mesh tile glitch is fixed
			//prefab.AddOrGet<Clearable>().isClearable = false;

			prefab.AddTag(ONITwitchLib.ExtraTags.OniTwitchSurpriseBoxForceEnabled);

			prefab.AddOrGet<KBatchedAnimController>().isMovable = true;

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst)
		{
			if (DlcManager.FeatureRadiationEnabled())
			{
				var emitter = inst.AddOrGet<RadiationEmitter>();
				emitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
				emitter.radiusProportionalToRads = false;
				emitter.emitRadiusX = 6;
				emitter.emitRadiusY = 6;
				emitter.emitRads = 800f;
				emitter.emissionOffset = new Vector3(0, -2);
			}
		}

		public void OnSpawn(GameObject inst) { }
	}
}
