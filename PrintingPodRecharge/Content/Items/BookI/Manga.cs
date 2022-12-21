namespace PrintingPodRecharge.Content.Items.BookI
{
    public class Manga : SelfImprovement
    {
        public override bool CanUse(MinionIdentity minionIdentity) => true;

        public override void OnUse(Worker worker)
        {
        }
    }
}
