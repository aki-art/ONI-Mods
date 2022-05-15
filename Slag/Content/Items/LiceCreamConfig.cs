using UnityEngine;

namespace Slag.Content.Items
{
    public class LiceCreamConfig : IEntityConfig
    {
        public const string ID = "Slag_LiceCream";
        public const float DEFAULT_TEMP = 263.15f;

        public GameObject CreatePrefab()
        {
            var prefab = EntityTemplates.CreateLooseEntity(
                ID,
                STRINGS.ITEMS.FOOD.LICE_CREAM.NAME,
                STRINGS.ITEMS.FOOD.LICE_CREAM.DESC,
                1f,
                false,
                Assets.GetAnim("cottoncandy_kanim"),
                "object",
                Grid.SceneLayer.Front,
                EntityTemplates.CollisionShape.RECTANGLE,
                0.8f,
                0.4f,
                true,
                element: SimHashes.CrushedIce);


            var foodInfo = new EdiblesManager.FoodInfo(
                 ID,
                 DlcManager.VANILLA_ID,
                 6000000f,
                 3,
                 235.15f,
                 277.15f,
                 600f,
                 false);

            EntityTemplates.ExtendEntityToFood(prefab, foodInfo);
            prefab.GetComponent<PrimaryElement>().Temperature = DEFAULT_TEMP;

            return prefab;
        }

        public string[] GetDlcIds()
        {
            return DlcManager.AVAILABLE_ALL_VERSIONS;
        }

        public void OnPrefabInit(GameObject inst)
        {
            // make it somewhat insulated just so it doesn't immediately melt after production
            var simTemp = inst.AddOrGet<SimTemperatureTransfer>();
            simTemp.SurfaceArea = 10f;
            simTemp.Thickness = 0.7f;
        }

        public void OnSpawn(GameObject inst)
        {
        }
    }
}
