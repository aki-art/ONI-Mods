using CrittersDropBones.Content.Items;
using System.Collections.Generic;
using UnityEngine;
using static EdiblesManager;

namespace CrittersDropBones.Integration.SpookyPumpkin
{
    public class PumpkinSoupConfig
    {
        public static string ID = "CrittersDropBones_PumpkinSoup";
        public static ComplexRecipe recipe;

        public GameObject CreatePrefab()
        {
            var prefab = FEntityTemplates.CreateSoup(
                ID,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_PUMPKINSOUP.NAME,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_PUMPKINSOUP.DESC,
                "cdb_pumpkinsoup_kanim");

            var foodInfo = new FoodInfo(
                ID,
                DlcManager.VANILLA_ID,
                3200f * 1000f,
                TUNING.FOOD.FOOD_QUALITY_GOOD,
                TUNING.FOOD.DEFAULT_PRESERVE_TEMPERATURE,
                TUNING.FOOD.DEFAULT_ROT_TEMPERATURE,
                TUNING.FOOD.SPOIL_TIME.DEFAULT,
                true);

            foodInfo.AddEffects(new List<string>()
            {
                "AHM_HolidaySpirit"
            }, DlcManager.AVAILABLE_ALL_VERSIONS);

            EntityTemplates.ExtendEntityToFood(prefab, foodInfo);

            return prefab;
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;
    }
}
