using UnityEngine;
using static EdiblesManager;

namespace CrittersDropBones.Items
{
    public class SoupStockConfig : IEntityConfig
    {
        public static string ID = Mod.Prefix("SoupStock");
        public static ComplexRecipe recipe;

        public GameObject CreatePrefab()
        {
            GameObject prefab = FEntityTemplates.CreateSoup(
                ID,
                STRINGS.ITEMS.FOOD.CDB_SOUPSTOCK.NAME,
                STRINGS.ITEMS.FOOD.CDB_SOUPSTOCK.DESC,
                "cdb_bonestock_kanim");

            FoodInfo foodInfo = new FoodInfo(
                ID,
                DlcManager.VANILLA_ID,
                3200f * 1000f,
                TUNING.FOOD.FOOD_QUALITY_GOOD,
                TUNING.FOOD.DEFAULT_PRESERVE_TEMPERATURE,
                TUNING.FOOD.DEFAULT_ROT_TEMPERATURE,
                TUNING.FOOD.SPOIL_TIME.DEFAULT,
                true);

            GameObject gameObject = EntityTemplates.ExtendEntityToFood(prefab, foodInfo);
            return gameObject;
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst) { }

        public void OnSpawn(GameObject inst) { }
    }
}
