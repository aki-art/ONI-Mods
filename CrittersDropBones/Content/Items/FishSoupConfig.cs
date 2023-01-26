using TUNING;
using UnityEngine;

namespace CrittersDropBones.Content.Items
{
    public class FishSoupConfig : IEntityConfig
    {
        public const string ID = "CrittersDropBones_FishSoup";

        public GameObject CreatePrefab()
        {
            var prefab = FEntityTemplates.CreateSoup(
                ID,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_FISHSOUP.NAME,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_FISHSOUP.DESC,
                "cdb_fishsoup_kanim");

            var foodInfo = Util.FoodInfoBuilder.StandardFood(ID)
                .KcalPerUnit(3200)
                .Quality(FOOD.FOOD_QUALITY_GOOD)
                .Effect("SeafoodRadiationResistance", DlcManager.AVAILABLE_EXPANSION1_ONLY)
                .Build();

            EntityTemplates.ExtendEntityToFood(prefab, foodInfo);

            return prefab;
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst) { }

        public void OnSpawn(GameObject inst) { }
    }
}
