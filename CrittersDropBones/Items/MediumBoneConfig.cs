using UnityEngine;

namespace CrittersDropBones.Items
{
    public class MediumBoneConfig : IEntityConfig
    {
        public static string ID = "CDB_MediumBone";

        public GameObject CreatePrefab()
        {
            return BoneEntityTemplate.CreateBone(
                ID,
                STRINGS.ITEMS.BONES.CDB_MEDIUMBONE.NAME,
                STRINGS.ITEMS.BONES.CDB_MEDIUMBONE.DESC,
                1f,
                "mediumbone_kanim",
                0.8f,
                0.33f
                );
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst) { }

        public void OnSpawn(GameObject inst) { }
    }
}