using Slag.Cmps;
using UnityEngine;

namespace Slag.Content.Entities
{
    public class EggComet : Comet
    {
        [SerializeField]
        public Vector3 impactOffset;

        [SerializeField]
        public Vector3 target;

        [SerializeField]
        public bool aimAtTarget;

        [SerializeField]
        public float angleVariation;

        public override void RandomizeVelocity()
        {
            // move towards a target position
            if (aimAtTarget)
            {
                var dir = target - transform.position;
                var angle = -Vector3.Angle(Vector3.left, dir); // 0 is left

                spawnAngle = new Vector2(angle - angleVariation, angle + angleVariation);
            }

            base.RandomizeVelocity();
        }

        // The first 2 per save file are guaranteed to be an egg
        // from then on it'n RNG between eggs and omelettes
        private string GetPrefabTag()
        {
            if (ModSaveData.Instance.mitiorsSpawned < 2)
            {
                return craterPrefabs[0];
            }
            else
            {
                return craterPrefabs[Random.Range(0, craterPrefabs.Length)];
            }
        }

        protected override void SpawnCraterPrefabs()
        {
            if (craterPrefabs != null && craterPrefabs.Length != 0)
            {
                var cell = Grid.PosToCell(this);

                if (Grid.IsValidCell(Grid.CellAbove(cell)))
                {
                    cell = Grid.CellAbove(cell);
                }

                var prefabTag = GetPrefabTag();
                var position = Grid.CellToPos(cell) + impactOffset;

                var gameObject = Util.KInstantiate(Assets.GetPrefab(prefabTag), position);
                gameObject.GetComponent<KBatchedAnimController>().FlipX = GetComponent<KBatchedAnimController>().FlipX;
                gameObject.SetActive(true);

                if (prefabTag == CookedEggConfig.ID)
                {
                    var eggShell = Util.KInstantiate(Assets.GetPrefab(EggShellConfig.ID), position);
                    eggShell.SetActive(true);
                    eggShell.GetComponent<PrimaryElement>().Mass = 0.33f;

                }
            }

            ModSaveData.Instance.mitiorsSpawned++;
            Util.KDestroyGameObject(gameObject);
        }
    }
}
