using Slag.Content.Critters;
using UnityEngine;

namespace Slag.Content.Entities
{
    public class EggCometConfig : IEntityConfig
    {
        public const string ID = "Slag_EggComet";

        public GameObject CreatePrefab()
        {
            var prefab = EntityTemplates.CreateEntity(ID, STRINGS.COMETS.EGGCOMET.NAME, true);

            prefab.AddOrGet<SaveLoadRoot>();
            prefab.AddOrGet<LoopingSounds>();

            var comet = prefab.AddOrGet<EggComet>();
            comet.massRange = new Vector2(1f, 1f);
            comet.EXHAUST_ELEMENT = SimHashes.CarbonDioxide;
            comet.EXHAUST_RATE = 5f;
            comet.temperatureRange = new Vector2(296.15f, 318.15f);
            comet.entityDamage = 0;
            comet.explosionOreCount = new Vector2I(0, 0);
            comet.totalTileDamage = 0f;
            comet.splashRadius = 1;
            comet.impactSound = "Meteor_GassyMoo_Impact";
            comet.flyingSoundID = 4;
            comet.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
            comet.addTiles = 0;
            comet.destroyOnExplode = false;
            comet.craterPrefabs = new string[]
            {
                SlagmiteConfig.EGG_ID,
                SlagmiteConfig.EGG_ID, // lazy weighting
                CookedEggConfig.ID
            };

            var primaryElement = prefab.AddOrGet<PrimaryElement>();
            primaryElement.SetElement(SimHashes.Creature, true);
            primaryElement.Temperature = (comet.temperatureRange.x + comet.temperatureRange.y) / 2f;

            var kbac = prefab.AddOrGet<KBatchedAnimController>();
            kbac.AnimFiles = new KAnimFile[]
            {
                Assets.GetAnim("egg_comet_kanim")
            };

            kbac.isMovable = true;
            kbac.initialAnim = "fall_loop";
            kbac.initialMode = KAnim.PlayMode.Loop;
            kbac.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;

            // half size
            prefab.transform.localScale = new Vector3(0.5f, 0.5f, 1f);

            prefab.AddOrGet<KCircleCollider2D>().radius = 0.5f;
            prefab.AddTag(GameTags.Comet);

            return prefab;
        }

        public string[] GetDlcIds()
        {
            return DlcManager.AVAILABLE_ALL_VERSIONS;
        }

        public void OnPrefabInit(GameObject inst)
        {
        }

        public void OnSpawn(GameObject inst)
        {
        }
    }
}
