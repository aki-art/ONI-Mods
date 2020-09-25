using UnityEngine;

namespace WorldTraitsPlus.Meteors
{
    class IceCometConfig : IEntityConfig
    {
        public static string ID = "WTP_IceComet";
        public GameObject CreatePrefab()
        {
            GameObject gameObject = EntityTemplates.CreateEntity(ID, STRINGS.UI.SPACEDESTINATIONS.COMETS.ICECOMET.NAME);

            gameObject.AddOrGet<SaveLoadRoot>();
            gameObject.AddOrGet<LoopingSounds>();

            Comet comet = gameObject.AddOrGet<Comet>();
            comet.entityDamage = 2;
            comet.totalTileDamage = 0;
            comet.splashRadius = 3;
            comet.impactSound = "Meteor_Small_Impact";
            comet.temperatureRange = new Vector2(50, 180);
            comet.explosionTemperatureRange = new Vector2(100, 273);
            comet.massRange = new Vector2(5f, 20f);
            comet.explosionOreCount = new Vector2I(2, 4);
            comet.flyingSoundID = 1;
            comet.explosionEffectHash = Tuning.FXHashes.MeteorImpactSteam;
            comet.addTiles = 2;
            comet.addTilesMinHeight = 1;
            comet.addTilesMaxHeight = 4;

            PrimaryElement primaryElement = gameObject.AddOrGet<PrimaryElement>();
            primaryElement.SetElement(SimHashes.CrushedIce);
            primaryElement.Temperature = (comet.temperatureRange.x + comet.temperatureRange.y) / 2f;

            KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
            kbatchedAnimController.AnimFiles = new KAnimFile[]
            {
                Assets.GetAnim("icecomet_kanim")
            };
            kbatchedAnimController.isMovable = true;
            kbatchedAnimController.initialAnim = "fall_loop";
            kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
            kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;

            gameObject.AddOrGet<KCircleCollider2D>().radius = 1f;
            gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 1f);

            return gameObject;
        }

        public void OnPrefabInit(GameObject go) { }

        public void OnSpawn(GameObject go) { }
    }
}
