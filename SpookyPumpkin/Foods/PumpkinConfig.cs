using UnityEngine;
using static SpookyPumpkin.STRINGS.ITEMS.FOOD;

namespace SpookyPumpkin.Foods
{
    class PumpkinConfig : IEntityConfig
    {
        public const string ID = ModAssets.PREFIX + "Pumpkin";

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
                isPickupable: true);

            var foodInfo = new EdiblesManager.FoodInfo(
                id: ID,
                caloriesPerUnit: 600000f,
                quality: TUNING.FOOD.FOOD_QUALITY_AWFUL,
                preserveTemperatue: 255.15f,
                rotTemperature: 277.15f,
                spoilTime: 2400f,
                can_rot: true);

            var foodEntity = EntityTemplates.ExtendEntityToFood(
                template: looseEntity,
                foodInfo: foodInfo);

            return foodEntity;
        }

        public void OnPrefabInit(GameObject inst) { }

        public void OnSpawn(GameObject inst) { }
    }
}
