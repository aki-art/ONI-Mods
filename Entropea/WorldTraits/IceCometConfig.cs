using STRINGS;
using UnityEngine;

namespace Entropea.WorldTraits
{
    class IceCometConfig : IEntityConfig
    {

        public static string ID = "AE_IceComet";
        public GameObject CreatePrefab()
        {
            GameObject gameObject = EntityTemplates.CreateEntity(ID, "Ice Comet");

            gameObject.AddOrGet<SaveLoadRoot>();
            gameObject.AddOrGet<LoopingSounds>();

            Comet comet = gameObject.AddOrGet<Comet>();
            comet.entityDamage = 2;
            comet.totalTileDamage = 0.15f;
            comet.splashRadius = 10;
            comet.impactSound = "Meteor_Small_Impact";
            comet.temperatureRange = new Vector2(50, 180);
            comet.massRange = new Vector2(5f, 20f);
            comet.explosionOreCount = new Vector2I(2, 4);
            comet.flyingSoundID = 1;
            comet.explosionEffectHash = SpawnFXHashes.MeteorImpactDirt;//SpawnFXHashes.MeteorImpactDirt;



            PrimaryElement primaryElement = gameObject.AddOrGet<PrimaryElement>();
            primaryElement.SetElement(SimHashes.Ice);
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
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f);


            return gameObject;
        }

        public void OnPrefabInit(GameObject go) { }

        public void OnSpawn(GameObject go) { }
    }
}
