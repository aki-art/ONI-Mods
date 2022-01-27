using FUtility;

namespace CrittersDropBones.Items
{
    public class TieredItem : KMonoBehaviour
    {
        [MyCmpReq]
        private EntitySplitter splitter;

        [MyCmpReq]
        private PrimaryElement primaryElement;

        [MyCmpReq]
        private KBatchedAnimController kbac;

        private HashedString GetAnimForMass(float mass)
        {
            switch(mass)
            {
                case float n when n > 10f: return "large";
                case float n when n > 3f: return "object";
                default: return "small";
            }
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            splitter.Subscribe((int)GameHashes.SplitFromChunk, OnSplit);
            splitter.Subscribe((int)GameHashes.Absorb, OnAbsorb);
            UpdateAnim();
        }

        private void UpdateAnim()
        {
            kbac.Play(GetAnimForMass(primaryElement.Mass));
        }

        private void OnAbsorb(object obj)
        {
            UpdateAnim();
        }

        private void OnSplit(object obj)
        {
            UpdateAnim();
        }
    }
}
