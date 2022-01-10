namespace DecorPackB.Buildings.Fountain
{
    public class FountainArtable : Sculpture
    {
        public string GetAnim()
        {
            return stages.Find(s => s.id == CurrentStage).anim;
        }
    }
}
