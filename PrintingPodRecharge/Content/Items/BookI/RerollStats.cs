namespace PrintingPodRecharge.Content.Items.BookI
{
    public class RerollStats : SelfImprovement
    {
        public override bool CanUse(MinionIdentity minionIdentity) => true;

        public override void OnUse(Worker worker)
        {
            // reroll all stats
        }
    }
}
