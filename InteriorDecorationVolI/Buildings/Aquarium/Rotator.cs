using System.Collections.Generic;
using Harmony;
using UnityEngine;

namespace InteriorDecorationv1.Buildings.Aquarium
{
    public class Rotator : KMonoBehaviour
    {
        public const float BASE_DEGREES_PER_SEC = 360;
        public Vector2 direction;

        [MyCmpGet] private KBatchedAnimController AnimController;
        [MyCmpGet] private ChoreConsumer ChoreConsumer;

        private List<ChoreTable.Entry> _entries;
        private Traverse _trav;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Subscribe((int) GameHashes.Landed, _ => StopRotation());
        }

        private void Update()
        {
            if(AnimController != null)
            {
                // Multiply speed by y component divided by minimum y
                // Rotate in direction of explosion (+ is CCW, - is CW)
                var scale = -Mathf.Sign(direction.x) * (direction.y / FishYeeterTank.MIN_YEET_DISTANCE);
                // Time.DeltaTime scales with the timescale
                AnimController.Rotation += (scale * BASE_DEGREES_PER_SEC * Time.deltaTime) % 360;
            }
        }

        private void StopRotation()
        {
            Object.Destroy(GetComponent<Rotator>());
            var animController = GetComponent<KBatchedAnimController>();
            if(animController != null)
            {
                animController.Rotation = 0f;
            }
        }
    }
}
