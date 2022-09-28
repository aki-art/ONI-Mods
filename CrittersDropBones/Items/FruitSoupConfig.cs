using CrittersDropBones.Effects;
using Klei.AI;
using TUNING;
using UnityEngine;

namespace CrittersDropBones.Items
{
    public class FruitSoupConfig : IEntityConfig
    {
        public const string ID = Mod.PREFIX + "FruitSoup";
        public static ComplexRecipe recipe;

        public GameObject CreatePrefab()
        {
            var prefab = FEntityTemplates.CreateSoup(
                ID,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_FRUITSOUP.NAME,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_FRUITSOUP.DESC,
                "cdb_fruitsoup_kanim");

            var foodInfo = Util.FoodInfoBuilder.StandardFood(ID)
                .KcalPerUnit(3200)
                .Quality(FOOD.FOOD_QUALITY_AMAZING)
                .Effect(CDBEffects.CHILL_FOOD)
                .Build();


            return EntityTemplates.ExtendEntityToFood(prefab, foodInfo);
        }


        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst) { }

        public void OnSpawn(GameObject inst) { }
    }
}
