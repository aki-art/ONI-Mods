using CrittersDropBones.Content;
using UnityEngine;

namespace CrittersDropBones.Content.Items
{
    public class SludgeConfig : IEntityConfig
    {
        public const string ID = "CrittersDropBones_Sludge";

        public GameObject CreatePrefab()
        {
            var prefab = FEntityTemplates.CreateSoup(
                ID,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_SLUDGE.NAME,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_SLUDGE.DESC,
                "cdb_sludge_kanim");

            var foodInfo = Util.FoodInfoBuilder.StandardFood(ID)
                .KcalPerUnit(3200)
                .Quality(TUNING.FOOD.FOOD_QUALITY_GOOD)
                .Effect(CDBEffects.UPSET_STOMACH)
                .Build();

            EntityTemplates.ExtendEntityToFood(prefab, foodInfo);

            return prefab;
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst) { }

        public void OnSpawn(GameObject inst) { }
    }
}
