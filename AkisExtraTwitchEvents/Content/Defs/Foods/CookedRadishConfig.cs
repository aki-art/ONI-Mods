using UnityEngine;

namespace Twitchery.Content.Defs.Foods
{
	public class CookedRadishConfig : IEntityConfig, IHasDlcRestrictions
	{
		public const string ID = "AkisExtraTwitchEvents_CookedRadish";

		public string[] GetAnyRequiredDlcIds() => null;

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateLooseEntity(
				ID,
				STRINGS.ITEMS.FOOD.AKISEXTRATWITCHEVENTS_COOKEDRADISH.NAME,
				STRINGS.ITEMS.FOOD.AKISEXTRATWITCHEVENTS_COOKEDRADISH.DESC,
				1f,
				true,
				Assets.GetAnim("aete_cookedradish_kanim"),
				"object",
				Grid.SceneLayer.Creatures,
				EntityTemplates.CollisionShape.RECTANGLE,
				1,
				0.62f,
				true);

			EntityTemplates.ExtendEntityToFood(prefab, TFoodInfos.cookedRadish);

			return prefab;
		}

		public string[] GetForbiddenDlcIds() => null;

		public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst)
		{
			if (DlcManager.FeatureRadiationEnabled())
			{
				var emitter = inst.AddOrGet<RadiationEmitter>();
				emitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
				emitter.radiusProportionalToRads = false;
				emitter.emitRadiusX = 1;
				emitter.emitRadiusY = 1;
				emitter.emitRads = 5f;
				emitter.emissionOffset = new Vector3(0, -2);
			}

			/*            var light = inst.AddOrGet<Light2D>();
                        light.Color = new Color(0, 2f, 0, 0.6f);
                        light.Range = 2;
                        light.shape = LightShape.Circle;
                        light.drawOverlay = false;*/
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
