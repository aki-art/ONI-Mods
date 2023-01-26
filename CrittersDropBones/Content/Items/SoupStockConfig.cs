using UnityEngine;

namespace CrittersDropBones.Content.Items
{
    public class SoupStockConfig : IEntityConfig
    {
        public const string ID = "CrittersDropBones_SoupStock";

        public GameObject CreatePrefab()
        {
            var prefab = FEntityTemplates.CreateSoup(
                ID,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_SOUPSTOCK.NAME,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_SOUPSTOCK.DESC,
                "cdb_bonestock_kanim");

            var foodInfo = Util.FoodInfoBuilder.StandardFood(ID)
                .KcalPerUnit(800)
                .Quality(TUNING.FOOD.FOOD_QUALITY_TERRIBLE)
                .Build();

            EntityTemplates.ExtendEntityToFood(prefab, foodInfo);

            return prefab;
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst) { }

        public void OnSpawn(GameObject inst) { }
    }
}
