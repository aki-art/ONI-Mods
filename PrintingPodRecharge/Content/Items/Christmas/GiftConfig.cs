using UnityEngine;

namespace PrintingPodRecharge.Content.Items.Christmas
{
    public class GiftConfig : IEntityConfig
    {
        public const string ID = "PrintingPodRecharge_ChristmasGift";

        public GameObject CreatePrefab()
        {
            var prefab = EntityTemplates.CreateLooseEntity(
                ID,
                "Gift",
                "Open for a surprise!",
                100f,
                true,
                Assets.GetAnim("rpp_giftbox_kanim"),
                "closed",
                Grid.SceneLayer.Creatures,
                EntityTemplates.CollisionShape.RECTANGLE,
                1,
                1.1f,
                true);

            prefab.AddOrGet<Gift>();

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
