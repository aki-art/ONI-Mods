using UnityEngine;

namespace CrittersDropBones.Items
{
    public class SmallBoneConfig : IEntityConfig
    {
        public static string ID = "CDB_SmallBone";

        public GameObject CreatePrefab()
        {
            return BoneEntityTemplate.CreateBone(
                ID,
                STRINGS.ITEMS.BONES.CDB_SMALLBONE.NAME,
                STRINGS.ITEMS.BONES.CDB_SMALLBONE.DESC,
                0.5f,
                "smallbone_kanim",
                0.8f,
                0.25f);
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst) { }

        public void OnSpawn(GameObject inst) { }
    }
}