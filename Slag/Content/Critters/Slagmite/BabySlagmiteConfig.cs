using UnityEngine;

namespace Slag.Content.Critters.Slagmite
{
    public class BabySlagmiteConfig : IEntityConfig
    {
        public const string ID = "Beached_BabySlagmite";

        public GameObject CreatePrefab()
        {
            var prefab = SlagmiteConfig.CreateMite(
                ID,
                STRINGS.CREATURES.SPECIES.SLAGMITE.BABY.NAME,
                STRINGS.CREATURES.SPECIES.SLAGMITE.BABY.DESC,
                30f,
                "baby_slagmite_kanim",
                false);

            EntityTemplates.ExtendEntityToBeingABaby(prefab, SlagmiteConfig.ID);

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
            inst.GetComponent<KBatchedAnimController>().animWidth = -1f; // yeah i animated it facing the wrong way oop
        }
    }
}
