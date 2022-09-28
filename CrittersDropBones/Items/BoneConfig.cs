using FUtility;
using System.Collections.Generic;
using UnityEngine;

namespace CrittersDropBones.Items
{
    public class BoneConfig : IEntityConfig
    {
        public const string ID = Mod.PREFIX + "Bone";

        public GameObject CreatePrefab()
        {
            var prefab = EntityTemplates.CreateLooseEntity(
                ID,
                STRINGS.ITEMS.BONES.CRITTERSDROPBONES_BONE.NAME,
                STRINGS.ITEMS.BONES.CRITTERSDROPBONES_BONE.DESC,
                1f,
                true,
                Assets.GetAnim("cdb_bone_kanim"),
                "object",
                Grid.SceneLayer.BuildingBack,
                EntityTemplates.CollisionShape.RECTANGLE,
                0.8f,
                0.33f,
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

        public string[] GetDlcIds()
        {
            return DlcManager.AVAILABLE_ALL_VERSIONS;
        }

        public void OnPrefabInit(GameObject inst)
        {
            inst.AddOrGet<TieredItem>().tiers = new List<TieredItem.Tier>()
            {
                new TieredItem.Tier()
                {
                    minimumMass = 0f,
                    anim = "small"
                },
                new TieredItem.Tier()
                {
                    minimumMass = 3f,
                    anim = "object"
                },
                new TieredItem.Tier()
                {
                    minimumMass = 10f,
                    anim = "large"
                }
            };
        }

        public void OnSpawn(GameObject inst)
        {
            Log.Debuglog(inst.GetComponent<KPrefabID>().PrefabID());
        }
    }
}