namespace DecorPackB.Content.ModDb
{
	public class BigFossilVariant(string id, string name, string description, string animFile, Tag requiredItemId, bool hangable) : Resource(id, name)
	{
		public readonly Tag requiredItemId = requiredItemId;
		public readonly bool hangable = hangable;
		public readonly string description = description;
		public readonly string animFile = animFile;
	}
}
