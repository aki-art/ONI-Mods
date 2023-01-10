namespace DecorPackB.Content.Scripts
{
    public class Pot : Sculpture
    {
        public void SetRandomStage()
        {
            var potentialStages = Db.GetArtableStages().GetPrefabStages(this.PrefabID());

            potentialStages.RemoveAll(stage => stage.statusItem.StatusType != Database.ArtableStatuses.ArtableStatusType.LookingGreat);
            var selectedStage = potentialStages.GetRandom();

            SetStage(selectedStage.id, false);
        }
    }
}
