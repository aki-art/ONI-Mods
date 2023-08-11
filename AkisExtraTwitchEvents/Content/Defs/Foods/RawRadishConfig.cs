using UnityEngine;

namespace Twitchery.Content.Defs.Foods
{
	public class RawRadishConfig : IEntityConfig
    {
        public const string ID = "AkisExtraTwitchEvents_RawRadish";

        public GameObject CreatePrefab()
        {
            var prefab = EntityTemplates.CreateLooseEntity(
                ID,
                STRINGS.ITEMS.FOOD.AKISEXTRATWITCHEVENTS_RAWRADISH.NAME,
                STRINGS.ITEMS.FOOD.AKISEXTRATWITCHEVENTS_RAWRADISH.DESC,
                1f,
                true,
                Assets.GetAnim("aete_rawradish_kanim"),
                "object",
                Grid.SceneLayer.Creatures,
                EntityTemplates.CollisionShape.RECTANGLE,
                1f,
                0.6f,
                true);

            EntityTemplates.ExtendEntityToFood(prefab, TFoodInfos.rawRadish);

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
                emitter.emitRadiusX = 1;
                emitter.emitRadiusY = 1;
                emitter.emitRads = 5f;
                emitter.emissionOffset = new Vector3(0, -2);
            }
        }

        public void OnSpawn(GameObject inst)
        {
        }
    }
}
