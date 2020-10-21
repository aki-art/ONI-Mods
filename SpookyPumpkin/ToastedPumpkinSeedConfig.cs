using UnityEngine;
using static EdiblesManager;

namespace SpookyPumpkin
{
    public class ToastedPumpkinSeedConfig : IEntityConfig
    {
        public const string ID = "SP_ToastedPumpkinSeed";
        public static ComplexRecipe recipe;

        public GameObject CreatePrefab()
        {
            GameObject prefab = EntityTemplates.CreateLooseEntity(
                id: ID,
                name: STRINGS.ITEMS.FOOD.SP_TOASTEDPUMPKINSEED.NAME,
                desc: STRINGS.ITEMS.FOOD.SP_TOASTEDPUMPKINSEED.DESC,
                mass: 1f,
                unitMass: false,
                anim: Assets.GetAnim("sp_toastedpumpkinseed_kanim"),
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
                caloriesPerUnit: 800000f,
                quality: 2,
                preserveTemperatue: 255.15f,
                rotTemperature: 277.15f,
                spoilTime: 9999f,
                can_rot: false);

            return EntityTemplates.ExtendEntityToFood(prefab, foodInfo);
        }

        public void OnPrefabInit(GameObject inst)
        {
        }

        public void OnSpawn(GameObject inst)
        {
        }
    }
}
