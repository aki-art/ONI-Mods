using CrittersDropBones.Effects;
using TUNING;
using UnityEngine;

namespace CrittersDropBones.Items
{
    public class VegetableSoupConfig : IEntityConfig
    {
        public const string ID = Mod.PREFIX + "VegetableSoup";

        public GameObject CreatePrefab()
        {
            var prefab = FEntityTemplates.CreateSoup(
                ID,
                STRINGS.ITEMS.FOOD.CDB_VEGETABLESOUP.NAME,
                STRINGS.ITEMS.FOOD.CDB_VEGETABLESOUP.DESC,
                "cdb_vegetablesoup_kanim");

            var foodInfo = Util.FoodInfoBuilder.StandardFood(ID)
                .KcalPerUnit(3200)
                .Quality(FOOD.FOOD_QUALITY_GOOD)
                .Effect(CDBEffects.STAMINA_REGENERATION)
                .Build();

            return EntityTemplates.ExtendEntityToFood(prefab, foodInfo);
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst) { }

        public void OnSpawn(GameObject inst) { }
    }
}
