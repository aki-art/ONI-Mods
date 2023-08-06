using UnityEngine;

namespace Twitchery.Content.Defs.Foods
{
    internal class GoopParfaitConfig : IEntityConfig
    {
        public const string ID = "AkisExtraTwitchEvents_GoopParfait";

        public GameObject CreatePrefab()
        {
            var prefab = EntityTemplates.CreateLooseEntity(
                ID,
                STRINGS.ITEMS.FOOD.AKISEXTRATWITCHEVENTS_GOOPPARFAIT.NAME,
                STRINGS.ITEMS.FOOD.AKISEXTRATWITCHEVENTS_GOOPPARFAIT.DESC,
                1f,
                true,
                Assets.GetAnim("aete_goop_parfait_kanim"),
                "object",
                Grid.SceneLayer.Creatures,
                EntityTemplates.CollisionShape.RECTANGLE,
                1f,
                0.8f,
                true);

            EntityTemplates.ExtendEntityToFood(prefab, TFoodInfos.goopParfait);

            return prefab;
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst) { }

        public void OnSpawn(GameObject inst) { }
    }
}
