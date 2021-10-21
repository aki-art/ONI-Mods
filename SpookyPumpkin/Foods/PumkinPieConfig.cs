using System.Collections.Generic;
using UnityEngine;
using static EdiblesManager;

namespace SpookyPumpkin.Foods
{
    public class PumkinPieConfig : IEntityConfig
    {
        public const string ID = "SP_PumpkinPie";
        public static ComplexRecipe recipe;

        public GameObject CreatePrefab()
        {
            GameObject prefab = EntityTemplates.CreateLooseEntity(
                id: ID,
                name: STRINGS.ITEMS.FOOD.SP_PUMPKINPIE.NAME,
                desc: STRINGS.ITEMS.FOOD.SP_PUMPKINPIE.DESC,
                mass: 1f,
                unitMass: false,
                anim: Assets.GetAnim("sp_pumpkinpie_kanim"),
                initialAnim: "object",
                sceneLayer: Grid.SceneLayer.Front,
                collisionShape: EntityTemplates.CollisionShape.RECTANGLE,
                width: 0.8f,
                height: 0.4f,
                isPickupable: true,
                sortOrder: 0,
                element: SimHashes.Creature,
                additionalTags: null);

            FoodInfo foodInfo = new FoodInfo(
                id: ID,
                dlcId: DlcManager.VANILLA_ID,
                caloriesPerUnit: 6000f * 1000f,
                quality: TUNING.FOOD.FOOD_QUALITY_WONDERFUL,
                preserveTemperatue: TUNING.FOOD.DEFAULT_PRESERVE_TEMPERATURE,
                rotTemperature: TUNING.FOOD.DEFAULT_ROT_TEMPERATURE,
                spoilTime: TUNING.FOOD.SPOIL_TIME.DEFAULT,
                can_rot: true).AddEffects(new List<string>
                {
                    ModAssets.holidaySpiritEffectID
                }, DlcManager.AVAILABLE_ALL_VERSIONS);

            return EntityTemplates.ExtendEntityToFood(prefab, foodInfo);
        }

        public string[] GetDlcIds()
        {
            return DlcManager.AVAILABLE_ALL_VERSIONS;
        }

        public void OnPrefabInit(GameObject inst)
        {
        }

        public void OnSpawn(GameObject inst)
        {
        }
    }
}
