using System.Collections.Generic;
using UnityEngine;

namespace CrittersDropBones.Items
{
    public class FishBoneConfig : IEntityConfig
    {
        public static string ID = Mod.PREFIX + "FishBone";

        public GameObject CreatePrefab()
        {
            GameObject prefab = EntityTemplates.CreateLooseEntity(
                ID,
                STRINGS.ITEMS.BONES.CDB_FISHBONE.NAME,
                STRINGS.ITEMS.BONES.CDB_FISHBONE.DESC,
                0.5f,
                true,
                Assets.GetAnim("cdb_fishbone_kanim"),
                "object",
                Grid.SceneLayer.BuildingBack,
                EntityTemplates.CollisionShape.RECTANGLE,
                0.8f,
                0.4f,
                true,
                0,
                SimHashes.Lime,
                additionalTags: new List<Tag>
                {
                    GameTags.Organics,
                    ModAssets.Tags.Bones
                });

            prefab.AddOrGet<EntitySplitter>();
            prefab.AddOrGet<TieredItem>();
            prefab.AddOrGet<SimpleMassStatusItem>();

            EntityTemplates.CreateAndRegisterCompostableFromPrefab(prefab);

            return prefab;
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst)
        {
            inst.AddOrGet<TieredItem>().tiers = new List<TieredItem.Tier>()
            {
                new TieredItem.Tier()
                {
                    minimumMass = 0f,
                    anim = "fish_small"
                },
                new TieredItem.Tier()
                {
                    minimumMass = 2f,
                    anim = "object"
                }
            };
        }

        public void OnSpawn(GameObject inst)
        {
        }
    }
}