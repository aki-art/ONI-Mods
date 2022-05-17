namespace Slag.Content.Critters
{
    public class AutoMineableCreature : KMonoBehaviour
    {
        [MyCmpReq]
        private MineableCreature mineableCreature;

        [MyCmpReq]
        private KPrefabID kPrefabID;

        public float shellIntegrity = 100f;

        public bool Mineable()
        {
            return mineableCreature.IsMineable();
        }

        public void Reserve()
        {
            kPrefabID.AddTag(GameTags.Creatures.Stunned);
            kPrefabID.AddTag(ModAssets.Tags.beingMined);
        }

        public void Break(float amount = 1f)
        {
            shellIntegrity -= amount;

            if (shellIntegrity < 0)
            {
                mineableCreature.allowMining = false;
                kPrefabID.RemoveTag(GameTags.Creatures.Stunned);
                kPrefabID.RemoveTag(ModAssets.Tags.beingMined);
            }
        }
    }
}
