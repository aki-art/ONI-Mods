using KSerialization;
using UnityEngine;

namespace FUtility.OverrideAnim
{
    // TODO: full serialization
    [SerializationConfig(MemberSerialization.OptIn)]
    public abstract class OverrideAnim : KMonoBehaviour, ISaveLoadable
    {
        [Serialize]
        public bool paused;

        [MyCmpGet]
        private KBatchedAnimController kbac;

        private readonly Timeline<AnimKey.Pos> positionTimeline = new Timeline<AnimKey.Pos>();
        private readonly Timeline<AnimKey.Anim> animTimeline = new Timeline<AnimKey.Anim>();

        private float animationLength;

        [Serialize]
        private float elapsed;

        [Serialize]
        private bool finished;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            animationLength = GetAnimationLength();
            CreateTimeline(new Builder(this));
            SortTimelines();
        }

        public void Update()
        {
            if (finished || paused || SpeedControlScreen.Instance.IsPaused) return;

            elapsed += Time.deltaTime;

            if(elapsed > animationLength)
            {
                positionTimeline.EndAll(kbac);
                finished = true;
                return;
            }

            positionTimeline.Update(kbac, Time.deltaTime);
        }

        protected abstract float GetAnimationLength();

        protected abstract void CreateTimeline(Builder builder);

        void SortTimelines()
        {
            positionTimeline.Sort(animationLength);
            animTimeline.Sort(animationLength);
        }

        public class Builder
        {
            private readonly OverrideAnim master;

            public Builder(OverrideAnim master)
            {
                this.master = master;
            }

            /// <summary>
            /// Mode the Offset of the KBatchedAnimController component to a position.
            /// </summary>
            /// <param name="time">Start of movement</param>
            /// <param name="position">Position to move towards, relative to the GameObject.</param>
            /// <param name="easing">Easing curve to apply.</param>
            public Builder Move(float time, Vector3 position, Easing.Type easing = Easing.Type.Linear)
            {
                master.positionTimeline.Add(new AnimKey.Pos(time, position, easing));
                return this;
            }

            /// <summary>
            /// Change the animation the KBatchedAnimController is currently playing.
            /// </summary>
            /// <param name="time">Time of switching</param>
            /// <param name="anim">Animation name (this is the animation within the kanim to play)</param>
            /// <param name="animFileName">Name of the animation file, suffixed with "_kanim".</param>
            /// <param name="playMode"></param>
            /// <param name="speed">Animation speed. Negative numbers can be used to play backwards.</param>
            /// <returns></returns>
            public Builder PlayAnim(float time, string anim, string animFileName = "", KAnim.PlayMode playMode = KAnim.PlayMode.Loop, float speed = 1f)
            {
                master.animTimeline.Add(new AnimKey.Anim(time, anim, animFileName, playMode, speed));
                return this;
            }
        }
    }
}

