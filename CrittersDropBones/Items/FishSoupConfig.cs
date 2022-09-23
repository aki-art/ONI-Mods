using TUNING;
using UnityEngine;

namespace CrittersDropBones.Items
{
    public class FishSoupConfig : IEntityConfig
    {
        public const string ID = Mod.PREFIX + "FishSoup";
        public static ComplexRecipe recipe;

        public GameObject CreatePrefab()
        {
            var prefab = FEntityTemplates.CreateSoup(
                ID,
                STRINGS.ITEMS.FOOD.CDB_FISHSOUP.NAME,
                STRINGS.ITEMS.FOOD.CDB_FISHSOUP.DESC,
                "cdb_fishsoup_kanim");

            var foodInfo = Util.FoodInfoBuilder.StandardFood(ID)
                .KcalPerUnit(3200)
                .Quality(FOOD.FOOD_QUALITY_GOOD)
                .Effect("SeafoodRadiationResistance", DlcManager.AVAILABLE_EXPANSION1_ONLY)
                .Build();

            return EntityTemplates.ExtendEntityToFood(prefab, foodInfo);
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst) { }

        public void OnSpawn(GameObject inst) { }
    }
}
