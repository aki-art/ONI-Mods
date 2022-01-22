using UnityEngine;

namespace FUtility.OverrideAnim
{
    public abstract class AnimKey
    {
        public readonly float time;
        public float length;

        private AnimKey(float time)
        {
            this.time = time;
        }

        public abstract void Update(KBatchedAnimController kbac, float dt, float elapsed);

        public abstract void Stop(KBatchedAnimController kbac);

        public abstract void Start(KBatchedAnimController kbac);

        public class Pos : AnimKey
        {
            public readonly Vector3 position;
            private readonly Easing.Type easing;
            private Vector3 startOffset;

            public Pos(float time, Vector3 position, Easing.Type easing) : base(time)
            {
                this.position = position;
                this.easing = easing;
            }

            public override void Start(KBatchedAnimController kbac)
            {
                startOffset = kbac.Offset;
            }

            public override void Stop(KBatchedAnimController kbac)
            {
                kbac.Offset = startOffset;
            }

            public override void Update(KBatchedAnimController kbac, float dt, float elapsed)
            {
                kbac.Offset = Vector3.Lerp(startOffset, position, elapsed / length);
            }

            public override string ToString()
            {
                return $"[{time}] : Position {position} ({easing})";
            }
        }

        public class Anim : AnimKey
        {
            private readonly string anim;
            private readonly string animOverride;
            private readonly KAnim.PlayMode playMode;
            private readonly float speed;

            public Anim(float time, string anim, string animOverride, KAnim.PlayMode playMode, float speed) : base(time)
            {
                this.anim = anim;
                this.animOverride = animOverride;
                this.playMode = playMode;
                this.speed = speed;
            }

            public override void Start(KBatchedAnimController kbac)
            {
                //throw new System.NotImplementedException();
            }

            public override void Stop(KBatchedAnimController kbac)
            {
                //throw new System.NotImplementedException();
            }

            public override void Update(KBatchedAnimController kbac, float dt, float elapsed)
            {
                //throw new System.NotImplementedException();
            }


            public override string ToString()
            {
                return $"[{time}] :  Anim {anim}/{animOverride} ({playMode}), speed: {speed}";
            }
        }
    }
}
