using UnityEngine;

namespace FUtility.OverrideAnim
{
    public class TestAnim : OverrideAnim
    {
        protected override float GetAnimationLength() => 10f;

        protected override void CreateTimeline(Builder builder)
        {
            builder
                .Move(3f, new Vector3(3, 3))
                .PlayAnim(5f, "working_pre", "anim_interacts_fabricator_generic_kanim")
                .Move(6f, new Vector3(3, 2), Easing.Type.OutBack);
        }
    }
}
