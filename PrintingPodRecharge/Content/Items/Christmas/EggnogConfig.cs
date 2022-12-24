using UnityEngine;

namespace PrintingPodRecharge.Content.Items.Christmas
{
    public class EggnogConfig : IEntityConfig
    {
        public const string ID = "PrintingPodRecharge_Eggnog";

        public GameObject CreatePrefab()
        {
            var info = new MedicineInfo(
                ID,
                PEffects.EGGNOG,
                MedicineInfo.MedicineType.Booster,
                null);

            var prefab = EntityTemplates.CreateLooseEntity(
                ID,
                "Eggnog",
                "A non-alcoholic sweet treat that will make any Duplicants day better!",
                1f,
                true,
                Assets.GetAnim("rpp_eggnog_kanim"),
                "object",
                Grid.SceneLayer.Front,
                EntityTemplates.CollisionShape.RECTANGLE,
                0.8f,
                0.7f,
                true);

            EntityTemplates.ExtendEntityToMedicine(prefab, info);

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
