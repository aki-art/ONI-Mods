using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

namespace Slag.Content.Items
{
    public class SlagWoolConfig : IEntityConfig
    {
        public const string ID = "Slag_SlagWool";
        private readonly AttributeModifier overHeatModifier = new AttributeModifier(Db.Get().BuildingAttributes.OverheatTemperature.Id, +120);
        private readonly AttributeModifier decorModifier = new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, -0.25f, STRINGS.ITEMS.INDUSTRIAL_INGREDIENTS.SLAG_WOOL.NAME , true);

        public GameObject CreatePrefab()
        {
            var prefab = EntityTemplates.CreateLooseEntity(
                ID,
                STRINGS.ITEMS.INDUSTRIAL_INGREDIENTS.SLAG_WOOL.NAME,
                STRINGS.ITEMS.INDUSTRIAL_INGREDIENTS.SLAG_WOOL.DESC,
                1f,
                true,
                Assets.GetAnim("slagwool_kanim"),
                "object",
                Grid.SceneLayer.BuildingBack,
                EntityTemplates.CollisionShape.RECTANGLE,
                0.8f,
                0.45f,
                true,
                0,
                Elements.Slag,
                new List<Tag>
                {
                    GameTags.IndustrialIngredient,
                    // GameTags.BuildingFiber,
                    ModAssets.Tags.slagWool
                });

            prefab.AddOrGet<EntitySplitter>();

            var modifiers = prefab.AddOrGet<PrefabAttributeModifiers>();
            modifiers.AddAttributeDescriptor(overHeatModifier);
            modifiers.AddAttributeDescriptor(decorModifier);

            return prefab;
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
