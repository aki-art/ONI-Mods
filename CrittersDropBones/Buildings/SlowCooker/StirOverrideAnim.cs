using FUtility.OverrideAnim;
using UnityEngine;

namespace CrittersDropBones.Buildings.SlowCooker
{
    internal class StirOverrideAnim : OverrideAnim
    {
        protected override void CreateTimeline(Builder animBuilder)
        {
            animBuilder
                .Move(0f, Vector3.zero)
                .Move(10f, new Vector3(0, 1f))
                .Move(20f, new Vector3(1, 0));
        }

        protected override float GetAnimationLength() => 20f;
    }
}
