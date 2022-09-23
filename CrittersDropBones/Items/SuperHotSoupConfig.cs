using UnityEngine;
using static EdiblesManager;

namespace CrittersDropBones.Items
{
    public class SuperHotSoupConfig : IEntityConfig
    {
        public const string ID = Mod.PREFIX + "SuperHotSoup";
        public static ComplexRecipe recipe;

        public GameObject CreatePrefab()
        {
            var prefab = FEntityTemplates.CreateSoup(
                ID,
                STRINGS.ITEMS.FOOD.CDB_SUPERHOTSOUP.NAME,
                STRINGS.ITEMS.FOOD.CDB_SUPERHOTSOUP.DESC,
                "cdb_superhotsoup_kanim");

            var foodInfo = new FoodInfo(
                ID,
                DlcManager.VANILLA_ID,
                2200f * 1000f,
                TUNING.FOOD.FOOD_QUALITY_GOOD,
                TUNING.FOOD.DEFAULT_PRESERVE_TEMPERATURE,
                TUNING.FOOD.DEFAULT_ROT_TEMPERATURE,
                TUNING.FOOD.SPOIL_TIME.DEFAULT,
                true);

            var gameObject = EntityTemplates.ExtendEntityToFood(prefab, foodInfo);
            return gameObject;
        }

        public string[] GetDlcIds()
        {
            return DlcManager.AVAILABLE_ALL_VERSIONS;
        }

        public void OnPrefabInit(GameObject inst) { }

        public void OnSpawn(GameObject inst) { }
    }
}
