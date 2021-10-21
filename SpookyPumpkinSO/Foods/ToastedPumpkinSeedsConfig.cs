using UnityEngine;
using static EdiblesManager;

namespace SpookyPumpkinSO.Foods
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
                dlcId: DlcManager.VANILLA_ID,
                caloriesPerUnit: 800f * 1000f,
                quality: TUNING.FOOD.FOOD_QUALITY_GOOD,
                preserveTemperatue: TUNING.FOOD.HIGH_PRESERVE_TEMPERATURE,
                rotTemperature: TUNING.FOOD.HIGH_ROT_TEMPERATURE,
                spoilTime: TUNING.FOOD.SPOIL_TIME.VERYSLOW,
                can_rot: false);

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
