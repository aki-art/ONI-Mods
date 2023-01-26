using CrittersDropBones.Content;
using TUNING;
using UnityEngine;

namespace CrittersDropBones.Content.Items
{
    public class VegetableSoupConfig : IEntityConfig
    {
        public const string ID = "CrittersDropBones_VegetableSoup";

        public GameObject CreatePrefab()
        {
            var prefab = FEntityTemplates.CreateSoup(
                ID,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_VEGETABLESOUP.NAME,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_VEGETABLESOUP.DESC,
                "cdb_vegetablesoup_kanim");

            var foodInfo = Util.FoodInfoBuilder.StandardFood(ID)
                .KcalPerUnit(3200)
                .Quality(FOOD.FOOD_QUALITY_GOOD)
                .Effect(CDBEffects.STAMINA_REGENERATION)
                .Build();

            EntityTemplates.ExtendEntityToFood(prefab, foodInfo);

            return prefab;
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst) { }

        public void OnSpawn(GameObject inst) { }
    }
}
