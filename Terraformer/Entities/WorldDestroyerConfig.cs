using UnityEngine;

namespace Terraformer.Entities
{
    // This is an invisible entity controlling the destruction of a planet
    public class WorldDestroyerConfig : IEntityConfig
    {
        public const string ID = Mod.PREFIX + "WorldDestroyer";
        public string[] GetDlcIds()
        {
            return DlcManager.AVAILABLE_EXPANSION1_ONLY;
        }

        public GameObject CreatePrefab()
        {
            GameObject gameObject = EntityTemplates.CreateBasicEntity(
                ID,
                "World Destroyer",
                "",
                1f,
                false,
                Assets.GetAnim("spark_radial_high_energy_particles_kanim"),
                "travel_pre",
                Grid.SceneLayer.FXFront2);


            gameObject.AddComponent<WorldDestroyer>();
            Assets.AddPrefab(gameObject.GetComponent<KPrefabID>());
            return gameObject;
        }

        public void OnPrefabInit(GameObject inst)
        {
        }

        public void OnSpawn(GameObject inst)
        {
        }
    }
}
