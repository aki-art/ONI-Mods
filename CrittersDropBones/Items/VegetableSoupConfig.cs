using CrittersDropBones.Effects;
using System.Collections.Generic;
using UnityEngine;
using static EdiblesManager;

namespace CrittersDropBones.Items
{
    public class VegetableSoupConfig : IEntityConfig
    {
        public static string ID = Mod.Prefix("VegetableSoup");
        public static ComplexRecipe recipe;

        public GameObject CreatePrefab()
        {
            var prefab = FEntityTemplates.CreateSoup(
                ID,
                STRINGS.ITEMS.FOOD.CDB_VEGETABLESOUP.NAME,
                STRINGS.ITEMS.FOOD.CDB_VEGETABLESOUP.DESC,
                "cdb_vegetablesoup_kanim");

            var foodInfo = new FoodInfo(
                ID,
                DlcManager.VANILLA_ID,
                3200f * 1000f,
                TUNING.FOOD.FOOD_QUALITY_GOOD,
                TUNING.FOOD.DEFAULT_PRESERVE_TEMPERATURE,
                TUNING.FOOD.DEFAULT_ROT_TEMPERATURE,
                TUNING.FOOD.SPOIL_TIME.DEFAULT,
                true)
                .AddEffects(new List<string>
                {
                    StaminaRegenerationEffect.ID
                }, DlcManager.AVAILABLE_ALL_VERSIONS);

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
