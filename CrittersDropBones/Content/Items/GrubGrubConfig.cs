using CrittersDropBones.Effects;
using UnityEngine;
using TUNING;

namespace CrittersDropBones.Items
{
    public class GrubGrubConfig : IEntityConfig
    {
        public const string ID = Mod.PREFIX + "GrubGrubGrub";

        public GameObject CreatePrefab()
        {
            var prefab = FEntityTemplates.CreateSoup(
                ID,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_GRUBGRUB.NAME,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_GRUBGRUB.DESC,
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
