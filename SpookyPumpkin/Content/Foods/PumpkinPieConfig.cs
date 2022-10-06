using System.Collections.Generic;
using UnityEngine;
using static EdiblesManager;

namespace SpookyPumpkinSO.Content.Foods
{
    public class PumkinPieConfig : IEntityConfig
    {
        public const string ID = "SP_PumpkinPie";
        public static ComplexRecipe recipe;

        public GameObject CreatePrefab()
        {
            var prefab = EntityTemplates.CreateLooseEntity(
                ID,
                STRINGS.ITEMS.FOOD.SP_PUMPKINPIE.NAME,
                STRINGS.ITEMS.FOOD.SP_PUMPKINPIE.DESC,
                1f,
                false,
                Assets.GetAnim("sp_pumpkinpie_kanim"),
                "object",
                Grid.SceneLayer.Front,
                EntityTemplates.CollisionShape.RECTANGLE,
                0.8f,
                0.4f,
                true);


            var foodInfo = new FoodInfo(
                ID,
                DlcManager.VANILLA_ID,
                6000f * 1000f,
                TUNING.FOOD.FOOD_QUALITY_WONDERFUL,
                TUNING.FOOD.DEFAULT_PRESERVE_TEMPERATURE,
                TUNING.FOOD.DEFAULT_ROT_TEMPERATURE,
                TUNING.FOOD.SPOIL_TIME.DEFAULT,
                true).AddEffects(new List<string>
                {
                    SPEffects.HOLIDAY_SPIRIT
                }, DlcManager.AVAILABLE_ALL_VERSIONS);

            return EntityTemplates.ExtendEntityToFood(prefab, foodInfo);
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst)
        {
        }

        public void OnSpawn(GameObject inst)
        {
        }
    }
}
