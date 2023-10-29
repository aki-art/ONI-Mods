namespace Twitchery.Content
{
	public class Adventures : ResourceSet<Adventure>
	{
		public Adventure spaceAbduction;

		public Adventures()
		{
			spaceAbduction = new Adventure("SpaceAbduction")
				.AddCard(StoryCards.ABDUCTORS_MERMALADE)
				.AddCard(StoryCards.ABDUCTORS_SCARY)
				.AddCard(StoryCards.ABDUCTORS_GLOBS)

				.AddCard(StoryCards.TEA_TIME);
		}
	}
}
