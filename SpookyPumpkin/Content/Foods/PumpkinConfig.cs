using System.Collections.Generic;
using UnityEngine;
using static SpookyPumpkinSO.STRINGS.ITEMS.FOOD;

namespace SpookyPumpkinSO.Content.Foods
{
    internal class PumpkinConfig : IEntityConfig
    {
        public const string ID = "SP_Pumpkin";

        public GameObject CreatePrefab()
        {
            var looseEntity = EntityTemplates.CreateLooseEntity(
                id: ID,
                name: SP_PUMPKIN.NAME,
                desc: SP_PUMPKIN.DESC,
                mass: 1f,
                unitMass: false,
                anim: Assets.GetAnim("sp_itempumpkin_kanim"),
                initialAnim: "object",
                sceneLayer: Grid.SceneLayer.Front,
                collisionShape: EntityTemplates.CollisionShape.RECTANGLE,
                width: 0.8f,
                height: 0.4f,
                isPickupable: true,
                additionalTags: new List<Tag>
                {
                    ModAssets.buildingPumpkinTag
                });

            var foodInfo = new EdiblesManager.FoodInfo(
                id: ID,
                dlcId: DlcManager.VANILLA_ID,
                caloriesPerUnit: 600f * 1000f,
                quality: TUNING.FOOD.FOOD_QUALITY_AWFUL,
                preserveTemperatue: TUNING.FOOD.DEFAULT_PRESERVE_TEMPERATURE,
                rotTemperature: TUNING.FOOD.DEFAULT_ROT_TEMPERATURE,
                spoilTime: TUNING.FOOD.SPOIL_TIME.DEFAULT,
                can_rot: true);

            var foodEntity = EntityTemplates.ExtendEntityToFood(
                template: looseEntity,
                foodInfo: foodInfo);

            return foodEntity;
        }

        public string[] GetDlcIds()
        {
            return DlcManager.AVAILABLE_ALL_VERSIONS;
        }

        public void OnPrefabInit(GameObject inst) { }

        public void OnSpawn(GameObject inst) { }
    }
}
