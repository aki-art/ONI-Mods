using CrittersDropBones.Effects;
using TUNING;
using UnityEngine;

namespace CrittersDropBones.Items
{
    public class GelatineConfig : IEntityConfig
    {
        public const string ID = Mod.PREFIX + "Gelatine";

        public GameObject CreatePrefab()
        {
            var prefab = FEntityTemplates.CreateSoup(
                ID,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_GELATINE.NAME,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_GELATINE.DESC,
                "cdb_fruitsoup_kanim");

            var foodInfo = Util.FoodInfoBuilder.StandardFood(ID)
                .KcalPerUnit(800)
                .Quality(FOOD.FOOD_QUALITY_AWFUL)
                .Effect(CDBEffects.CHILL_FOOD)
                .Build();

            return EntityTemplates.ExtendEntityToFood(prefab, foodInfo);
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst) { }

        public void OnSpawn(GameObject inst) { }
    }
}
