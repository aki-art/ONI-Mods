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
        public float targetAngleMargin;

        public override void RandomizeVelocity()
        {
            // move towards a target position
            if (target != null)
            {
                var dir = target - transform.position;
                var angle = -Vector3.Angle(Vector3.left, dir); // 0 is left

                spawnAngle = new Vector2(angle - targetAngleMargin, angle + targetAngleMargin);
            }

            base.RandomizeVelocity();
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

                var gameObject = Util.KInstantiate(Assets.GetPrefab(craterPrefabs[Random.Range(0, craterPrefabs.Length)]), Grid.CellToPos(cell));
                gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
                gameObject.transform.position += impactOffset;
                gameObject.GetComponent<KBatchedAnimController>().FlipX = GetComponent<KBatchedAnimController>().FlipX;
                gameObject.SetActive(true);
            }

            Util.KDestroyGameObject(gameObject);
        }
    }
}
