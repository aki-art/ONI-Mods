using UnityEngine;

namespace PrintingPodRecharge.Content.Items
{
    public class LeekConfig : IEntityConfig
    {
        public const string ID = "PrintingPodRecharge_Leek";

        public GameObject CreatePrefab()
        {
            var prefab = EntityTemplates.CreateLooseEntity(
                ID,
                STRINGS.ITEMS.FOOD.PRINTINGPODRECHARGE_LEEK.NAME,
                STRINGS.ITEMS.FOOD.PRINTINGPODRECHARGE_LEEK.DESC,
                1f,
                false,
                Assets.GetAnim("rrp_leek_kanim"),
                "object",
                Grid.SceneLayer.Front,
                EntityTemplates.CollisionShape.RECTANGLE,
                0.8f,
                0.55f,
                true);

            var foodInfo = new EdiblesManager.FoodInfo(
                ID,
                DlcManager.VANILLA_ID,
                800f * 1000f,
                1,
                255.15f,
                277.15f,
                2400f,
                true);

            EntityTemplates.ExtendEntityToFood(prefab, foodInfo);

            return prefab;
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst)
        {
        }

        public void OnSpawn(GameObject inst)
        {
        }
    }
}
