namespace SpookyPumpkinSO.Content
{
	public class FriendlyPumpkinFace(string id, string name, int numericId, KAnimFile animFile) : Resource(id, name)
	{
		public int numericId = numericId; // for legacy reason
		public KAnimFile animFile = animFile;
	}
}
