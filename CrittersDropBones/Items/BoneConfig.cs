using System.Collections.Generic;
using UnityEngine;

namespace CrittersDropBones.Items
{
    public class BoneConfig : IEntityConfig
    {
        public static string ID = "CDB_MediumBone";

        public GameObject CreatePrefab()
        {
            GameObject prefab = EntityTemplates.CreateLooseEntity(
                ID,
                STRINGS.ITEMS.BONES.CDB_MEDIUMBONE.NAME,
                STRINGS.ITEMS.BONES.CDB_MEDIUMBONE.DESC,
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

            //prefab.GetComponent<KPrefabID>().AddTag(GameTags.Organics, false);
            //prefab.GetComponent<KPrefabID>().AddTag(ModAssets.Tags.Bone, false);
            prefab.AddOrGet<EntitySplitter>();
            prefab.AddOrGet<TieredItem>();
            prefab.AddOrGet<SimpleMassStatusItem>();

            EntityTemplates.CreateAndRegisterCompostableFromPrefab(prefab);

            return prefab;
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst) { }

        public void OnSpawn(GameObject inst) { }
    }
}