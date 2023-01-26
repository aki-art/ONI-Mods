using UnityEngine;
using TUNING;
using CrittersDropBones.Content;

namespace CrittersDropBones.Content.Items
{
    public class GrubGrubConfig : IEntityConfig
    {
        public const string ID = "CrittersDropBones_GrubGrubGrub";

        public GameObject CreatePrefab()
        {
            var prefab = FEntityTemplates.CreateSoup(
                ID,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_GRUBGRUBGRUB.NAME,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_GRUBGRUBGRUB.DESC,
                "cdb_grubgrub_kanim");

            var foodInfo = Util.FoodInfoBuilder.StandardFood(ID)
                .KcalPerUnit(3200)
                .Quality(FOOD.FOOD_QUALITY_GOOD)
                .Effect(CDBEffects.CHILL_FOOD)
                .Build();

            EntityTemplates.ExtendEntityToFood(prefab, foodInfo);

            return prefab;
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst) { }

        public void OnSpawn(GameObject inst) { }
    }
}
