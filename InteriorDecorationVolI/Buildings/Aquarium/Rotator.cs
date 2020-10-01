using UnityEngine;

namespace InteriorDecorationv1.Buildings.Aquarium
{
    public class Rotator : KMonoBehaviour
    {
        public const float BASE_DEGREES_PER_SEC = 360;

        [SerializeField]
        private Vector2 direction;

        [SerializeField]
        private float scale;

        [MyCmpGet] private KBatchedAnimController animController;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Subscribe((int) GameHashes.Landed, _ => StopRotation());
        }

        public void SetVec(Vector2 vec)
        {
            direction = vec;
            // Multiply speed by y component divided by minimum y
            // Rotate in direction of explosion (+ is CCW, - is CW)
            scale = -Mathf.Sign(direction.x) * (direction.y / Aquarium.MIN_YEET_DISTANCE);
            enabled = true;
        }

        private void Update()
        {
            if(animController != null)
            {
                // Time.DeltaTime scales with the timescale
                animController.Rotation += scale * BASE_DEGREES_PER_SEC * Time.deltaTime % 360;
            }
        }

        private void StopRotation()
        {
            if(animController != null)
            {
                animController.Rotation = 0f;
            }

            enabled = false;
        }
    }
}
